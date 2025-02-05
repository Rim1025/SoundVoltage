using System.Collections.Generic;
using Interfaces;
using Services;
using UnityEngine;
using Zenject;

namespace Installer
{
    public class InputProviderInstaller: MonoInstaller
    {
        [SerializeField] private List<KeyCode> _notesKeyCodes;
        public override void InstallBindings()
        {
            Container.Bind<List<KeyCode>>()
                .FromInstance(_notesKeyCodes);
            Container.Bind<IInputProvider>()
                .To<KeyInput>()
                .AsSingle()
                .NonLazy();
        }
    }
}