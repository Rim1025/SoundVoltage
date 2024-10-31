using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using Input;

namespace Serial
{
    public class SerialManager: MonoBehaviour
    {
        [SerializeField] private string _rightPortName;
        [SerializeField] private int _rightPortBand = 9600;
        [SerializeField] private string _leftPortName;
        [SerializeField] private int _leftPortBand = 9600;
        [SerializeField] private SerialUpdate _update;
        private GetSerial _rightSerial;
        private GetSerial _leftSerial;

        private List<int> _oldRightResult = new List<int>();
        private List<int> _oldLeftResult = new List<int>();

        private void Start()
        {
            _leftSerial = new GetSerial(_leftPortName, _leftPortBand, this,_update);
            _rightSerial = new GetSerial(_rightPortName, _rightPortBand, this,_update);
            
            _leftSerial.OnDataReceived += OnDataReceived;
            _rightSerial.OnDataReceived += OnDataReceived;
            _update.Timer.Subscribe(x =>
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.A))
                {
                    _rightSerial.OnDestroy();
                    _leftSerial.OnDestroy();
                }
            });
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
            for (int i = 0; i < 4; i++)
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
                if (_oldResult != null)
                {
                    var _pushing = false;
                    foreach (var _oldNumber in _oldResult)
                    {
                        // 過去からずっと押している
                        if (_number == _oldNumber)
                        {
                            _pushing = true;
                            KeyManager.PushingKey((KeyType)_number);
                        }
                    }
                    // 押し続けていないけれど押されている
                    // 押された瞬間
                    if (!_pushing)
                    {
                        KeyManager.DownKey((KeyType)_number);
                    }
                }
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
                    KeyManager.UpKey((KeyType)_old);
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