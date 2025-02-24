using Defaults;
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
    public class MusicSettingPresenter: MonoBehaviour
    {
        [SerializeField] private SettingViewer _viewer;
        
        [Inject]
        public void Construct(MusicSetting setting)
        {
            var _typeAnim = new ChangeAnim();
            var _vaLAnim = new ChangeAnim();
            // 選択中にステータスタイプ
            setting.Type.Subscribe(t =>
            {
                _typeAnim.Change(_viewer.GetTypeTransform(), Vector3.up, GameData.ButtonMoveDelay, 360);
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
                _vaLAnim.Change(_viewer.GetValueTransform(), Vector3.up, GameData.ButtonMoveDelay, 360);
                //NOTE: 小数点をカット
                _viewer.SetStatusValue(v.ToString("F1"));
            });
        }
    }
}