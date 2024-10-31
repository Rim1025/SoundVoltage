using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

namespace InGame.Audio
{
    public class AudioReader
    {
        public static async Task<AudioClip> ReadAudio(string fileName)
        {
            fileName = fileName + ".mp3";
            AudioClip _audioData;// CSVファイルの中身を入れるリスト
            _audioData = await _findAudio(fileName);
            return _audioData; // ResourcesにあるCSVファイルを格納
        }
        
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
                        AudioClip audioClip = await LoadMP3AsAudioClipAsync(fileName,_file);
                        return audioClip;
                    }
                }
            }

            return null;
        }
        
        // 非同期でMP3をAudioClipに変換するメソッド
        private static async Task<AudioClip> LoadMP3AsAudioClipAsync(string fileName,string folderPath)
        {
            if (!File.Exists(folderPath))
            {
                throw new FileNotFoundException($"File not found: {folderPath}");
            }

            string url = "file://" + Path.GetFullPath(folderPath);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                var operation = www.SendWebRequest();

                // 非同期でリクエストの完了を待つ
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load MP3 file: {folderPath}, Error: {www.error}");
                    return null;
                }

                // AudioClipを取得
                return DownloadHandlerAudioClip.GetContent(www);
            }
        }

        // 複数のmp3ファイルを一括でロードするメソッド
        private static async Task LoadAllMP3sAsync(string folderPath)
        {
            string[] mp3Files = Directory.GetFiles(folderPath, "*.mp3");

            foreach (string filePath in mp3Files)
            {
                string fileName = Path.GetFileName(filePath);
                try
                {
                    AudioClip audioClip = await LoadMP3AsAudioClipAsync(fileName,folderPath);
                    if (audioClip != null)
                    {
                        Debug.Log($"Loaded MP3 file: {fileName}, Length: {audioClip.length} seconds");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading MP3 file {fileName}: {e.Message}");
                }
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