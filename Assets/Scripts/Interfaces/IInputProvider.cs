using System;
using Model;
using UniRx;

namespace Interfaces
{
    public interface IInputProvider
    {
        IObservable<LaneName> Push { get; }
        IObservable<LaneName> ExitPush { get; }
    }
}