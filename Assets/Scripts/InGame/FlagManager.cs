using System;
using UniRx;

namespace InGame
{
    public class FlagManager
    {
        private static Subject<Unit> _endSubject = new Subject<Unit>();

        public static IObservable<Unit> EndFlag => _endSubject;
        
        public static void End()
        {
            _endSubject.OnNext(Unit.Default);
        }

    }
}