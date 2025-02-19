using Defaults;
using Model;
using Services;
using UnityEngine;
using View;
using Zenject;
using UniRx;
using UnityEngine.Rendering.PostProcessing;

namespace Presenters
{
    /// <summary>
    /// ステータス変更管理
    /// </summary>
    public class SettingPresenter: MonoBehaviour
    {
        [SerializeField] private SettingViewer _viewer;
        
        [Inject]
        public void Construct(MusicSetting setting)
        {
            // 選択中にステータスタイプ
            setting.Type.Subscribe(t =>
            {
                _viewer.TypeText.text = t.ToString();
            });
            // ステータスの値
            setting.SelectValue.Subscribe(v =>
            {
                if (setting.Type.Value == StatusType.Voltage && _viewer.Volume.profile.TryGetSettings<Bloom>(out var _bloom))
                {
                    _bloom.intensity.value = GameData.JudgeBloom[JudgeType.Miss] * v;
                }
                //NOTE: 小数点をカット
                _viewer.ValueText.text = v.ToString("F1");
            });
        }
    }
}