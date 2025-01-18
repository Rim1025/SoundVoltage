using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace View
{
    public class ScoreViewer: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private TextMeshProUGUI _judgeResultText;
        [SerializeField] private PostProcessVolume _volume;

        public void SetScore(string t)
        {
            _scoreText.text = t;
        }
        
        public void SetCombo(string t)
        {
            _comboText.text = t;
        }

        public void SetJudge(string t,Color color)
        {
            _judgeResultText.text = t;
            _judgeResultText.color = color;
        }

        public void SetBloom(float intensity)
        {
            if(_volume.profile.TryGetSettings(out Bloom _bloom))
                _bloom.intensity.value = intensity;
        }
    }
}