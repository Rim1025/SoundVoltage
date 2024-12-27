using System.Collections.Generic;
using Abstracts;
using Defaults;
using Interfaces;
using UnityEngine;
using View.Notes;
using Zenject;
using Vector3 = System.Numerics.Vector3;

namespace Model
{
    /// <summary>
    /// ノーツの管理
    /// </summary>
    public class NotesSpawner: INotesSpawner
    {
        private NotesFactory _factory;
        private GameObject _notesParent;

        public List<INotes> NotesList { get; private set; } = new();
        
        public NotesSpawner(NotesFactory factory)
        {
            _factory = factory;
            _notesParent = new GameObject("NotesParent");
        }
        
        /// <summary>
        /// ノーツの生成
        /// </summary>
        /// <param name="lane">召喚するレーン名</param>
        /// <param name="type">ノーツの種類</param>
        /// <returns>生成したノーツ</returns>
        public INotes Spawn(LaneName lane,NotesType type)
        {
            foreach (var _n in NotesList)
            {
                if (!_n.Active)
                {
                    _n.Activate(lane);
                    return _n;
                }
            }

            var _notes = _factory.Create(type, lane);
            NotesList.Add(_notes);
            _notes.Activate(lane);
            return _notes;
        }
    }
}

