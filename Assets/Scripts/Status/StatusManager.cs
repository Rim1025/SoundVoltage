using System.IO;
using UnityEngine;

namespace Status
{
    /// <summary>
    /// 曲の情報管理
    /// </summary>
    public class StatusManager
    {
        private string _filePath = Application.dataPath + @"\Data\Status.json";
        private Status _status = new Status();

        public StatusManager()
        {
            LoadStatus();
        }
            
        /// <summary>
        /// 保存
        /// </summary>
        public void SaveStatus()
        {
            var _json = JsonUtility.ToJson(_status);
            File.WriteAllText(_filePath, _json);
        }
    
        /// <summary>
        /// 読み込み
        /// </summary>
        public void LoadStatus()
        {
            if (File.Exists(_filePath))
            {
                var _json = File.ReadAllText(_filePath);
                _status = JsonUtility.FromJson<Status>(_json);
            }
            else
            {
                SaveStatus();
            }
        }
    
        /// <summary>
        /// 移動速度
        /// </summary>
        public float MoveSpeed
        {
            get => _status.MoveSpeed;
            set => _status.MoveSpeed = value;
        }
    
        /// <summary>
        /// 判定のずれ
        /// </summary>
        public float AdjustTouch
        {
            get => _status.AdjustTouch;
            set => _status.AdjustTouch = value;
        }
    
        /// <summary>
        /// 判定位置
        /// </summary>
        public float[] JudgePosition
        {
            get => _status.JudgePosition;
            set => _status.JudgePosition = value;
        }
    
        /// <summary>
        /// 曲名
        /// </summary>
        public string CsvName
        {
            get => _status.CsvName;
            set => _status.CsvName = value;
        }

        public int Bpm
        {
            get => _status.Bpm;
            set => _status.Bpm = value;
        }
        
     
        /// <summary>
        /// 保持する情報
        /// </summary>
        [System.Serializable]
        private class Status
        {
            public float MoveSpeed = 100;
            public float AdjustTouch = 0;
            public float[] JudgePosition = {10,25,50};
            public string CsvName = "テスト";
            public int Bpm = 120;
        }
    }
}