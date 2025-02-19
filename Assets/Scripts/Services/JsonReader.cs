using System.IO;
using Defaults;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// jsonの読み込み
    /// </summary>
    public static class JsonReader
    {
        /// <summary>
        /// MusicStatus読み込み
        /// </summary>
        /// <returns>結果</returns>
        public static MusicStatus Read()
        {
            if (File.Exists(GameData.MusicDataPath))
            {
                var _json = File.ReadAllText(GameData.MusicDataPath);
                var _status = JsonUtility.FromJson<MusicStatus>(_json);
                return _status;
            }

            JsonSaver.Create();
            return null;
        }
    }
}