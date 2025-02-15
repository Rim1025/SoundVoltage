using System;
using System.Collections.Generic;
using Interfaces;
using Model;
using TMPro;
using UnityEngine;
using UniRx;

namespace Serial
{
    public class SerialManager: MonoBehaviour, IInputProvider
    {
        [SerializeField] private string _rightPortName;
        [SerializeField] private int _rightPortBand = 9600;
        [SerializeField] private string _leftPortName;
        [SerializeField] private int _leftPortBand = 9600;
        private GetSerial _rightSerial;
        private GetSerial _leftSerial;

        private Subject<LaneName> _push = new();
        private Subject<LaneName> _exitPush = new();

        public IObservable<LaneName> Push => _push;
        public IObservable<LaneName> ExitPush => _exitPush;


        private List<int> _oldRightResult = new List<int>();
        private List<int> _oldLeftResult = new List<int>();

        private void Start()
        {
            _leftSerial = new GetSerial(_leftPortName, _leftPortBand,this);
            _rightSerial = new GetSerial(_rightPortName, _rightPortBand,this);
            
            _leftSerial.OnDataReceived += OnDataReceived;
            _rightSerial.OnDataReceived += OnDataReceived;
        }

        private void OnDestroy()
        {
            _leftSerial.OnDestroy();
            _rightSerial.OnDestroy();
        }

        void OnDataReceived(string message)
        {
            var _text = int.TryParse(message, out var _result);
            if (!_text)
            {
                return;
            }
            bool _isLeft = false;
            var _resultNumber = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                if (1 == (_result >> i & 0b1))
                {
                    switch (i)
                    {
                        case 0:
                            _isLeft = true;
                            break;
                        case 1:
                            if (_isLeft)
                            {
                                _resultNumber.Add(0);
                            }
                            else
                            {
                                _resultNumber.Add(2);
                            }
                            break;
                        case 2:
                            if (_isLeft)
                            {
                                _resultNumber.Add(1);
                            }
                            else
                            {
                                _resultNumber.Add(3);
                            }
                            break;
                        case 3:
                            if (_isLeft)
                            {
                                _resultNumber.Add(4);
                            }
                            else
                            {
                                _resultNumber.Add(5);
                            }
                            break;
                        case 4:
                        {
                            break;
                        }
                        case 5:
                        {
                            break;
                        }
                        case 6:
                        {
                            break;
                        }
                        case 7:
                        {
                            break;
                        }
                            
                    }
                }
            }

            List<int> _oldResult;
            if (_isLeft)
            {
                _oldResult = _oldLeftResult;
            }
            else
            {
                _oldResult = _oldRightResult;
            }
            foreach (var _number in _resultNumber)
            {
                _push.OnNext((LaneName)Enum.ToObject(typeof(LaneName),_number));
            }

            foreach (var _old in _oldResult)
            {
                var _pushing = false;
                foreach (var _res in _resultNumber)
                {
                    if (_old == _res)
                    {
                        _pushing = true;
                    }
                }

                if (!_pushing)
                {
                    _exitPush.OnNext((LaneName)Enum.ToObject(typeof(LaneName),_old));
                }
            }
            
            if (_isLeft)
            {
                _oldLeftResult = _resultNumber;
            }
            else
            {
                _oldRightResult = _resultNumber;
            }
        }
    }
}