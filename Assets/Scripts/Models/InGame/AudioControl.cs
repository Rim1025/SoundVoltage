using Services;
using System.IO;
using Defaults;
using Interfaces;
using UnityEngine;
using Zenject;
using UniRx;

namespace Model
{
    /// <summary>
    /// 楽曲の取得、再生
    /// </summary>
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
            var _fileName = _status.MusicName + ".mp3";
            // Dataフォルダのパスを取得
            string _dataPath = GameData.DataPath;
            // dataフォルダ以下のすべてのファイルを取得
            string[] _files = System.IO.Directory.GetFiles(_dataPath, "*.mp3", SearchOption.AllDirectories);
            foreach (string _file in _files)
            {
                // ファイル名を比較選んだ曲の名前と一致するファイルを変換
                if (Path.GetFileName(_file) == _fileName)
                {
                    string _relativePath = GetRelativePath(_file, _dataPath);
                    if (_relativePath != null)
                    {
                        _audioSource.clip = await _audioReader.Convert(@GameData.DataPath +"/" +_relativePath);
                    }
                }
            }
            
            //NOTE: 読み込んで置くと再生にラグがない
            Play();
            Stop();
            
            // 曲の開始を講読
            //NOTE: ノーツの到達に合わせる必要があるため
            _notesSpawner.StartAudio.Subscribe(_ =>
            {
                Play();
            });

        }

        /// <summary>
        /// 楽曲再生
        /// </summary>
        public void Play()
        {
            _audioSource.Play();
        }

        /// <summary>
        /// 楽曲停止
        /// </summary>
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