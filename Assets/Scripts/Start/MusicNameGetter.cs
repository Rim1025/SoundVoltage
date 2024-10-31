using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Start
{
    public class MusicNameGetter : MonoBehaviour
    {
        [SerializeField] private GameObject _musicButtonPrefab;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject[] _buttons;
        [SerializeField] private float _buttonDistance;

        private FileNameGetter _musicNameGetter = new FileNameGetter();
        private string _musicFilePath = Application.dataPath + @"/Resource";
        public List<string> MusicNames { get; private set; }

        public List<GameObject> MusicButtons { get; private set; }

        private void Awake()
        {
            MusicNames = _musicNameGetter.GetFileName(_musicFilePath);
            GenerateList();
        }

        private void GenerateList()
        {
            MusicButtons = new List<GameObject>();
            var _position = new Vector2(0, 0);
            if (MusicNames == null)
                Debug.LogError("楽曲リストの取得に失敗しました");
            foreach (var _name in MusicNames)
            {
                GameObject _generateButton = Instantiate(_musicButtonPrefab, _canvas.transform.position,
                    Quaternion.identity, transform);
                RectTransform _transform = _generateButton.GetComponent<RectTransform>();
                TextMeshProUGUI _text = _generateButton.GetComponentInChildren<TextMeshProUGUI>();

                if (!_transform)
                    Debug.LogError("ボタンにRectTransformが実装されていません");
                else
                {
                    _transform.anchoredPosition += _position;
                    _position += new Vector2(_buttonDistance, 0);
                }

                _text.text = _name;

                MusicButtons.Add(_generateButton);
            }
        }

        public void OnDisable()
        {
            foreach (var _buttons in _buttons)
                _buttons.SetActive(false);
            foreach (var _button in MusicButtons)
                _button.SetActive(false);
        }

        public void OnEnable()
        {
            foreach (var _buttons in _buttons)
                _buttons.SetActive(true);
            foreach (var _button in MusicButtons)
                _button.SetActive(true);
        }
    }
}

