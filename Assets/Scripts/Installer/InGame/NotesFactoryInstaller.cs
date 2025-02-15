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
            var _notesDictionary = new Dictionary<NotesType, GameObject>();
            foreach (var _prefab in _notesPrefabs.Prefabs)
            {
                _notesDictionary.Add(_prefab.Type, _prefab.gameObject);
            }

            Container.Bind<Dictionary<NotesType, GameObject>>()
                .FromInstance(_notesDictionary);
            Container.Bind<NotesFactory>()
                .AsSingle();
        }
    }
}