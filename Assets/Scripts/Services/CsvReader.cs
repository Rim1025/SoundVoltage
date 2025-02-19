using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// csvの読み込み
    /// </summary>
    public static class CsvReader
    {
        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>2重のリスト形式</returns>
        public static List<List<string>> Read(string filePath)
        {
            List<List<string>> _result = new ();
            try
            {
                // csvがない場合作成(作曲機能)
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
                
                // カンマ区切りcsvの読み込み
                using (var _stream = new StreamReader(filePath))
                {
                    string _line;
                    while ((_line = _stream.ReadLine())!= null)
                    {
                        _result.Add(_line.Split(',').ToList());
                    }
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