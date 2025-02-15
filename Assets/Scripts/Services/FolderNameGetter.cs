using System.Collections.Generic;
using System.IO;

namespace Services
{
    public static class FolderNameGetter
    {
        public static List<string> PathUnderAll(string path)
        {
            var _fileName = new List<string>();

            // ディレクトリが存在するか確認
            if (Directory.Exists(path))
            {
                // ディレクトリ内のファイルを取得
                string[] _files = Directory.GetDirectories(path);

                // ファイル名のみをリストに追加
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