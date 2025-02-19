using System.Collections.Generic;
using System.IO;

namespace Services
{
    /// <summary>
    /// フォルダ名を取得
    /// </summary>
    public static class FolderNameGetter
    {
        /// <summary>
        /// 特定パス下全てのフォルダ名を取得
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>フォルダ名リスト</returns>
        public static List<string> PathUnderAll(string path)
        {
            var _fileName = new List<string>();

            // ディレクトリが存在するか確認
            if (Directory.Exists(path))
            {
                // ディレクトリ内のフォルダを取得
                string[] _files = Directory.GetDirectories(path);

                // フォルダ名のみをリストに追加
                foreach (string _file in _files)
                {
                    _fileName.Add(Path.GetFileName(_file));
                }
            }
            else
                Err.Err.ViewErr("指定されたフォルダが見つかりません: " + path);
            return _fileName;
        }
    }
}