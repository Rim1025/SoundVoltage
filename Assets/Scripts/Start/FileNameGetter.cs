using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Start
{
    public class FileNameGetter
    {
        private List<string> _fileName;
    
        /// <summary>
        /// 指定フォルダ下のファイル名を取得
        /// </summary>
        /// <param name="_filePath">フォルダパス</param>
        /// <returns>取得したデータ</returns>
        public List<string> GetFileName (string _filePath)
        {
            _fileName = new List<string>();

            // ディレクトリが存在するか確認
            if (Directory.Exists(_filePath))
            {
                // ディレクトリ内のファイルを取得
                string[] _files = Directory.GetDirectories(_filePath);

                // ファイル名のみをリストに追加
                foreach (string _file in _files)
                {
                    _fileName.Add(Path.GetFileName(_file));
                }
            }
            else
                Debug.LogError("指定されたフォルダが見つかりません: " + _filePath);
            return _fileName;
        }
    }
}