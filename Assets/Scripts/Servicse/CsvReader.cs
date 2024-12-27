using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Services
{
    public class CsvReader
    {
        public static List<List<string>> Read(string filePath)
        {
            List<List<string>> _result = new List<List<string>>();
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
                        string[] _val = _line.Split(',');
                        _result.Add(new List<string>(_val));
                        Debug.Log(new List<string>(_val));
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