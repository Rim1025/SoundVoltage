using System;
using UniRx;

namespace Model
{
    public class GameEvents
    {
        private static Subject<Unit> _start = new Subject<Unit>();
        public static IObservable<Unit> StartGame => _start;

        private static Subject<float> _update = new Subject<float>();
        public static IObservable<float> UpdateGame => _update;

        public void GameStart()
        {
            _start.OnNext(Unit.Default);
        }

        public void GameUpdate(float time)
        {
            _update.OnNext(time);
        }
    }
}