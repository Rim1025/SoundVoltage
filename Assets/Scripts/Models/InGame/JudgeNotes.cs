using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defaults;
using Interfaces;
using Services;
using UniRx;
using Zenject;

namespace Model
{
    /// <summary>
    /// ジャッチの種類
    /// </summary>
    public enum JudgeType
    {
        Perfect,
        Good,
        Miss
    }

    /// <summary>
    /// ノーツの位置から判定を行う
    /// </summary>
    public class JudgeNotes: IJudgeNotes
    {
        private Dictionary<JudgeType, float> _judgePosition;
        private Subject<JudgeType> _calJudge = new();
        private MusicStatus _status;
        public IObservable<JudgeType> CalJudge => _calJudge;
    
        [Inject]
        public JudgeNotes(MusicStatus status, IGetNotesPool getNotesPool)
        {
            _status = status;
            _judgePosition = new Dictionary<JudgeType, float>()
            {
                { JudgeType.Perfect, GameData.JudgePosition[JudgeType.Perfect]},
                { JudgeType.Good ,GameData.JudgePosition[JudgeType.Good]},
                { JudgeType.Miss ,GameData.JudgePosition[JudgeType.Miss]}
            };

            // 通り過ぎた時の処理
            GameEvents.UpdateGame.Subscribe(t =>
            {
                foreach (var _notes in getNotesPool.GetPool()
                             .Where(n =>
                             n.Position.z < -GameData.JudgePosition[JudgeType.Good] + _status.DelayPosition && n.Active))
                {
                    if (_notes.TryGetComponent<ILongNotes>(out var _long))
                    {
                        _long.Miss();
                    }
                    else
                    {
                        _notes.Push();
                    }
                    Judge(JudgeType.Miss);
                }
            });
        }
        
        /// <summary>
        /// 判定
        /// </summary>
        /// <param name="position">ノーツの位置</param>
        /// <returns>判定結果</returns>
        public JudgeType Judge(Vector3 position)
        {
            JudgeType _judgeType = JudgeType.Miss;
            foreach (JudgeType _type in Enum.GetValues(typeof(JudgeType)))
            {
                // より良い判定なら_judgeTypeに格納
                if (Mathf.Abs(position.z) <_judgePosition[_type] + _status.DelayPosition &&
                    _judgePosition[_type]<_judgePosition[_judgeType])
                {
                    _judgeType = _type;
                }
            }
            _calJudge.OnNext(_judgeType);
            return _judgeType;
        }

        /// <summary>
        /// 強制的に特定のタイプで判定
        /// </summary>
        /// <param name="type">判定したいタイプ</param>
        /// <returns>結果</returns>
        public JudgeType Judge(JudgeType type)
        {
            _calJudge.OnNext(type);
            return type;
        }
    }
}