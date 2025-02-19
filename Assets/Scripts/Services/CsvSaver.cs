using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// csv保存(作曲機能)
    /// </summary>
    public static class CsvSaver
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filePath">パス</param>
        /// <param name="data">内容(2重リスト形式)</param>
        public static void Save(string filePath,List<List<string>> data)
        {
            try
            {
                // ファイルがなければ生成
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
                }
                
                // 書き込み(上書き)
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

        /// <summary>
        /// リストをカンマ区切りcsvに変換
        /// </summary>
        /// <param name="data">リスト</param>
        /// <returns>カンマ区切りcsv</returns>
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