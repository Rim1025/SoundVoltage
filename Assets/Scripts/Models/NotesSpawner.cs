using System.Collections.Generic;
using Abstracts;
using Interfaces;
using UnityEngine;
using View.Notes;
using Zenject;

namespace Model
{
    /// <summary>
    /// ノーツの管理
    /// </summary>
    public class NotesSpawner: INotesSpawner
    {
        private Normal.Factory _normalFactory;
        private Big.Factory _bigFactory;
        private Long.Factory _lonFactory;
        
        private Dictionary<NotesType, NotesCore.Factory> _notesObject = new();
        private GameObject _notesParent;

        public List<NotesCore> NotesList { get; private set; } = new();

        [Inject]
        public NotesSpawner(Normal.Factory normalFactory,Big.Factory bigFactory,Long.Factory longFactory)
        {
            _normalFactory = normalFactory;
            _bigFactory = bigFactory;
            _lonFactory = longFactory;

            _notesObject = new Dictionary<NotesType, NotesCore.Factory>()
            {
                { NotesType.Normal, _normalFactory },
                { NotesType.Big, _bigFactory },
                //{ NotesType.Long, _lonFactory }
            };
            _notesParent = new GameObject("NotesParent");
        }
        
        /// <summary>
        /// ノーツの生成
        /// </summary>
        /// <param name="lane">召喚するレーン名</param>
        /// <param name="type">ノーツの種類</param>
        /// <returns>生成したノーツ</returns>
        public NotesCore Spawn(LaneName lane,NotesType type)
        {
            if (!_notesObject.TryGetValue(type,out var _factory))
                Err.Err.ViewErr("生成しようとしたノーツは登録されていません" + type.ToString());

            foreach (var _n in NotesList)
            {
                if (!_n.Active)
                {
                    _n.Activate(lane);
                    return _n;
                }
            }

            var _notes = _notesObject[type].Create();
            _notes.Activate(lane);
            return _notes;
        }
    }
}

