using Abstracts;
using Defaults;
using Model;
using UnityEngine;
using Zenject;

namespace View.Notes
{
    public class Normal: NotesCore
    {
        public override NotesType Type => NotesType.Normal;

        [SerializeField] private Material _normalMaterial;
        [SerializeField] private Material _bigMaterial;
        
        /// <summary>
        /// 押された際の処理
        /// </summary>
        protected override void OnPush()
        {
            DeActivate();
        }

        /// <summary>
        /// 生成又は再利用時の処理
        /// </summary>
        /// <param name="laneName">生成されるレーン</param>
        /// <param name="speed">ノーツの速度</param>
        protected override void OnActivate(LaneName laneName,float speed)
        {
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