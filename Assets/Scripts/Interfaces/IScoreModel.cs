using System;
using UniRx;
using Model;

namespace Interfaces
{
    public interface IScoreModel
    {
        public ReactiveProperty<float> Score { get; }
        public ReactiveProperty<int> Combo { get; }
        public IObservable<JudgeType> JudgeResult { get; }
    }
}