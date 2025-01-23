using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Defaults;
using Interfaces;
using Services;
using UnityEngine;
using UniRx;
using Zenject;

namespace Model
{
    /// <summary>
    /// ノーツの制御
    /// </summary>
    public class Lane : ILane

    {
        private List<List<string>> _notesList;
        private int _bpm = GameData.Bpm;

        private MusicStatus _status;
        private NotesFactory _factory;
        private GameObject _notesParent;
        private IJudgeNotes _judgeNotes;

        public List<INotes> NotesPool { get; private set; } = new();

        [Inject]
        public Lane(NotesFactory factory, IJudgeNotes judgeNotes)
        {
            _factory = factory;
            _judgeNotes = judgeNotes;
            _notesParent = new GameObject("NotesParent");
            _status = JsonReader.Read();
            _notesList = TrackReader.Read(_status);
            //NOTE: 1行目は説明のため消す
            _notesList.RemoveAt(0);

            float _time = 0;
            int _timeCounter = 0;
            GameEvents.StartGame
                .Subscribe(_ =>
                {

                });
            GameEvents.UpdateGame
                .Select(t => _time += t)
                .Where(_ => _time > (float)60 / _bpm)
                .Subscribe(_ =>
                {
                    _time -= (float)60 / _bpm;
                    Spawn(_timeCounter);
                    _timeCounter++;
                });
        }

        /// <summary>
        /// csvに基づいてノーツ生成
        /// </summary>
        /// <param name="time"></param>
        private void Spawn(int time)
        {
            _bpm = int.TryParse(_notesList[time][GameData.BpmLane], out var _result) ? _result : _bpm;
            foreach (var _notes in _notesList[time]
                         .Select((value, index) => new { value, index })
                         .Where(n => n.index < GameData.BpmLane))
            {
                if (int.TryParse(_notes.value, out var _val))
                {
                    CheckObjectPool((LaneName)Enum.ToObject(typeof(LaneName), _notes.index),
                        (NotesType)Enum.ToObject(typeof(NotesType), _val),
                        _status.NotesSpeed);
                }
            }

            Debug.Log(_bpm);
        }

        /// <summary>
        /// 非アクティブオブジェクトの再利用
        /// </summary>
        /// <param name="lane">召喚するレーン名</param>
        /// <param name="type">ノーツの種類</param>
        /// <returns>生成したノーツ</returns>
        private INotes CheckObjectPool(LaneName lane, NotesType type, float speed)
        {
            foreach (var _n in NotesPool)
            {
                if (!_n.Active)
                {
                    _n.Activate(lane, speed);
                    return _n;
                }
            }

            // NOTE: ロングノーツをはじく
            if ((int)type >= 2)
            {
                return null;
            }


            var _notes = _factory.Create(type, _notesParent.transform);
            NotesPool.Add(_notes);
            _notes.Activate(lane, speed);
            Debug.Log(_notes.Type + lane.ToString());
            return _notes;
        }

        public void Push(LaneName lane)
        {
            var _bestNotes = SearchPool(lane);

            if (_bestNotes != null)
            {
                _judgeNotes.Judge(_bestNotes.Position);
            }
        }

        /// <summary>
        /// ジャッチ可能なノーツを探索
        /// </summary>
        /// <param name="lane">探索するレーン</param>
        /// <returns>結果</returns>
        private INotes SearchPool(LaneName lane)
        {
            INotes _bestNotes = null;
            foreach (var _notes in NotesPool
                         .Where(n => n.Active && n.MyLane == lane &&
                                     Mathf.Abs(n.Position.z) < GameData.JudgePosition[(int)JudgeType.Miss]))
            {
                if (_bestNotes != null)
                {
                    if (Mathf.Abs(_notes.Position.z) < Mathf.Abs(_bestNotes.Position.z))
                        _bestNotes = _notes;
                }
                else
                {
                    _bestNotes = _notes;
                }
            }

            return _bestNotes;
        }

        public void OnPush(LaneName lane)
        {

        }
    }
}