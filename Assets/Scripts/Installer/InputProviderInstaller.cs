using System.Collections.Generic;
using Interfaces;
using Serial;
using Services;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class InputProviderInstaller: MonoInstaller
    {
        [SerializeField] private List<KeyCode> _notesKeyCodes;
        [SerializeField] private SerialManager _serialManager;
        public override void InstallBindings()
        {
            Container.Bind<List<KeyCode>>()
                .FromInstance(_notesKeyCodes);
            Container.Bind<IInputProvider>()
                .To<KeyInput>()
                .AsSingle()
                .NonLazy();
            /*
            Container.Bind<IInputProvider>()
                .FromInstance(_serialManager)
                .AsSingle()
                .NonLazy();
            */
        }
    }
}