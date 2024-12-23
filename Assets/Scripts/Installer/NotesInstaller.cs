using System;
using Abstracts;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using View.Notes;
using Zenject;

namespace Installer
{
    public class NotesInstaller: MonoInstaller
    {
        [SerializeField] private GameObject[] _notesPrefabs;

        private GameObject[] _notesObjects;
        public override void InstallBindings()
        {
            foreach (var _type in Enum.GetValues(typeof(NotesType)))
            {
                foreach (var _notes in _notesPrefabs)
                {
                    if (_type.ToString() == _notes.name)
                    {
                        _notesObjects[(int)_type] = _notes;
                    }
                }
                
            }
            Container.BindFactory<NotesCore, Normal.Factory>()
                .To<Normal>()
                .FromComponentInNewPrefab(_notesObjects[(int)NotesType.Normal])
                .AsTransient();
            
            Container.BindFactory<NotesCore, Big.Factory>()
                .To<Big>()
                .FromComponentInNewPrefab(_notesObjects[(int)NotesType.Big])
                .AsTransient();
            
            Container.BindFactory<NotesCore, Long.Factory>()
                .To<Long>()
                .FromComponentInNewPrefab(_notesObjects[(int)NotesType.Long])
                .AsTransient();
        }
    }
}