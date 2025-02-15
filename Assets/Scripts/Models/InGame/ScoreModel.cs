using UniRx;
using System;
using Defaults;
using Interfaces;
using Zenject;

namespace Model
{
    public class ScoreModel: IScoreModel
    {
        public ReactiveProperty<float> Score { get; } = new();
        public ReactiveProperty<int> Combo { get; } = new();
        private Subject<JudgeType> _judge { get; } = new();
        public IObservable<JudgeType> JudgeResult => _judge;

        private float _score;
        private float _combo;
        [Inject]
        public ScoreModel(IJudgeNotes judgeNotes)
        {
            judgeNotes.CalScore.Subscribe(type =>
            {
                Score.Value = _score += GameData.JudgeScore[(int)type];

                // Missなら0にそれ以外なら1加算
                Combo.Value = (int)(_combo += type == JudgeType.Miss ? -_combo : 1);
            
                _judge.OnNext(type);
            });
        }
    }
}