using Model;
using UnityEngine;
using UniRx;
using Zenject;

namespace Presenters
{
    /// <summary>
    /// 曲選択テキスト管理
    /// </summary>
    public class SelectButtonPresenter: MonoBehaviour
    {
        [SerializeField] private SelectMusicButton _button;

        [Inject]
        public void Construct(MusicSelect select)
        {
            select.MusicName.Subscribe(t =>
            {
                _button.SetMusicName(t);
            });
        }
    }
}