using Defaults;
using Interfaces;
using UniRx;
using UnityEngine;
using View;
using Zenject;

namespace Model
{
    public enum CanvasType
    {
        PCConfig,
        Select,
        Setting
    }
    
    /// <summary>
    /// キャンバス群を切り替え
    /// </summary>
    public class CanvasChanger
    {
        private ListMover<GameObject> SelectCanvas { get; }
        
        [Inject]
        public CanvasChanger(Canvases canvases,IInputProvider input,StatusSaver statusSaver)
        {
            SelectCanvas = new ListMover<GameObject>(canvases.Canvas,false);
            
            foreach (var _canvas in canvases.Canvas)
            {
                _canvas.gameObject.SetActive(false);
            }
            SelectCanvas.Selecting().gameObject.SetActive(true);

            // 切り替え時間制限用
            float _time = 0;
            // Bigボタンで切り替え
            input.Push
                .Where(lane => _time <= 0 && lane is LaneName.BigRight or LaneName.BigLeft)
                .Subscribe(lane =>
                {
                    // Listの終端で更に移動するとシーン遷移
                    if (SelectCanvas.SelectingNumber() == canvases.Canvas.Count - 1 && lane == LaneName.BigLeft)
                    {
                        statusSaver.ChangeScene();
                    }
                    SelectCanvas.Selecting().SetActive(false);
                    SelectCanvas.Move(lane == LaneName.BigRight ? -1 : 1);
                    SelectCanvas.Selecting().SetActive(true);
                    _time = GameData.ButtonMoveDelay;
                });
            input.Push
                .First()
                .Subscribe(_ =>
                {
                    GameEvents.UpdateGame.Subscribe(t =>
                    {
                        _time -= t;
                    });
                });
        }
    }
}