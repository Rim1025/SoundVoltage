using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefaultNamespace;
using UnityEngine.Windows;
using File = System.IO.File;

/// <summary>
/// CSVを読んで中身の配列を返却
/// </summary>
public class CsvReader
{
    public static List<string[]> ReadCsv(string fileName)
    {
        fileName = fileName + ".csv";
        List<string[]> _csvData = new List<string[]>();// CSVファイルの中身を入れるリスト
        TextAsset _csvFile = _findCSV(fileName); // ResourcesにあるCSVファイルを格納

        if (_csvFile == null)
        {
            ErrDisplay.ViewErr("Resources下にcsvファイルが見つかりません" + fileName);
            return _csvData;
        }

        StringReader _reader = new StringReader(_csvFile.text); // TextAssetをStringReaderに変換

        while (_reader.Peek() != -1)
        {
            string _line = _reader.ReadLine(); // 1行ずつ読み込む
            _csvData.Add(_line.Split(',')); // csvDataリストに追加する
        }
        return _csvData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private static TextAsset _findCSV(string fileName)
    {
        // パスを取得
        string _resourcesPath = Application.dataPath + @"/Resource";
        // フォルダ以下のすべてのファイルを取得
        string[] _files = System.IO.Directory.GetFiles(_resourcesPath, "*.csv", SearchOption.AllDirectories);

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

                    string _fileText = File.ReadAllText(_file);

                    TextAsset _textAsset = new TextAsset(_fileText);
                    // ResourcesからCSVファイルをロード
                    return _textAsset;
                }
            }
        }
        return null;
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
