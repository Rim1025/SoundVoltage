using System;
using Unit = UniRx.Unit;

namespace Interfaces
{
    public interface INotesSpawner
    {
        public IObservable<Unit> StartAudio { get; }
    }
}