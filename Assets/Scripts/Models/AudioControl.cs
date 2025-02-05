﻿using System;
using System.IO;
using System.Threading.Tasks;
using Defaults;
using Interfaces;
using UnityEngine;
using Zenject;
using UniRx;

namespace Model
{
    public class AudioControl: MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private Mp3ToAudioClip _audioReader;
        private MusicStatus _status;
        private INotesSpawner _notesSpawner;
        [Inject]
        public void Construct(Mp3ToAudioClip audioReader,MusicStatus status,INotesSpawner spawner)
        {
            _audioReader = audioReader;
            _status = status;
            _notesSpawner = spawner;
            
        }

        private async void Awake()
        {
            var fileName = _status.MusicName + ".mp3";
            // Dataフォルダのパスを取得
            string _dataPath = GameData.DataPath;
            // dataフォルダ以下のすべてのファイルを取得
            string[] _files = System.IO.Directory.GetFiles(_dataPath, "*.mp3", SearchOption.AllDirectories);
            foreach (string _file in _files)
            {
                // ファイル名を比較
                if (Path.GetFileName(_file) == fileName)
                {
                    string _relativePath = GetRelativePath(_file, _dataPath);
                    if (_relativePath != null)
                    {
                        _audioSource.clip = await _audioReader.Convert(@GameData.DataPath +"/" +_relativePath);
                    }
                }
            }
            
            Play();
            Stop();
            _notesSpawner.StartAudio.Subscribe(_ =>
            {
                Play();
            });

        }

        public void Play()
        {
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
        
        /// <summary>
        /// Resourcesフォルダからの相対パスに変換
        /// </summary>
        private string GetRelativePath(string fullPath, string basePath)
        {
            if (fullPath.StartsWith(basePath))
                return fullPath.Substring(basePath.Length + 1).Replace('\\', '/');
            else
                return null;
        }
    }
}