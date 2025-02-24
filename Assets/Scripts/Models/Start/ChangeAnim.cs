using System;
using UnityEngine;
using UniRx;

namespace Model
{
    public class ChangeAnim
    {
        private bool _isChanging;
        private float _time;
        private RectTransform _transform;
        private Vector3 _defaultScale;
        private Vector3 _direction;
        private float _rotationTime;
        private float _rad;

        public ChangeAnim()
        {
            _isChanging = false;
            GameEvents.UpdateGame
                .Where(_=>_isChanging)
                .Select(t =>
                {
                    _time += t;
                    return _time;
                })
                .Subscribe(t =>
                {
                    _transform.localScale = _defaultScale - _direction * (t * 2 / _rotationTime) * _rad / 360;
                    if (_time > _rotationTime)
                    {
                        _transform.localScale = _defaultScale;
                        _isChanging = false;
                        _time = 0;
                    }
                });
        }

        /// <summary>
        /// UIを任意の方向に回転
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="direction"></param>
        /// <param name="rotationTime"></param>
        /// <param name="rad">角度(度)</param>
        public void Change(RectTransform transform,Vector3 direction,float rotationTime,float rad)
        {
            _transform = transform;
            _direction = direction;
            _rotationTime = rotationTime;
            _rad = rad;
            _defaultScale = transform.localScale;
            _isChanging = true;
        }
    }
}