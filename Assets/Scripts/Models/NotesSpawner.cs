using System;
using System.Collections.Generic;
using System.Linq;
using Defaults;
using Interfaces;
using UnityEngine;
using UniRx;
using Zenject;

namespace Model
{
    public class NotesSpawner
    {
        private List<List<string>> _notesList = new();
        
        private IAddNotesPool _addNotesPool;
        private IGetNotesPool _getNotesPool;
        private IJudgeNotes _judge;
        private NotesFactory _factory;
        private MusicStatus _status;
        private GameObject _notesParent;

        private INotes _endNotes;
        private IDisposable _updateDisposable;
        private int _bpm = GameData.Bpm;
        private List<INotes> _spawningNotesList = new();
        
        [Inject]
        public NotesSpawner(IAddNotesPool addNotesPool,IGetNotesPool getNotesPool,IJudgeNotes judge, NotesFactory factory,MusicStatus musicStatus)
        {
            _addNotesPool = addNotesPool;
            _getNotesPool = getNotesPool;
            _judge = judge;
            _factory = factory;
            _status = musicStatus;
            _notesParent = new GameObject("NotesParent");
            _notesList = TrackReader.Read(_status);
            //NOTE: 1行目は説明のため消す
            _notesList.RemoveAt(0);
            
            float _time = 0;
            int _timeCounter = 0;
            GameEvents.StartGame
                .Subscribe(_ =>
                {

                });
            _updateDisposable = GameEvents.UpdateGame
                .Select(t => _time += t)
                .Where(_ => _time > (float)60 / _bpm)
                .Subscribe(_ =>
                {
                    _time -= (float)60 / _bpm;
                    Spawn(_timeCounter);
                    _timeCounter++;
                    
                    // 通り過ぎたときの処理
                    foreach (var _notes in _getNotesPool.GetPool().Where(n => n.Position.z < -GameData.JudgePosition[^1] + _status.DelayPosition && n.Active))
                    {
                        _notes.Push();
                        _judge.Judge(JudgeType.Miss);
                    }
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
                    var _type = (NotesType)Enum.ToObject(typeof(NotesType), _val);
                    var _spawnNotes = SpawnNotes((LaneName)Enum.ToObject(typeof(LaneName), _notes.index),
                        _type, _status);

                    if (_type == NotesType.Long)
                    {
                        _spawningNotesList.Add(_spawnNotes);
                    }
                }
            }
        }
        
        /// <summary>
        /// ノーツの生成or再利用
        /// </summary>
        /// <param name="lane">召喚するレーン名</param>
        /// <param name="type">ノーツの種類</param>
        /// <param name="status">MusicStatus</param>
        /// <returns>生成したノーツ</returns>
        private INotes SpawnNotes(LaneName lane, NotesType type, MusicStatus status)
        {
            foreach (var _n in _getNotesPool.GetPool())
            {
                if (!_n.Active)
                {
                    _n.Activate(lane, status);
                    return _n;
                }
            }

            foreach (var _n in _spawningNotesList.Where(n => type == NotesType.LongEnd && n.MyLane == lane))
            {
                
            }

            var _notes = _factory.Create(type, _notesParent.transform);
            _addNotesPool.AddNotes(_notes);
            _notes.Activate(lane, status);

            // 曲終了時の処理
            if (type == NotesType.End)
            {
                _endNotes = _notes;
                _updateDisposable.Dispose();
                _updateDisposable = GameEvents.UpdateGame
                    .Where(_ => !_endNotes.Active)
                    .Subscribe(_ =>
                    {
                        _updateDisposable.Dispose();
                        
                        // 終了処理
                    });
            }
            return _notes;
        }
    }
}