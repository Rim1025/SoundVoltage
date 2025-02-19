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
        // 本体
        private List<NotesCore> _notesList = new();

        /// <summary>
        /// ノーツオブジェクトプールを取得
        /// </summary>
        /// <returns></returns>
        public List<NotesCore> GetPool()
        {
            return _notesList;
        }

        /// <summary>
        /// ノーツオブジェクトプールに追加
        /// </summary>
        /// <param name="notes"></param>
        public void AddNotes(NotesCore notes)
        {
            _notesList.Add(notes);
        }
    }
}