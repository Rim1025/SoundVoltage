using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace InGame.Score
{
    public class ViewJudge: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private NotesUpdate _update;
        [SerializeField] private float _viewTime;

        private float _time = 0;

        private void Start()
        {
            _update.Timer.Subscribe(x =>
            {
                _time += (float)1/_update.FPS;
                if (_time > _viewTime && _text.text != "")
                {
                    _text.text = "";
                }
            });
            ScoreManager.Reset();
            ScoreManager.JudgeSubject.Subscribe(text =>
            {
                _time = 0;
                _text.text = text;
            });
        }
    }
}