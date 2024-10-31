using System;
using DefaultNamespace;
using Input;
using TMPro;
using UnityEngine;
using UniRx;

namespace Start
{
    public class Setting: MonoBehaviour
    {
        [SerializeField] private GameObject _select;
        [SerializeField] private float _adjust;
        [SerializeField] private float _speed;
        [SerializeField] private TextMeshProUGUI _adjustText;
        [SerializeField] private TextMeshProUGUI _speedText;
        
        private void Start()
        {
            StatusHolder _statusHolder = new StatusHolder();
            
            View();
            KeyManager.KeyDownSubject.Subscribe(x =>
            {
                if (!gameObject.activeInHierarchy)
                {
                    return;
                }
                switch (x)
                {
                    case 0:
                        StatusHolder.Status.AdjustTouch -= _adjust;
                        break;
                    case 1:
                        StatusHolder.Status.AdjustTouch += _adjust; 
                        break;
                    case 2:
                        StatusHolder.Status.MoveSpeed -= _speed;
                        break;
                    case 3:
                        StatusHolder.Status.MoveSpeed += _speed;
                        break;
                    case 4:
                        Debug.Log(gameObject.name);
                        gameObject.SetActive(false);
                        _select.SetActive(true);
                        break;
                    case 5:
                        StatusHolder.Status.SaveStatus();
                        LoadScene.SceneLoad("InGame");
                        break;
                }
                View();
            }).AddTo(this);
        }

        public void View()
        {
            Debug.Log(StatusHolder.Status);
            _adjustText.text = "判定調整\n" + StatusHolder.Status.AdjustTouch.ToString();
            _speedText.text = "移動速度\n" + StatusHolder.Status.MoveSpeed.ToString();
        }

        private void ReturnSelect()
        {
            this.gameObject.SetActive(false);
            _select.SetActive(true);
        }
    }
}