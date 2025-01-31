﻿using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;
using Defaults;

namespace InGame.Audio
{
    public class AudioReader
    {
        private AudioClip _audioClip;
        
        public async Task<AudioClip> ReadAudio(string fileName)
        {
            fileName = fileName + ".mp3";
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
                         _audioClip = await LoadMp3AsAudioClipAsync(_file);
                        return _audioClip;
                    }
                }
            }

            return null;
        }
        
        // 非同期でMP3をAudioClipに変換するメソッド
        private async Task<AudioClip> LoadMp3AsAudioClipAsync(string folderPath)
        {
            if (!File.Exists(folderPath))
            {
                Err.Err.ViewErr("指定されたパスにファイルは存在しません");
            }

            string _url = "file://" + Path.GetFullPath(folderPath);

            using (UnityWebRequest _www = UnityWebRequestMultimedia.GetAudioClip(_url, AudioType.MPEG))
            {
                var _operation = _www.SendWebRequest();

                // 非同期でリクエストの完了を待つ
                while (!_operation.isDone)
                {
                    await Task.Yield();
                }

                if (_www.result != UnityWebRequest.Result.Success)
                {
                    Err.Err.ViewErr("リクエストの完了に失敗しました");
                    return null;
                }

                // AudioClipを取得
                _audioClip = DownloadHandlerAudioClip.GetContent(_www);
                return DownloadHandlerAudioClip.GetContent(_www);
            }
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