using System;
using System.Collections.Generic;
using Defaults;
using Interfaces;
using Services;
using UniRx;
using UnityEngine;
using View;

namespace Model
{
    public class MusicSetting
    {
        public ReactiveProperty<StatusType> Type = new();
        public ListMover<float> StatusValues;
        public ReactiveProperty<float> SelectValue = new();
        public MusicSetting(IInputProvider input,Canvases canvasChanger)
        {
            var _list = new List<float>();
            foreach (int _value in Enum.GetValues(typeof(StatusType)))
            {
                _list.Add(GameData.StatusDefault[(StatusType)Enum.ToObject(typeof(StatusType), _value)]);
            }

            StatusValues = new ListMover<float>(_list);
            SelectValue.Value = StatusValues.Selecting();
            
            var _valTime = 0f;
            input.Push
                .Where(lane => canvasChanger.Canvas[(int)CanvasType.Setting].activeSelf &&
                               _valTime <= 0 && lane is LaneName.InnerLeft or LaneName.OuterLeft)
                .Subscribe(lane =>
                {
                    var _val = StatusValues.Selecting() +
                               GameData.StatusMass[Type.Value] * ((lane == LaneName.InnerLeft) ? -1 : 1);
                    if (_val <= GameData.StatusMax[Type.Value] && _val >= GameData.StatusMin[Type.Value])
                    {
                        StatusValues.SetValue(_val);
                        SelectValue.Value = StatusValues.Selecting();
                        _valTime = GameData.ButtonMoveDelay;
                    }
                });
            var _typeTime = 0f;
            input.Push
                .Where(lane => canvasChanger.Canvas[(int)CanvasType.Setting].activeSelf &&
                               _typeTime <= 0 && lane is LaneName.InnerRight or LaneName.OuterRight)
                .Subscribe(lane =>
                {
                    StatusValues.Move(lane == LaneName.InnerRight ? -1 : 1);
                    Type.Value = (StatusType)Enum.ToObject(typeof(StatusType), StatusValues.SelectingNumber());
                    SelectValue.Value = StatusValues.Selecting();
                    _typeTime = GameData.ButtonMoveDelay;
                });
            input.Push
                .Where(_=> canvasChanger.Canvas[(int)CanvasType.Setting].activeSelf)
                .First()
                .Subscribe(_ =>
                {
                    GameEvents.UpdateGame
                        .Where(_=>_valTime >=0 || _typeTime >= 0)
                        .Subscribe(t =>
                    {
                        _valTime -= t;
                        _typeTime -= t;
                    });
                });
        }
    }
}