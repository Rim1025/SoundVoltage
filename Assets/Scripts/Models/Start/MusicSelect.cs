using Services;
using Defaults;
using Interfaces;
using UniRx;
using View;

namespace Model
{
    /// <summary>
    /// 曲選択
    /// </summary>
    public class MusicSelect
    {
        private ListMover<string> _musicNameList;
        private IInputProvider _input;
        
        /// <summary>
        /// 選択中の曲名
        /// </summary>
        public ReactiveProperty<string> MusicName = new ();

        public MusicSelect(IInputProvider input,Canvases canvasChanger)
        {
            _input = input;
            _musicNameList = new (FolderNameGetter.PathUnderAll(GameData.DataPath));
            MusicName.Value = _musicNameList.Selecting();
            // 切り替え時間制限用
            float _time = 0;
            // ボタンで移動
            _input.Push
                .Where(lane => canvasChanger.GetCanvases()[(int)CanvasType.Select].activeSelf &&
                               _time <= 0 && lane != LaneName.BigLeft && lane != LaneName.BigRight)
                .Subscribe(lane =>
                {
                    _musicNameList.Move((lane is LaneName.OuterRight or LaneName.InnerRight) ? -1 : 1);
                    MusicName.Value = _musicNameList.Selecting();
                    _time = GameData.ButtonMoveDelay;
                });
            // 切り替え時間制限
            _input.Push
                .First()
                .Subscribe(_ =>
                {
                    GameEvents.UpdateGame
                        .Where(_ => canvasChanger.GetCanvases()[(int)CanvasType.Select].activeSelf)
                        .Subscribe(t =>
                        {
                            _time -= t;
                        });
                });
        }
    }
}