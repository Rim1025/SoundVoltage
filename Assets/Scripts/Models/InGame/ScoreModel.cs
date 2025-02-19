using UniRx;
using System;
using Defaults;
using Interfaces;
using Zenject;

namespace Model
{
    /// <summary>
    /// スコアの管理
    /// </summary>
    public class ScoreModel: IScoreModel
    {
        /// <summary>
        /// スコア
        /// </summary>
        public ReactiveProperty<float> Score { get; } = new();
        
        /// <summary>
        /// コンボ数
        /// </summary>
        public ReactiveProperty<int> Combo { get; } = new();

        private Subject<JudgeType> _judge { get; } = new();
                
        /// <summary>
        /// ジャッチ結果
        /// </summary>
        public IObservable<JudgeType> JudgeResult => _judge;

        // スコア本体
        private float _score;
        // コンボ数本体
        private float _combo;
        
        [Inject]
        public ScoreModel(IJudgeNotes judgeNotes)
        {
            judgeNotes.CalJudge.Subscribe(type =>
            {
                Score.Value = _score += GameData.JudgeScore[type];

                // Missなら0にそれ以外なら1加算
                Combo.Value = (int)(_combo += type == JudgeType.Miss ? -_combo : 1);
            
                _judge.OnNext(type);
            });
        }
    }
}