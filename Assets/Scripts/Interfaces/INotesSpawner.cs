using System;
using System.Collections.Generic;
using System.Linq;
using Abstracts;
using Defaults;
using Interfaces;
using UnityEngine;
using UniRx;
using Zenject;
using Unit = UniRx.Unit;

namespace Interfaces
{
    public interface INotesSpawner
    {
        public IObservable<Unit> StartAudio { get; }
    }
}