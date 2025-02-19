using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace View
{
    /// <summary>
    /// スコア表示UI所持
    /// </summary>
    public class ScoreViewer: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private TextMeshProUGUI _judgeResultText;
        [SerializeField] private PostProcessVolume _volume;

        /// <summary>
        /// スコア表示
        /// </summary>
        /// <param name="text">スコア</param>
        public void SetScore(string text)
        {
            _scoreText.text = text;
        }
        
        /// <summary>
        /// コンボ表示
        /// </summary>
        /// <param name="text">コンボ数</param>
        public void SetCombo(string text)
        {
            _comboText.text = text;
        }

        /// <summary>
        /// ジャッチ表示
        /// </summary>
        /// <param name="text">結果</param>
        /// <param name="color">文字色</param>
        public void SetJudge(string text,Color color)
        {
            _judgeResultText.text = text;
            _judgeResultText.color = color;
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