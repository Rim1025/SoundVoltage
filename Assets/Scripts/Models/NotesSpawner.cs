using System.Collections.Generic;
using Interfaces;
using UnityEngine;

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
        public INotes Spawn(LaneName lane,NotesType type,float speed)
        {
            foreach (var _n in NotesList)
            {
                if (!_n.Active)
                {
                    _n.Activate(lane,speed);
                    return _n;
                }
            }

            var _notes = _factory.Create(type);
            NotesList.Add(_notes);
            _notes.Activate(lane,speed);
            return _notes;
        }
    }
}

