using System;
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
            SelectCanvas = new ListMover<GameObject>(canvases.GetCanvases(),false);
            
            foreach (var _canvas in canvases.GetCanvases())
            {
                _canvas.gameObject.SetActive(false);
            }
            SelectCanvas.Selecting().gameObject.SetActive(true);
            
            // Bigボタンで切り替え
            input.Push
                .Where(lane => lane is LaneName.BigRight or LaneName.BigLeft)
                .ThrottleFirst(TimeSpan.FromSeconds(GameData.ButtonMoveDelay))
                .Subscribe(lane =>
                {
                    // Listの終端で更に移動するとシーン遷移
                    if (SelectCanvas.SelectingNumber() == canvases.GetCanvases().Count - 1 && lane == LaneName.BigLeft)
                    {
                        statusSaver.ChangeScene();
                    }
                    SelectCanvas.Selecting().SetActive(false);
                    SelectCanvas.Move(lane == LaneName.BigRight ? -1 : 1);
                    SelectCanvas.Selecting().SetActive(true);
                });
        }
    }
}