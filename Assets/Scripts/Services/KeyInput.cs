using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services
{
    public class KeyInput:IInputProvider
    {
        private List<KeyCode> _keyCodes;
        private Subject<LaneName> _push = new();
        private Subject<LaneName> _exitPush = new();

        public IObservable<LaneName> Push => _push;
        public IObservable<LaneName> ExitPush => _exitPush;
        
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
                    if (Input.GetKey(_key.value))
                    {
                        _pushing[_lane] = true;
                        _push.OnNext(_lane);
                    }
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