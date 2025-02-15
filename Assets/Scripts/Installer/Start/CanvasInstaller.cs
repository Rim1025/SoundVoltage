using Model;
using UnityEngine;
using View;
using Zenject;
using Interfaces;

namespace Installer
{
    public class CanvasInstaller: MonoInstaller
    {
        [SerializeField] private Canvases _canvas;

        public override void InstallBindings()
        {
            Container.Bind<Canvases>()
                .FromInstance(_canvas);
            Container.Bind<CanvasChanger>()
                .AsSingle()
                .NonLazy();
        }
    }
}