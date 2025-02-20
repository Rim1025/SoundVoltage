using Model;
using Services;
using UnityEngine;
using View;
using Zenject;
using UniRx;

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
                _viewer.SetStatusType(t.ToString());
            });
            // ステータスの値
            setting.SelectValue.Subscribe(v =>
            {
                // Voltage選択中なら後ろのレーンの光り方を変更
                if (setting.Type.Value == StatusType.Voltage)
                {
                    _viewer.SetBloom(v);
                }
                //NOTE: 小数点をカット
                _viewer.SetStatusValue(v.ToString("F1"));
            });
        }
    }
}