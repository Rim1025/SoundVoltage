using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Err
{
    /// <summary>
    /// エラー用View
    /// </summary>
    public class ErrViewer: MonoBehaviour
    {
        [SerializeField] private Text _text;

        private void Start()
        {
            _text.text = "";
            Err.ErrEvents.Subscribe(text =>
            {
                View(text);
            });
        }

        private void View(string text)
        {
            _text.text = text;
        }
    }
}
