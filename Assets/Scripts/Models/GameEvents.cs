using System;
using UniRx;
using UnityEngine;

namespace Model
{
    public class GameEvents
    {
        private static Subject<Unit> _start = new();
        public static IObservable<Unit> StartGame => _start;

        private static Subject<float> _update = new();
        public static IObservable<float> UpdateGame => _update;

        public void GameStart()
        {
            _start.OnNext(Unit.Default);
        }

        public void GameUpdate(float time)
        {
            _update.OnNext(time);
        }

        public void Complete()
        {
            _start.OnCompleted();
            _update.OnCompleted();
            _start = new Subject<Unit>();
            _update = new Subject<float>();
        }
    }
}