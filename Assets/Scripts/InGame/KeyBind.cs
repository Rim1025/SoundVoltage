using System;
using System.Collections.Generic;
using Input;
using UnityEngine;
using UniRx;

namespace InGame
{
    public class KeyBind: MonoBehaviour
    {
        [SerializeField] private LaneManager _laneManager;

        private List<Lane> _lanes;

        private void Start()
        {
            _lanes = _laneManager.GetLane();

            KeyManager.KeyDownSubject.Subscribe(x =>
            {
                _lanes[x].Push();
            }).AddTo(this);

            KeyManager.KeyUpSubject.Subscribe(x =>
            {
                _lanes[x].Up();
            }).AddTo(this);

            KeyManager.KeySubject.Subscribe(x =>
            {
                _lanes[x].Down();
            }).AddTo(this);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.W))
            {
                _lanes[0].Push();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                _lanes[1].Push();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                _lanes[2].Push();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                _lanes[3].Push();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                _lanes[4].Push();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.M))
            {
                _lanes[5].Push();
            }
        }
    }
}