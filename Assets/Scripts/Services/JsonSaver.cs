using System.IO;
using Defaults;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// jsonの保存
    /// </summary>
    public static class JsonSaver
    {
        /// <summary>
        /// MusicStatusの作成
        /// </summary>
        /// <returns></returns>
        public static MusicStatus Create()
        {
            var _status = new MusicStatus("FirstCreate", 0, 0,5);
            var _json = JsonUtility.ToJson(_status);
            File.WriteAllText(GameData.MusicDataPath, _json);
            return _status;
        }

        /// <summary>
        /// MusicStatusの保存
        /// </summary>
        /// <param name="status"></param>
        public static void Save(MusicStatus status)
        {
            var _json = JsonUtility.ToJson(status);
            File.WriteAllText(GameData.MusicDataPath,_json);
        }
    }
}