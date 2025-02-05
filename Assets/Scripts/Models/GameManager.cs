using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using View;
using Interfaces;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Services;

namespace Model
{
    public class GameManager: MonoBehaviour
    {
        private async void Start()
        {
            GameEvents _gameEvents = new GameEvents();
            await Task.Run(() => _gameEvents.GameStart());

            this.UpdateAsObservable()
                .Select(_ => Time.deltaTime)
                .Subscribe(t =>
                {
                    _gameEvents.GameUpdate(t);
                })
                .AddTo(this);
        }
    }
}