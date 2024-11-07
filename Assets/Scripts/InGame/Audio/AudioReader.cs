using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using DefaultNamespace;

namespace InGame.Audio
{
    public class AudioReader
    {
        public static async Task<AudioClip> ReadAudio(string fileName)
        {
            fileName = fileName + ".mp3";
            // Resourcesフォルダのパスを取得
            string _resourcesPath = Application.dataPath + @"/Resource";
            // Resourcesフォルダ以下のすべてのファイルを取得
            string[] _files = System.IO.Directory.GetFiles(_resourcesPath, "*.mp3", SearchOption.AllDirectories);
            foreach (string _file in _files)
            {
                // ファイル名を比較
                if (Path.GetFileName(_file) == fileName)
                {
                    string _relativePath = GetRelativePath(_file, _resourcesPath);
                    if (_relativePath != null)
                    {
                        // 拡張子を除いたファイル名を取得
                        //string _resourcePath = _relativePath.Substring(0, _relativePath.LastIndexOf('.'));
                        AudioClip _audioClip = await LoadMp3AsAudioClipAsync(_file);
                        return _audioClip;
                    }
                }
            }

            return null;
        }
        /*
        private static async Task<AudioClip> _findAudio(string fileName)
        {
            // Resourcesフォルダのパスを取得
            string _resourcesPath = Application.dataPath + @"/Resource";
            // Resourcesフォルダ以下のすべてのファイルを取得
            string[] _files = System.IO.Directory.GetFiles(_resourcesPath, "*.mp3", SearchOption.AllDirectories);
            foreach (string _file in _files)
            {
                // ファイル名を比較
                if (Path.GetFileName(_file) == fileName)
                {
                    string _relativePath = GetRelativePath(_file, _resourcesPath);
                    if (_relativePath != null)
                    {
                        // 拡張子を除いたファイル名を取得
                        string _resourcePath = _relativePath.Substring(0, _relativePath.LastIndexOf('.'));
                        AudioClip audioClip = await LoadMp3AsAudioClipAsync(_file);
                        return audioClip;
                    }
                }
            }

            return null;
        }*/
        
        // 非同期でMP3をAudioClipに変換するメソッド
        private static async Task<AudioClip> LoadMp3AsAudioClipAsync(string folderPath)
        {
            if (!File.Exists(folderPath))
            {
                throw new FileNotFoundException($"File not found: {folderPath}");
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
                    ErrDisplay.ViewErr("リクエストの完了に失敗しました");
                    return null;
                }

                // AudioClipを取得
                return DownloadHandlerAudioClip.GetContent(_www);
            }
        }

        /// <summary>
        /// Resourcesフォルダからの相対パスに変換
        /// </summary>
        private static string GetRelativePath(string fullPath, string basePath)
        {
            if (fullPath.StartsWith(basePath))
                return fullPath.Substring(basePath.Length + 1).Replace('\\', '/');
            else
                return null;
        }
    }
}