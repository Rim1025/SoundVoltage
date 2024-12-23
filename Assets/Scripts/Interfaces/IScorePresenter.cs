using System;
using UniRx;
using Model;

namespace Interfaces
{
    public interface IScorePresenter
    {
        public void SetScore(float score);
        public void SetCombo(int combo);
        public void SetJudgeResult(JudgeType type);
        public ReactiveProperty<string> Score { get; }
        public ReactiveProperty<string> Combo { get; }
        public ReactiveProperty<string> JudgeResult { get; }
    }
}