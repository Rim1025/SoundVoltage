using System.Collections.Generic;
using Abstracts;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Model
{
    public class NotesFactory
    {
        private DiContainer _container;
        private Dictionary<NotesType, GameObject> _objects;
        
        public NotesFactory(DiContainer container, Dictionary<NotesType, GameObject> objects)
        {
            _container = container;
            _objects = objects;
        }
        public INotes Create(NotesType type, LaneName name)
        {
            if (!_objects.TryGetValue(type,out var _prefab))
            {
                Err.Err.ViewErr(type + "は登録されていません");
            }

            var _notes = _container.InstantiatePrefabForComponent<INotes>(_prefab);
            _notes.Activate(name);
            
            return _notes;
        }
    }
}