using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Services
{
    public static class CsvReader
    {
        /// <summary>
        /// Csvの読み込み
        /// </summary>
        /// <param name="filePath">パス</param>
        /// <returns>List<List<string>></returns>
        public static List<List<int>> Read(string filePath)
        {
            List<List<int>> _result = new List<List<int>>();
            try
            {
                if (!File.Exists(filePath))
                {
                    Err.Err.ViewErr("読み込み対象のscvファイルは存在しません\n空のファイルを作成します");
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
                using (var _stream = new StreamReader(filePath))
                {
                    string _line;
                    while ((_line = _stream.ReadLine())!= null)
                    {
                        _result.Add(_line.Split(',').Select(s => int.TryParse(s, out var n) ? n : ' ').ToList());
                    }
                    // NOTE: 最初の一行に説明があるため
                    
                }
            }
            catch (Exception e)
            {
                Err.Err.ViewErr($"csvの読み込み中にエラーが発生しました{e}");
            }

            return _result;
        }
    }
}