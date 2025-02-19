using System.Collections.Generic;
using Abstracts;
using UnityEngine;
using View;
using Zenject;

namespace Model
{
    /// <summary>
    /// ノーツの生成を行う
    /// </summary>
    public class NotesFactory
    {
        // コンテナ
        private DiContainer _container;
        // 生成するタイプに応じたオブジェクト
        private Dictionary<NotesType, GameObject> _objects;
        
        public NotesFactory(DiContainer container, NotesPrefabs notesPrefabs)
        {
            _container = container;
            _objects = notesPrefabs.GetNotesDictionary();
        }
        
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="type">生成するタイプ</param>
        /// <param name="parent">親オブジェクト</param>
        /// <returns>生成したノーツクラス</returns>
        public NotesCore Create(NotesType type,Transform parent)
        {
            if (!_objects.TryGetValue(type,out var _prefab))
            {
                Err.Err.ViewErr(type + "は登録されていません");
                return null;
            }
            
            // 登録されているオブジェクトを生成し、NotesCoreを抽出
            var _notes = _container.InstantiatePrefabForComponent<NotesCore>(_prefab);
            if (_notes is MonoBehaviour _monoBehaviour)
            {
                // 親オブジェクトを設定
                _monoBehaviour.transform.SetParent(parent);
            }
            
            return _notes;
        }
    }
}