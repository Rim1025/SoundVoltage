using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    /// <summary>
    /// Text直下に配置
    /// </summary>
    public class ErrDisplay: MonoBehaviour
    {
        private static Text _errText;

        private void Awake()
        {
            _errText = GetComponentInChildren<Text>();
        }

        public static void ViewErr(string text)
        {
            //Debug.LogError(text);
            _errText.text = text;
        }
    }
}