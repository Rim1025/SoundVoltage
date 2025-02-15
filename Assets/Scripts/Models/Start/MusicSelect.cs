using Services;
using Defaults;
using Interfaces;
using UniRx;
using UnityEngine;
using View;

namespace Model
{
    public class MusicSelect
    {
        private ListMover<string> _musicNameList;
        public ReactiveProperty<string> MusicName = new ();
        private IInputProvider _input;
        public MusicSelect(IInputProvider input,Canvases canvasChanger)
        {
            _input = input;
            _musicNameList = new (FolderNameGetter.PathUnderAll(GameData.DataPath));
            MusicName.Value = _musicNameList.Selecting();
            float _time = 0;
            _input.Push
                .Where(lane => canvasChanger.Canvas[(int)CanvasType.Select].activeSelf &&
                               _time <= 0 && lane != LaneName.BigLeft && lane != LaneName.BigRight)
                .Subscribe(lane =>
                {
                    _musicNameList.Move((lane is LaneName.OuterRight or LaneName.InnerRight) ? -1 : 1);
                    MusicName.Value = _musicNameList.Selecting();
                    _time = GameData.ButtonMoveDelay;
                });
            _input.Push
                .First()
                .Subscribe(_ =>
                {
                    GameEvents.UpdateGame
                        .Where(_ => canvasChanger.Canvas[(int)CanvasType.Select].activeSelf)
                        .Subscribe(t =>
                        {
                            _time -= t;
                        });
                });
        }
    }
}