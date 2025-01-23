using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// ノーツオブジェクトプール
    /// </summary>
    public class NotesPool: IGetNotesPool,IAddNotesPool
    {
        private List<INotes> _notesList = new();

        public List<INotes> GetPool()
        {
            return _notesList;
        }

        public void AddNotes(INotes notes)
        {
            _notesList.Add(notes);
        }
    }
}