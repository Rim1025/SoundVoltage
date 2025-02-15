using System.Collections.Generic;
using Abstracts;
using Interfaces;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// ノーツオブジェクトプール
    /// </summary>
    public class NotesPool: IGetNotesPool,IAddNotesPool
    {
        private List<NotesCore> _notesList = new();

        public List<NotesCore> GetPool()
        {
            return _notesList;
        }

        public void AddNotes(NotesCore notes)
        {
            _notesList.Add(notes);
        }
    }
}