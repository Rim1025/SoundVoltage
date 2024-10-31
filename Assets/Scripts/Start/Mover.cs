using System;
using InGame.Score;
using Input;
using UnityEngine;
using UniRx;
using Status;

namespace Start
{
    public class Mover: MonoBehaviour
    {
        [SerializeField] private MusicNameGetter _musicNameGetter;
        [SerializeField] private GameObject _settingPanel;
        
        private float _distance;
        private int _lookNumber = 0;
        private StatusHolder _status;

        private void Start()
        {
            _settingPanel.SetActive(false);
            StatusHolder _statusHolder = new StatusHolder();
            StatusHolder.Status.Bpm = 200;
            _distance = _musicNameGetter.MusicButtons[1].transform.localPosition.x;
            KeyManager.KeyDownSubject.Subscribe(x =>
            {
                if (!gameObject.activeInHierarchy)
                {
                    return;
                }
                var _direction = 0;
                if (x<=1)
                {
                    _direction = 1;
                }
                else if (x<=3)
                {
                    _direction = -1;
                }
                Move(_direction);

                if (x==5)
                {
                    StatusHolder.Status.CsvName = _musicNameGetter.MusicNames[_lookNumber];
                    StatusHolder.Status.SaveStatus();
                    Debug.Log(StatusHolder.Status.CsvName);
                    _settingPanel.SetActive(true);
                    gameObject.SetActive(false);
                }
            }).AddTo(this);
        }

        public void Move(int direction)
        {
            _musicNameGetter.transform.localPosition += new Vector3(direction * _distance, 0, 0);
            _lookNumber -= direction;
            if (_lookNumber<0)
            {
                Move(-_musicNameGetter.MusicButtons.Count);
            }

            if (_lookNumber >= _musicNameGetter.MusicButtons.Count)
            {
                Move(_musicNameGetter.MusicButtons.Count);
            }
        }
    }
}