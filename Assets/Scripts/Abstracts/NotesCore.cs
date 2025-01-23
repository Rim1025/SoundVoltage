using System;
using Defaults;
using Interfaces;
using Model;
using UnityEngine;
using UniRx;
using Zenject;

namespace Abstracts
{
    public abstract class NotesCore: MonoBehaviour, INotes
    {
        private IDisposable _updateDisposable;
        public abstract NotesType Type { get; }
        public Vector3 Position { get=> transform.position;}
        protected Material Material;//NOTE: 使うかも

        public abstract void OnActivate(LaneName laneName,float speed);

        protected abstract void OnPush();
        public bool Active { get; private set; } = false;
        public LaneName MyLane { get; private set; }

        public void Activate(LaneName laneName,float speed)
        {
            OnActivate(laneName,speed);
            Active = true;
            MyLane = laneName;
            this.transform.position = GameData.LanePositions[laneName];
            _updateDisposable = GameEvents.UpdateGame.Subscribe(t =>
            {
                transform.position += Vector3.back * speed * t;
                if (transform.position.z < -GameData.JudgePosition[^1])
                {
                    DeActivate();
                }
            });
        }

        protected void DeActivate()
        {
            Active = false;
            _updateDisposable.Dispose();
        }

        public void Push()
        {
            OnPush();
        }
    }
}