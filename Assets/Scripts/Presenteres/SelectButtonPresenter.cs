using Model;
using UnityEngine;
using UniRx;
using Zenject;

namespace Presenters
{
    public class SelectButtonPresenter: MonoBehaviour
    {
        [SerializeField] private SelectMusicButton _button;

        [Inject]
        public void Construct(MusicSelect select)
        {
            select.MusicName.Subscribe(t =>
            {
                _button.Text.text = t;
            });
        }
    }
}