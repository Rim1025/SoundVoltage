using System;
using UniRx;
using UnityEngine;

namespace Serial
{
    public class SerialUpdate: MonoBehaviour
    {
        private Subject<Unit> _subject = new Subject<Unit>();

        public IObservable<Unit> Timer => _subject;

        private void Update()
        {
            _subject.OnNext(Unit.Default);
        }
    }
}