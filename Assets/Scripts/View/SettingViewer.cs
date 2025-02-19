using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace View
{
    /// <summary>
    /// 曲設定UI所持
    /// </summary>
    public class SettingViewer: MonoBehaviour
    {
        /// <summary>
        /// 設定タイプ
        /// </summary>
        public TextMeshProUGUI TypeText;
        /// <summary>
        /// タイプの値
        /// </summary>
        public TextMeshProUGUI ValueText;
        /// <summary>
        /// 光量調節
        /// </summary>
        public PostProcessVolume Volume;
    }
}