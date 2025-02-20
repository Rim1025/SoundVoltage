using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace View
{
    /// <summary>
    /// リザルト表示UI所持
    /// </summary>
    public class ResultViewer: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private GameObject _endPanel;
        [SerializeField]private GameObject[] _notActiveInResultObject;

        private void Start()
        {
            _endPanel.SetActive(false);
        }

        /// <summary>
        /// リザルト表示
        /// </summary>
        /// <param name="score">スコア</param>
        /// <param name="combo">最大コンボ</param>
        public void View(float score, int combo)
        {
            _endPanel.SetActive(true);
            // リザルト時非アクティブオブジェクトを非アクティブに
            foreach (var _obj in _notActiveInResultObject)
            {
                _obj.SetActive(false);
            }
            _scoreText.text = "Score:" + score;
            _comboText.text = "最大コンボ:" + combo;
        }
    }
}