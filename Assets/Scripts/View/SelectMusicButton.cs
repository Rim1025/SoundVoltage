using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model
{
    /// <summary>
    /// 曲選択UI所持
    /// </summary>
    public class SelectMusicButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _musicNameText;
        [SerializeField] private RectTransform _transform;

        public void SetMusicName(string text)
        {
            _musicNameText.text = text;
        }

        public RectTransform GetTransform()
        {
            return _transform;
        }
    }
}