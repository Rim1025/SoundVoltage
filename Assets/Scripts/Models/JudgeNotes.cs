using System;
using System.Collections.Generic;
using UnityEngine;
using Defaults;
using Interfaces;
using JetBrains.Annotations;
using UniRx;

namespace Model
{
    public enum JudgeType
    {
        Perfect,
        Good,
        Miss
    }

    public class JudgeNotes: IJudgeNotes
    {
        private Dictionary<JudgeType, float> _judgePosition;
        private Subject<JudgeType> _calScore = new();
        public IObservable<JudgeType> CalScore => _calScore;
    
        public JudgeNotes()
        {
            _judgePosition = new Dictionary<JudgeType, float>()
            {
                { JudgeType.Perfect, GameData.JudgePosition[(int)JudgeType.Perfect] },
                { JudgeType.Good ,GameData.JudgePosition[(int)JudgeType.Good]},
                { JudgeType.Miss ,GameData.JudgePosition[(int) JudgeType.Miss]}
            };
        }
        
        public void Judge(Vector3 position)
        {
            JudgeType _judge = JudgeType.Miss;
            foreach (JudgeType _type in Enum.GetValues(typeof(JudgeType)))
            {
                if (Mathf.Abs(position.z) <_judgePosition[_type] && _judgePosition[_type]<Mathf.Abs(_judgePosition[_judge]))
                {
                    _judge = _type;
                }
            }
            _calScore.OnNext(_judge);
        }
    }
}