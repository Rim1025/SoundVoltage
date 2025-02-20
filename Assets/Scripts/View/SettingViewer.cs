using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace View
{
    /// <summary>
    /// 曲設定UI所持
    /// </summary>
    public class SettingViewer: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private PostProcessVolume _volume;

        /// <summary>
        /// 選択しているステータスのタイプをセット
        /// </summary>
        /// <param name="text">タイプ名</param>
        public void SetStatusType(string text)
        {
            _typeText.text = text;
        }

        /// <summary>
        /// 選択しているステータスの値をセット
        /// </summary>
        /// <param name="text">値</param>
        public void SetStatusValue(string text)
        {
            _valueText.text = text;
        }
        
        /// <summary>
        /// レーンの光量セット
        /// </summary>
        /// <param name="intensity">光量</param>
        public void SetBloom(float intensity)
        {
            if(_volume.profile.TryGetSettings(out Bloom _bloom))
                _bloom.intensity.value = intensity;
        }
    }
}