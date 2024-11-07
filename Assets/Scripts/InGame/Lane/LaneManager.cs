using System;
using System.Collections.Generic;
using DefaultNamespace;
using InGame.Audio;
using Status;
using TMPro;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;

namespace InGame
{
    public class LaneManager: MonoBehaviour
    {
        [SerializeField,Tooltip("normal*4,long*2")] private GameObject[] _laneParent;

        [SerializeField] private NotesUpdate _update;
        [SerializeField] private AudioManager _audioManager;

        private List<Lane> _lanes = new List<Lane>();
        private List<string[]> _csv;
        private AudioClip _audio;
        private StatusManager _status;
        private float _nextTime;
        private int _csvCounter = 1;
        private IDisposable _disposable;

        private void Awake()
        {
            foreach (var _obj in _laneParent)
            {
                if (!_obj.TryGetComponent<Lane>(out var _lane))
                {
                    ErrDisplay.ViewErr("Laneに必須のコンポーネントがアタッチされていません");
                }
                _lane.Init(_update);
                _lanes.Add(_obj.GetComponent<Lane>());
            }
        }

        private async void Start()
        {
            _status = new StatusManager();
            _status.LoadStatus();
            Debug.Log(_status.CsvName);
            
            _csv = CsvReader.ReadCsv(_status.CsvName);
            _audio = await AudioReader.ReadAudio(_status.CsvName);
            
            float.TryParse(_csv[1][7], out var _time);
            _audioManager.Set(_audio,_time);

            _disposable = _update.Timer.Subscribe(time =>
            {
                if (time >= _nextTime)
                {
                    var _laneCounter = 0;
                    foreach (string _value in _csv[_csvCounter])
                    {
                        // ノーツ生成
                        if (_laneCounter < 6 && int.TryParse(_value, out var _notes))
                        {
                            if (_notes == 0)
                            {
                                Debug.Log("終了");
                                // 終了
                                Invoke(nameof(End), 5);
                                _disposable.Dispose();
                            }
                            else if (_notes <= 2)
                            {
                                _lanes[_laneCounter].Generate((NotesType)_notes);
                            }
                            if (_notes == 3)
                            {
                                _lanes[_laneCounter].EndLong();
                            }
                        }

                        //その他の処理
                        if (_laneCounter >= 6)
                        {
                            switch (_laneCounter)
                            {
                                case 6 :
                                    if (int.TryParse(_value, out var _bpm))
                                    {
                                        _status.Bpm = _bpm;

                                        _status.SaveStatus();
                                    }
                                    break;
                                case 7:
                                {
                                    if (_csvCounter != 1)
                                    {
                                        break;
                                    }
                                    float.TryParse(_value, out var _time);
                                    //_audioManager.Test();
                                    var _defaultAddTime = Defaults.ConstGeneratePosition / Defaults.ConstMoveSpeed;
                                    var _timer = _update.Timer
                                        .Where(time => time>=_defaultAddTime)
                                        .First()
                                        .Subscribe(time =>
                                        {
                                            _audioManager.Play();
                                        });
                                    break;
                                }
                            }
                        }
                        _laneCounter++;
                    }
                    _csvCounter++;
                    _nextTime += 1 / ((float)_status.Bpm / 60);
                }

            }).AddTo(this);
        }

        public List<Lane> GetLane()
        {
            return _lanes;
        }

        private void End()
        {
            FlagManager.End();
        }
    }
}