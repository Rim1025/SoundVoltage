using System;
using UniRx;
using UnityEngine;

namespace Input
{
    public static class KeyManager
    {
        private static Subject<int> _krySubject = new Subject<int>();
        public static IObservable<int> KeySubject => _krySubject;
        
        private static Subject<int> _kryDownSubject = new Subject<int>();
        public static IObservable<int> KeyDownSubject => _kryDownSubject;
        
        private static Subject<int> _kryUpSubject = new Subject<int>();
        public static IObservable<int> KeyUpSubject => _kryUpSubject;

        /// <summary>
        /// キーが押されているとき
        /// </summary>
        /// <param name="type">種類</param>
        public static void PushingKey(KeyType type)
        {
            Debug.Log(type);
            _krySubject.OnNext((int)type);
        }

        /// <summary>
        /// キーを押した瞬間
        /// </summary>
        /// <param name="type">種類</param>
        public static void DownKey(KeyType type)
        {
            Debug.Log(type);
            _kryDownSubject.OnNext((int)type);
        }

        /// <summary>
        /// キーが離された瞬間
        /// </summary>
        /// <param name="type">種類</param>
        public static void UpKey(KeyType type)
        {
            Debug.Log(type);
            _kryUpSubject.OnNext((int)type);
        }
    }
}