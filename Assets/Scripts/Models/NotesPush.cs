using System.Linq;
using Defaults;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Model
{
    public class NotesPush
    {
        private IGetNotesPool _getNotesPool;
        private IJudgeNotes _judge;
        
        [Inject]
        public NotesPush(IGetNotesPool getNotesPool,IJudgeNotes judge)
        {
            _getNotesPool = getNotesPool;
            _judge = judge;
        }
        
        /// <summary>
        /// キーがおされたときの処理
        /// </summary>
        /// <param name="lane">どこのレーンが押されたか</param>
        public void Push(LaneName lane)
        {
            var _bestNotes = SearchPool(lane);

            if (_bestNotes != null)
            {
                SearchMissNotes(lane,_bestNotes);
                _judge.Judge(_bestNotes.Position);
                _bestNotes.Push();
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