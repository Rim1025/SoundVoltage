using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Abstracts;
using Cysharp.Threading.Tasks;
using Defaults;
using Interfaces;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using View.Notes;
using Zenject;
using Unit = UniRx.Unit;

namespace Model
{
    public class NotesSpawner
    {
        private Subject<Unit> _startAudio = new();
        public IObservable<Unit> StartAudio => _startAudio;
        
        private List<List<string>> _notesList = new();
        
        private IAddNotesPool _addNotesPool;
        private IGetNotesPool _getNotesPool;
        private IJudgeNotes _judge;
        private NotesFactory _factory;
        private MusicStatus _status;
        private GameObject _notesParent;

        private INotes _endNotes;
        private IDisposable _updateDisposable;
        private IDisposable _endDisposable;
        private int _bpm = GameData.Bpm;
        private List<NotesCore> _spawningNotesList = new();

        [Inject]
        public NotesSpawner(IAddNotesPool addNotesPool, IGetNotesPool getNotesPool, IJudgeNotes judge,
            NotesFactory factory, MusicStatus musicStatus)
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
            Debug.Log(_notesList.Count);

            float _time = 0;
            int _timeCounter = 0;
            
            if (!float.TryParse(_notesList[0][7], out var time))
            {
                time = 0;
            }
            float _audioTime = time - GameData.LanePositions[LaneName.OuterLeft].z / _status.NotesSpeed;
            //NOTE: 講読後に実行するため
            GameEvents.UpdateGame
                .Select(t => _time += t)
                .Where(t =>t > -_audioTime)
                .First()
                .Subscribe(_ =>
                {
                    _time = 0;
                    _startAudio.OnNext(Unit.Default);
                });
            GameEvents.UpdateGame
                .Select(t => _time += t)
                .Where(t =>t > _audioTime)
                .First()
                .Subscribe(_ =>
                {
                    _time = 0;
                    
                    _updateDisposable = GameEvents.UpdateGame
                        .Select(t => _time += t)
                        .Where(_ => _time > (float)60 / _bpm)
                        .Subscribe(_ =>
                        {
                            _time -= (float)60 / _bpm;
                            CsvSpawn(_timeCounter);
                            _timeCounter++;
                            if (_endNotes != null&& _endNotes.Position.z < -GameData.JudgePosition[^1] + _status.DelayPosition && _endNotes.Active)
                            {
                                _endNotes.Push();
                            }
                            // 通り過ぎたときの処理
                            foreach (var _notes in _getNotesPool.GetPool().Where(n =>
                                         n.Position.z < -GameData.JudgePosition[^1] + _status.DelayPosition && n.Active))
                            {
                                _notes.Push();
                                _judge.Judge(JudgeType.Miss);
                            }
                        });
                });
            
        }
        
        /// <summary>
        /// csvに基づいてノーツ生成
        /// </summary>
        /// <param name="time"></param>
        private void CsvSpawn(int time)
        {
            if (_endNotes != null)
            {
                return;
            }
            
            TimeNotesAction();
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
        private NotesCore SpawnNotes(LaneName lane, NotesType type, MusicStatus status)
        {
            if (SpawnNotesAction(lane,type, status))
            {
                return null;
            }
            
            foreach (var _n in _getNotesPool.GetPool().Where(n =>
                     {
                         //NOTE: Longノーツにノーマルノーツの挙動ができないため
                         if (type == NotesType.Long)
                         {
                             return !n.Active && n.Type == NotesType.Long;
                         }
                         return !n.Active && n.Type != NotesType.Long;
                     }))
            {
                _n.Activate(lane, status);
                return _n;
            }


            var _notes = _factory.Create(type, _notesParent.transform);
            _notes.Activate(lane, status);

            _addNotesPool.AddNotes(_notes);
            return _notes;
        }

        private void TimeNotesAction()
        {
            foreach (var _n in _spawningNotesList
                         .Select(n =>
                         {
                             n.TryGetComponent<ILongNotes>(out var _notes);
                             return _notes;
                         })
                         .Where(n => n != null))
            {
                _n.Grow();
            }
        }

        /// <summary>
        /// ノーツ生成時の個別処理
        /// </summary>
        /// <param name="lane"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns>処理中断かどうか</returns>
        private bool SpawnNotesAction(LaneName lane,NotesType type,MusicStatus status)
        {
            var _endFlag = false;
            _spawningNotesList.RemoveAll(n =>
            {
                var _longEnd = type == NotesType.LongEnd && n.MyLane == lane && n.TryGetComponent<ILongNotes>(out _);
                if (type == NotesType.LongEnd)
                {
                    _endFlag = true;
                }
                return _longEnd;
            });
            
            // 曲終了時の処理
            if (type == NotesType.End)
            {
                _endNotes = _factory.Create(NotesType.End, _notesParent.transform);
                _endDisposable = GameEvents.UpdateGame
                    .Where(_ => !_endNotes.Active)
                    .Subscribe(_ =>
                    {
                        Debug.Log("End");
                        _updateDisposable.Dispose();
                        _endDisposable.Dispose();
                        // 終了処理
                    });
                _endFlag = true;
            }
            return _endFlag;
        }
    }
}