﻿using System.Collections.Generic;
using Services;
using UnityEngine;
using System.IO;
using Interfaces;

namespace Model
{
    public class TrackReader
    {
        public static List<List<string>> Read(MusicStatus status)
        {
            Debug.Log(status.MusicName);
            var _fileName = status.MusicName + ".csv";
            // Dataフォルダのパスを取得
            string _dataPath = Application.dataPath + @"/Data";
            // Dataフォルダ以下のすべてのファイルを取得
            var _files = System.IO.Directory.GetFiles(_dataPath, "*.csv", SearchOption.AllDirectories);

            foreach (string _file in _files)
            {
                // ファイル名を比較
                if (Path.GetFileName(_file) == _fileName)
                {
                    return CsvReader.Read(_file);
                }
            }
            return null;
        }
    }
}