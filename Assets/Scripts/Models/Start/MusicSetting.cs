using System;
using System.Collections.Generic;
using Defaults;
using Interfaces;
using Services;
using UniRx;
using View;

namespace Model
{
    /// <summary>
    /// 曲設定選択
    /// </summary>
    public class MusicSetting
    {
        /// <summary>
        /// 選択中の要素
        /// </summary>
        public ReactiveProperty<StatusType> Type = new();
        
        /// <summary>
        /// 設定要素
        /// </summary>
        public ListMover<float> StatusValues;
        
        /// <summary>
        /// 選択中の要素の値
        /// </summary>
        public ReactiveProperty<float> SelectValue = new();
        
        public MusicSetting(IInputProvider input,Canvases canvasChanger)
        {
            var _list = new List<float>();
            // リストに値を設定する要素のを追加
            foreach (int _value in Enum.GetValues(typeof(StatusType)))
            {
                _list.Add(GameData.StatusDefault[(StatusType)Enum.ToObject(typeof(StatusType), _value)]);
            }

            StatusValues = new ListMover<float>(_list);
            SelectValue.Value = StatusValues.Selecting();
            
            // 左側のキーで要素切り替え
            input.Push
                .Where(lane => canvasChanger.GetCanvases()[(int)CanvasType.Setting].activeSelf && 
                               lane is LaneName.InnerLeft or LaneName.OuterLeft)
                .ThrottleFirst(TimeSpan.FromSeconds(GameData.ButtonMoveDelay))
                .Subscribe(lane =>
                {
                    var _val = StatusValues.Selecting() +
                               GameData.StatusMass[Type.Value] * ((lane == LaneName.InnerLeft) ? -1 : 1);
                    if (_val <= GameData.StatusMax[Type.Value] && _val >= GameData.StatusMin[Type.Value])
                    {
                        StatusValues.SetValue(_val);
                        SelectValue.Value = StatusValues.Selecting();
                    }
                });
            // 右側のキーで要素の値を変更
            input.Push
                .Where(lane => canvasChanger.GetCanvases()[(int)CanvasType.Setting].activeSelf &&
                               lane is LaneName.InnerRight or LaneName.OuterRight)
                .ThrottleFirst(TimeSpan.FromSeconds(GameData.ButtonMoveDelay))
                .Subscribe(lane =>
                {
                    StatusValues.Move(lane == LaneName.InnerRight ? -1 : 1);
                    Type.Value = (StatusType)Enum.ToObject(typeof(StatusType), StatusValues.SelectingNumber());
                    SelectValue.Value = StatusValues.Selecting();
                });
        }
    }
}