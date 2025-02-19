using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Model;
using UniRx;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// キー入力を取得
    /// </summary>
    public class KeyInput: IInputProvider
    {
        // 取得するキーコード
        private List<KeyCode> _keyCodes;
        
        private Subject<LaneName> _push = new();
        private Subject<LaneName> _exitPush = new();

        /// <summary>
        /// 押されているレーン
        /// </summary>
        public IObservable<LaneName> Push => _push;
        
        /// <summary>
        /// 離れた瞬間のレーン
        /// </summary>
        public IObservable<LaneName> ExitPush => _exitPush;
        
        // 押されているかのフラグ群
        private Dictionary<LaneName, bool> _pushing = new()
        {
            { LaneName.OuterRight, false },
            { LaneName.BigRight, false },
            { LaneName.InnerRight, false },
            { LaneName.InnerLeft, false },
            { LaneName.BigLeft, false },
            { LaneName.OuterLeft, false }
        };
        
        public KeyInput(List<KeyCode> keyCodes)
        {
            _keyCodes = keyCodes;
            
            GameEvents.UpdateGame.Subscribe(_ =>
            {
                foreach (var _key in _keyCodes
                             .Select((value, index) => new { value, index }))
                {
                    var _lane = (LaneName)Enum.ToObject(typeof(LaneName), _key.index);
                    // 押されているとき
                    if (Input.GetKey(_key.value))
                    {
                        _pushing[_lane] = true;
                        _push.OnNext(_lane);
                    }
                    // 押されていたが押されていないとき
                    else if (_pushing[_lane])
                    {
                        _pushing[_lane] = false;
                        _exitPush.OnNext((LaneName)Enum.ToObject(typeof(LaneName), _key.index));
                    }
                }
            });
        }
    }
}