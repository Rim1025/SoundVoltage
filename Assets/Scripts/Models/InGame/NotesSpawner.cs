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
        // 楽曲開始(最初のノーツ到達)のフラグ
        private Subject<Unit> _startAudio = new();
        public IObservable<Unit> StartAudio => _startAudio;
        // 楽曲終了(最後のノーツ到達のフラグ
        private Subject<Unit> _endSubject = new();
        public IObservable<Unit> EndSubject => _endSubject;
        
        // csvから読み込んだノーツ出現用のリスト
        private List<List<string>> _notesList = new();
        
        // ノーツプール
        private IAddNotesPool _addNotesPool;
        private IGetNotesPool _getNotesPool;
        // ファクトリー
        private NotesFactory _factory;
        // プレイヤーの設定
        private MusicStatus _status;
        // ノーツの親オブジェクト
        private GameObject _notesParent;
        // 生成を行うUpdate
        private IDisposable _updateDisposable;
        // Bpm
        private int _bpm = GameData.Bpm;
        // LongNotes用の生成途中のノーツリスト
        private List<NotesCore> _spawningNotesList = new();

        [Inject]
        public NotesSpawner(IAddNotesPool addNotesPool, IGetNotesPool getNotesPool,
            NotesFactory factory, MusicStatus musicStatus)
        {
            _addNotesPool = addNotesPool;
            _getNotesPool = getNotesPool;
            _factory = factory;
            _status = musicStatus;
            _notesParent = new GameObject("NotesParent");
            // csv読み込み
            _notesList = TrackReader.Read(_status);
            
            //NOTE: 1行目は説明のため消す
            _notesList.RemoveAt(0);

            // 生成時間管理用
            float _musicTime = 0;
            float _notesTime = 0;
            // csvの生成行管理用
            int _timeCounter = 0;
            
            // 曲の再生タイミング取得
            if (!float.TryParse(_notesList[0][7], out var _musicStartTime))
            {
                // 0秒からノーツが流れる
                _musicStartTime = 0;
            }
            // 曲の再生タイミングとノーツの移動速度から再生開始までの時間計算
            float _audioStartTime = _musicStartTime -
                                    (GameData.LanePositions[LaneName.OuterLeft].z + _status.DelayPosition) / 
                                    _status.NotesSpeed;
            // 再生開始までの時間待って開始
            GameEvents.UpdateGame
                .Select(t => _musicTime += t)
                .Where(_=>_musicTime > -_audioStartTime)
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    _startAudio.OnNext(Unit.Default);
                });
            // 曲再生までの時間が-の場合それだけ待ってのノーツ生成開始
            GameEvents.UpdateGame
                .Select(t => _notesTime += t)
                .Where(_ => _notesTime > _audioStartTime)
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    //NOTE: 60はBpmの定義より
                    _notesTime = 0;
                    _updateDisposable = GameEvents.UpdateGame
                        .Select(t => _notesTime += t)
                        .Where(_ => _notesTime > (float)60 / _bpm)
                        .Subscribe(_ =>
                        {
                            //NOTE: 規定時間を引くことで誤差の生じないように
                            _notesTime -= (float)60 / _bpm;
                            // 生成
                            CsvSpawn(_timeCounter);
                            // 一行進める
                            _timeCounter++;
                        });
                });
            
        }
        
        /// <summary>
        /// csvに基づいてノーツ生成
        /// </summary>
        /// <param name="time"></param>
        private void CsvSpawn(int time)
        {
            // 終了時処理
            if (time >= _notesList.Count)
            {
                End();
                return;
            }
            // 個別処理
            TimeNotesAction();
            // bpm変更のある場合読み取り
            _bpm = int.TryParse(_notesList[time][GameData.LaneCount], out var _result) ? _result : _bpm;
            // csvをレーンの数まで読んで生成
            //NOTE: レーンの数以降はbpm等が記載されているため
            foreach (var _notes in _notesList[time]
                         .Select((value, index) => new { value, index })
                         .Where(n => n.index < GameData.LaneCount))
            {
                // string->int
                if (int.TryParse(_notes.value, out var _val))
                {
                    // int->NotesType
                    var _type = (NotesType)Enum.ToObject(typeof(NotesType), _val);
                    // 生成
                    var _spawnNotes = SpawnNotes((LaneName)Enum.ToObject(typeof(LaneName), _notes.index),
                        _type, _status);
                    // Longなら個別処理
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
            // 個別処理、場合によって終了
            if (SpawnNotesAction(lane,type))
            {
                return null;
            }
            
            // 休止中のノーツがあるなら再利用
            foreach (var _n in _getNotesPool.GetPool()
                         .Where(n =>
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
            
            // 休止中のノーツがないなら生成
            var _notes = _factory.Create(type, _notesParent.transform);
            _notes.Activate(lane, status);
            // オブジェクトプールに追加
            _addNotesPool.AddNotes(_notes);
            return _notes;
        }

        /// <summary>
        /// ノーツ生成の可能性があるタイミングの個別処理
        /// </summary>
        private void TimeNotesAction()
        {
            // Longノーツを伸ばす
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
        /// <returns>処理中断かどうか</returns>
        private bool SpawnNotesAction(LaneName lane,NotesType type)
        {
            // ノーツ生成処理の中断フラグ
            var _endFlag = false;
            // Longノーツ終了なら対応する生成中扱いLongノーツをリストから除外
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
            // 最後のノーツの到達までの時間
            var _endTime = (GameData.LanePositions[LaneName.OuterLeft].z + _status.DelayPosition) / _status.NotesSpeed;
            var _time = 0f;
            // 最後のノーツが到達後終了
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