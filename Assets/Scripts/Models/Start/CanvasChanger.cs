using Defaults;
using Interfaces;
using Services;
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
    public class CanvasChanger
    {
        public ListMover<GameObject> SelectCanvas { get; private set; }

        private Canvases _canvases;
        private IInputProvider _input;

        [Inject]
        public CanvasChanger(Canvases canvases,IInputProvider input,StatusSaver statusSaver)
        {
            _input = input;
            _canvases = canvases;
            SelectCanvas = new ListMover<GameObject>(_canvases.Canvas,false);
            
            foreach (var _canvas in _canvases.Canvas)
            {
                _canvas.gameObject.SetActive(false);
            }
            SelectCanvas.Selecting().gameObject.SetActive(true);

            float _time = 0;
            _input.Push
                .Where(lane => _time <= 0 && lane is LaneName.BigRight or LaneName.BigLeft)
                .Subscribe(lane =>
                {
                    if (SelectCanvas.SelectingNumber() == _canvases.Canvas.Count - 1 && lane == LaneName.BigLeft)
                    {
                        statusSaver.ChangeScene();
                    }
                    SelectCanvas.Selecting().SetActive(false);
                    SelectCanvas.Move(lane == LaneName.BigRight ? -1 : 1);
                    SelectCanvas.Selecting().SetActive(true);
                    _time = GameData.ButtonMoveDelay;
                });
            _input.Push
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