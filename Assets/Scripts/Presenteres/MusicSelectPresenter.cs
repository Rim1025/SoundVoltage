using System;
using System.Threading;
using System.Threading.Tasks;
using Defaults;
using Model;
using UnityEngine;
using UniRx;
using UnityEditor;
using Zenject;

namespace Presenters
{
    /// <summary>
    /// 曲選択テキスト管理
    /// </summary>
    public class MusicSelectPresenter: MonoBehaviour
    {
        [SerializeField] private SelectMusicButton _button;

        [Inject]
        public void Construct(MusicSelect select)
        {
            var _anim = new ChangeAnim();
            select.MusicName.Subscribe(t =>
            {
                _anim.Change(_button.GetTransform(), Vector3.right, GameData.ButtonMoveDelay, 360);
                _button.SetMusicName(t);
            });
        }
    }
}