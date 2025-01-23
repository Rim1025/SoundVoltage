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
        private MusicStatus _status;
        public abstract NotesType Type { get; }
        public Vector3 Position => transform.position;
        protected Material Material;//NOTE: 使うかも

        public abstract void OnActivate(LaneName laneName,float speed);

        protected abstract void OnPush();
        protected abstract void OnDeActivate();
        public bool Active
        {
            get => gameObject.activeSelf;
            protected set => gameObject.SetActive(value);
        }
        public LaneName MyLane { get; private set; }

        /// <summary>
        /// ノーツ初期化時の処理
        /// </summary>
        /// <param name="laneName">生成するレーン</param>
        /// <param name="status">MusicStatus</param>
        public void Activate(LaneName laneName,MusicStatus status)
        {
            _status = status;
            OnActivate(laneName,_status.NotesSpeed);
            Active = true;
            MyLane = laneName;
            this.transform.position = GameData.LanePositions[laneName];
            _updateDisposable = GameEvents.UpdateGame.Subscribe(t =>
            {
                transform.position += Vector3.back * _status.NotesSpeed * t;
            });
        }

        /// <summary>
        /// ノーツを終了した際の処理
        /// </summary>
        protected void DeActivate()
        {
            Active = false;
            _updateDisposable.Dispose();
        }

        /// <summary>
        /// ノーツが押された際の処理
        /// </summary>
        public void Push()
        {
            OnPush();
        }
    }
}