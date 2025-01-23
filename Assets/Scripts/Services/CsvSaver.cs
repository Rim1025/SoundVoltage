using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Services
{
    public static class CsvSaver
    {
        public static void Save(string filePath,List<List<string>> data)
        {
            Debug.Log("Save");
            try
            {
                if (!File.Exists(filePath))
                {
                    try
                    {
                        using (var _stream = File.Create(filePath))
                        {
                           
                        }
                    }
                    catch (Exception e)
                    {
                        Err.Err.ViewErr($"ファイルの作成中にエラーが発生しました{e}");
                        throw;
                    }
                    Err.Err.ViewErr("");
                }

                using (var _stream = new StreamWriter(filePath,false))
                {
                    var _csvData = ListToCsv(data);
                    _stream.Write(_csvData);
                }
            }
            catch (Exception e)
            {
                Err.Err.ViewErr($"csvのセーブ中にエラーが発生しました{e}");
            }
        }

        private static string ListToCsv(List<List<string>> data)
        {
            var _builder = new System.Text.StringBuilder();

            foreach (var _row in data)
            {
                _builder.AppendLine(string.Join(',', _row));
            }

            return _builder.ToString();
        }
    }
}