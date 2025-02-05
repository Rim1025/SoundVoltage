using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace View
{
    public class ResultViewer: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private GameObject _endPanel;
        [SerializeField] private GameObject[] _playingActiveObject;

        private void Start()
        {
            _endPanel.SetActive(false);
        }

        public void View(float score, int combo)
        {
            _endPanel.SetActive(true);
            foreach (var _obj in _playingActiveObject)
            {
                _obj.SetActive(false);
            }
            _scoreText.text = "Score:" + score;
            _comboText.text = "最大コンボ:" + combo;
        }
    }
}