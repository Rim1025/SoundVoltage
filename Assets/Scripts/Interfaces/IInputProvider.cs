using System;
using Model;

namespace Interfaces
{
    public interface IInputProvider
    {
        IObservable<LaneName> Push { get; }
        IObservable<LaneName> ExitPush { get; }
    }
}