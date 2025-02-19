using System.Collections.Generic;
using Abstracts;
using Defaults;
using Model;
using UnityEngine;
using Interfaces;

namespace View.Notes
{
    /// <summary>
    /// Longノーツ
    /// </summary>
    public class Long: NotesCore,ILongNotes
    {
        public override NotesType Type => NotesType.Long;

        /// <summary>
        /// 押されたかどうか(長押し中かどうか)
        /// </summary>
        public bool IsPushed { get; private set; } = false;
        
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _bigMaterial;

        private List<float> _growLength = new();
        
        /// <summary>
        /// 押された又は押し続けられている際の処理
        /// </summary>
        protected override void OnPush()
        {
            // 最初に押されたかどうか
            if (!IsPushed)
            {
                IsPushed = true;
            }
            // 長押し中の処理
            if (_growLength.Count > 1)
            {
                var _old = _growLength[^1];
                _growLength.Remove(_growLength[^1]);
                var _length = _growLength[^1];
                var _tr = transform;
                transform.localScale = new Vector3(_tr.localScale.x, _tr.localScale.y, _length*1 / 10 + GameData.NotesScale[MyLane].z);
                Position = new Vector3(Position.x, Position.y, Position.z + _old - _length);
                transform.position = new Vector3(Position.x, Position.y, Position.z + _length / 2);
            }
            // 長押し終端に達した際の処理
            else
            {
                DeActivate();
            }
        }

        /// <summary>
        /// 生成中伸び続ける
        /// </summary>
        public void Grow()
        {
            var _length = GameData.LanePositions[MyLane].z - Position.z;
            var _tr = transform;
            transform.localScale = new Vector3(_tr.localScale.x, _tr.localScale.y, _length*1 / 10 + GameData.NotesScale[MyLane].z);
            transform.position = new Vector3(Position.x, Position.y, Position.z + _length / 2);
            _growLength.Add(_length);
        }

        /// <summary>
        /// 強制的に終了
        /// </summary>
        public void Miss()
        {
            DeActivate();
        }

        /// <summary>
        /// 生成時又は再利用時の処理
        /// </summary>
        /// <param name="laneName">生成されるレーン</param>
        /// <param name="speed">ノーツの速度</param>
        protected override void OnActivate(LaneName laneName,float speed)
        {
            // 伸びた際の自身の長さを記録
            _growLength = new();
            _growLength.Add(0);
            IsPushed = false;
            transform.localScale = GameData.NotesScale[MyLane];
            if (MyLane is LaneName.BigRight or LaneName.BigLeft)
            {
                Material = GetComponent<Renderer>().material = _bigMaterial;
                
            }
            else
            {
                Material = GetComponent<Renderer>().material = _normalMaterial;
            }
        }
    }
}