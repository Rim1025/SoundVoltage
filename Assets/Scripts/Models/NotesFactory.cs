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
        public NotesCore Create(NotesType type,Transform parent)
        {
            if (!_objects.TryGetValue(type,out var _prefab))
            {
                Err.Err.ViewErr(type + "は登録されていません");
                return null;
            }

            var _notes = _container.InstantiatePrefabForComponent<NotesCore>(_prefab);
            if (_notes is MonoBehaviour _monoBehaviour)
            {
                _monoBehaviour.transform.SetParent(parent);
            }
            
            return _notes;
        }
    }
}