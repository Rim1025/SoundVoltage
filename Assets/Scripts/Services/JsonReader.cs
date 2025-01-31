using Model;
using System.IO;
using Defaults;
using UnityEngine;

namespace Services
{
    public static class JsonReader
    {
        public static MusicStatus Read()
        {
            if (File.Exists(GameData.MusicDataPath))
            {
                var _json = File.ReadAllText(GameData.MusicDataPath);
                var _status = JsonUtility.FromJson<MusicStatus>(_json);
                Debug.Log(_json);
                return _status;
            }

            JsonSaver.Save();
            return null;
        }
    }
}