using System.Collections.Generic;
using Abstracts;
using Model;
using UnityEngine;
using View;
using Zenject;

namespace Installer
{
    public class NotesFactoryInstaller: MonoInstaller
    {
        [SerializeField] private NotesPrefabs _notesPrefabs;
        public override void InstallBindings()
        {
            Container.Bind<NotesPrefabs>()
                .FromInstance(_notesPrefabs);
            Container.Bind<NotesFactory>()
                .AsSingle();
        }
    }
}