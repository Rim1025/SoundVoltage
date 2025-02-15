using System.IO;
using Defaults;
using Model;
using UnityEngine;

namespace Services
{
    public static class JsonSaver
    {
        public static MusicStatus Save()
        {
            var _status = new MusicStatus("FirstCreate", 0, 0,5);
            var _json = JsonUtility.ToJson(_status);
            File.WriteAllText(GameData.MusicDataPath, _json);
            return _status;
        }

        public static void Save(MusicStatus status)
        {
            var _json = JsonUtility.ToJson(status);
            File.WriteAllText(GameData.MusicDataPath,_json);
        }
    }
}