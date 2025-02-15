using System.Collections.Generic;
using System.Linq;
using Abstracts;
using Defaults;
using Interfaces;
using Services;
using UnityEngine;
using Zenject;
using UniRx;

namespace Model
{
    public class NotesPush
    {
        private IGetNotesPool _getNotesPool;
        private IJudgeNotes _judge;
        private MusicStatus _status;

        private Dictionary<LaneName, bool> _pushing = new()
        {
            { LaneName.OuterRight, false },
            { LaneName.BigRight, false },
            { LaneName.InnerRight, false },
            { LaneName.InnerLeft, false },
            { LaneName.BigLeft, false },
            { LaneName.OuterLeft, false }
        };
        
        [Inject]
        public NotesPush(IGetNotesPool getNotesPool,IJudgeNotes judge,IInputProvider provider, MusicStatus status)
        {
            _getNotesPool = getNotesPool;
            _judge = judge;
            _status = status;

            provider.Push.Subscribe(lane => Push(lane));
            provider.ExitPush.Subscribe(lane => ExitPush(lane));
        }

        public void Push(LaneName lane)
        {
            if (!_pushing[lane])
            {
                DownPush(lane);
                _pushing[lane] = true;
            }
            else
            {
                Pushing(lane);
            }
        }

        public void ExitPush(LaneName lane)
        {
            _pushing[lane] = false;
            var _bestNotes = SearchPool(lane);
            if (_bestNotes != null && _bestNotes.TryGetComponent<ILongNotes>(out var _long) && _long.IsPushed)
            {
                SearchMissNotes(lane,_bestNotes);
                _judge.Judge(JudgeType.Miss);
                _long.Miss();
            }
        }

        /// <summary>
        /// キーが押し続けられているときの処理
        /// </summary>
        /// <param name="lane"></param>
        private void Pushing(LaneName lane)
        {
            var _bestNotes = SearchPool(lane);
            if (_bestNotes != null &&
                _bestNotes.Position.z < _status.DelayPosition &&
                _bestNotes.TryGetComponent<ILongNotes>(out var _long) && _long.IsPushed)
            {
                SearchMissNotes(lane, _bestNotes);
                _judge.Judge(JudgeType.Perfect);
                _bestNotes.Push();
            }
        }
        
        /// <summary>
        /// キーがおされたときの処理
        /// </summary>
        /// <param name="lane">どこのレーンが押されたか</param>
        private void DownPush(LaneName lane)
        {
            var _bestNotes = SearchPool(lane);

            if (_bestNotes != null)
            {
                SearchMissNotes(lane,_bestNotes);
                var _type = _judge.Judge(_bestNotes.Position);
                if (_type == JudgeType.Miss && _bestNotes.TryGetComponent<ILongNotes>(out var _long))
                {
                    _long.Miss();
                }
                else
                {
                    _bestNotes.Push();
                }
            }
        }
        
        /// <summary>
        /// ジャッチ可能なノーツを探索
        /// </summary>
        /// <param name="lane">探索するレーン</param>
        /// <returns>結果</returns>
        private NotesCore SearchPool(LaneName lane)
        {
            NotesCore _bestNotes = null;
            foreach (var _notes in _getNotesPool.GetPool()
                         .Where(n => n.Active && 
                                     n.MyLane == lane &&
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

        /// <summary>
        /// Pushするノーツ以下のノーツを判定
        /// </summary>
        /// <param name="lane">Pushするノーツのレーン</param>
        /// <param name="bestNotes">Pushするノーツの</param>
        private void SearchMissNotes(LaneName lane,INotes bestNotes)
        {
            foreach (var _notes in _getNotesPool.GetPool()
                         .Where(n => n.Active && n.MyLane == lane &&
                                     n.Position.z < bestNotes.Position.z))
            {
                _judge.Judge(JudgeType.Miss);
                _notes.Push();
            }
        }
    }
}