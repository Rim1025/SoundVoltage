using System;
using System.Collections.Generic;
using System.Linq;
using Abstracts;
using Defaults;
using Interfaces;
using Services;
using UnityEngine;
using UniRx;
using Zenject;
using Unit = UniRx.Unit;

namespace Model
{
    /// <summary>
    /// ノーツの生成
    /// </summary>
    public class NotesSpawner: IEndFlag,INotesSpawner
    {
        private Subject<Unit> _startAudio = new();
        public IObservable<Unit> StartAudio => _startAudio;
        private Subject<Unit> _endSubject = new();
        public IObservable<Unit> EndSubject => _endSubject;
        
        private List<List<string>> _notesList = new();
        
        private IAddNotesPool _addNotesPool;
        private IGetNotesPool _getNotesPool;
        private IJudgeNotes _judge;
        private NotesFactory _factory;
        private MusicStatus _status;
        private GameObject _notesParent;
        
        private IDisposable _updateDisposable;
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

            float _musicTime = 0;
            float _notesTime = 0;
            int _timeCounter = 0;
            
            if (!float.TryParse(_notesList[0][7], out var _musicStartTime))
            {
                _musicStartTime = 0;
            }
            float _audioStartTime = _musicStartTime - GameData.LanePositions[LaneName.OuterLeft].z / _status.NotesSpeed;
            //NOTE: 講読後に実行するため
            GameEvents.UpdateGame
                .Select(t => _musicTime += t)
                .Where(_=>_musicTime > -_audioStartTime)
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    _startAudio.OnNext(Unit.Default);
                });
            GameEvents.UpdateGame
                .Select(t => _notesTime += t)
                .Where(_ => _notesTime > _audioStartTime)
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    _notesTime = 0;
                    _updateDisposable = GameEvents.UpdateGame
                        .Select(t => _notesTime += t)
                        .Where(_ => _notesTime > (float)60 / _bpm)
                        .Subscribe(_ =>
                        {
                            _notesTime -= (float)60 / _bpm;
                            CsvSpawn(_timeCounter);
                            _timeCounter++;
                            // 通り過ぎたときの処理
                            foreach (var _notes in _getNotesPool.GetPool().Where(n =>
                                         n.Position.z < -GameData.JudgePosition[(int)JudgeType.Good] + _status.DelayPosition && n.Active))
                            {
                                if (_notes.TryGetComponent<ILongNotes>(out var _long))
                                {
                                    _long.Miss();
                                }
                                else
                                {
                                    _notes.Push();
                                }
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
            if (time >= _notesList.Count)
            {
                End();
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

        /// <summary>
        /// ノーツ生成の可能性があるタイミングの個別処理
        /// </summary>
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
            return _endFlag;
        }

        /// <summary>
        /// 終了時の処理
        /// </summary>
        private void End()
        {
            var _endTime = GameData.LanePositions[LaneName.OuterLeft].z / _status.NotesSpeed;
            var _time = 0f;
            GameEvents.UpdateGame
                .Select(t => _time += t)
                .Where(_=>_time > _endTime + 1)
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    _updateDisposable.Dispose();
                    // 終了処理
                    _endSubject.OnNext(Unit.Default);
                    _endSubject.OnCompleted();
                    _endSubject = new Subject<Unit>();
                });
        }
    }
}