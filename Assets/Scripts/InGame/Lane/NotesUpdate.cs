using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace InGame
{
    public class NotesUpdate: MonoBehaviour
    {
        [SerializeField] private int _fps;
        
        private Subject<float> _timer = new Subject<float>();
        private float _time;
        public float TimeCount;
        
        public IObservable<float> Timer => _timer;
        public bool IsNoteUpdate;

        public int FPS
        {
            get => _fps;
            private set => _fps = value;
        }

        private void Update()
        {
            if (!IsNoteUpdate)
                return;
            if (_time >= (float)1 / _fps)
            {
                _time = 0;
            }
            // NOTE:最初に呼ばれるためにif文を分けている
            if (_time == 0)
            {
                _timer.OnNext(TimeCount);
                TimeCount += (float)1 / _fps;
            }
            _time += Time.deltaTime;
        }
    }
}