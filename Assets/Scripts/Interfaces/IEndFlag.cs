using System;
using UniRx;

namespace Interfaces
{
    public interface IEndFlag
    {
        public IObservable<Unit> EndSubject { get; }
    }
}