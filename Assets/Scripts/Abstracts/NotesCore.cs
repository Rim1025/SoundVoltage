using System;
using Defaults;
using Interfaces;
using Model;
using UnityEngine;
using UniRx;
using Services;

namespace Abstracts
{
    /// <summary>
    /// ノーツの基本動作
    /// </summary>
    public abstract class NotesCore: MonoBehaviour, INotes
    {
        /// <summary>
        /// ノーツのタイプ
        /// </summary>
        public abstract NotesType Type { get; }
        
        /// <summary>
        /// 計算上のノーツ座標
        /// </summary>
        public Vector3 Position { get; protected set; }
        
        /// <summary>
        /// 自身の存在するレーン
        /// </summary>
        public LaneName MyLane { get; private set; }
        
        /// <summary>
        /// 今アクティブかどうか
        /// </summary>
        public bool Active
        {
            get => gameObject.activeSelf;
            private set => gameObject.SetActive(value);
        }
        
        // ノーツ独自の生成時処理
        protected abstract void OnActivate(LaneName laneName,float speed);
        
        // ノーツ独自の押された際の処理
        protected abstract void OnPush();

        // 移動処理を代入
        private IDisposable _updateDisposable;
        // 移動速度等のパラメータ
        private MusicStatus _status;
        // ノーツの色
        protected Material Material;

        /// <summary>
        /// ノーツ初期化時の処理
        /// </summary>
        /// <param name="laneName">生成するレーン</param>
        /// <param name="status">MusicStatus</param>
        public void Activate(LaneName laneName,MusicStatus status)
        {
            // 取得
            _status = status;
            MyLane = laneName;

            // ノーツ初期化
            OnActivate(MyLane,_status.NotesSpeed);
            transform.position = GameData.LanePositions[laneName];
            Position = transform.position;
            
            _updateDisposable = GameEvents.UpdateGame.Subscribe(t =>
            {
                // 移動はオブジェクトと座標の2つを移動させる
                transform.position += Vector3.back * _status.NotesSpeed * t;
                Position += Vector3.back * _status.NotesSpeed * t;
            });
            
            // アクティブ化
            Active = true;
        }

        /// <summary>
        /// ノーツが消された際の処理
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