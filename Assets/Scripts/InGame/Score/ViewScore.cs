using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

namespace InGame.Score
{
    public class ViewScore: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _panel;


        private void Start()
        { 
            _panel.SetActive(false);
            FlagManager.EndFlag.Subscribe(x =>
            {
                _panel.SetActive(true);
                View();
            }).AddTo(this);
        }

        private void View()
        {
            _text.text = "Score\n"+ ScoreManager.GetScore();
        }
    }
}