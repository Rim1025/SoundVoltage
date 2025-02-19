using System.Collections.Generic;
using Abstracts;
using Model;
using UnityEngine;

namespace View
{
    /// <summary>
    /// オブジェクトとノーツタイプの辞書を所持
    /// </summary>
    public class NotesPrefabs: MonoBehaviour
    {
        [SerializeField] private NotesCore[] _notesPrefabs;

        /// <summary>
        /// 辞書
        /// </summary>
        /// <returns>オブジェクトとノーツタイプの辞書</returns>
        public Dictionary<NotesType, GameObject> GetNotesDictionary()
        {
            var _notesDictionary = new Dictionary<NotesType, GameObject>();
            foreach (var _prefab in _notesPrefabs)
            {
                _notesDictionary.Add(_prefab.Type, _prefab.gameObject);
            }

            return _notesDictionary;
        }
    }
}