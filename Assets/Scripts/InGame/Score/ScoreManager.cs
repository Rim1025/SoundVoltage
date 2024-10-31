using System;
using InGame.Status;
using UnityEngine;
using UniRx;

namespace InGame.Score
{
    public class ScoreManager
    {
        private static float _score = 0;
        private static Subject<string> _subject = new Subject<string>();
        public static IObservable<string> JudgeSubject => _subject;

        public static void Reset()
        {
            _score = 0;
        }
        public static void Judge(JudgeType type,float rate = 1)
        {
            switch (type)
            {
                case JudgeType.Perfect:
                    Debug.Log("Perfect");
                    _subject.OnNext("Perfect");
                    _score += Defaults.ConstPerfectScore * rate;
                    break;
                case JudgeType.Good:
                    Debug.Log("Good");
                    _subject.OnNext("Good");
                    _score += Defaults.ConstGoodScore * rate;
                    break;
                case JudgeType.Miss:
                    Debug.Log("Miss");
                    _subject.OnNext("Miss");
                    _score += Defaults.ConstMissScore * rate;
                    break;
            }
        }

        public static int GetScore()
        {
            return (int)_score;
        }
    }
}