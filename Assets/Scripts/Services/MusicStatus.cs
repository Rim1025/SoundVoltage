namespace Services
{
    public enum StatusType
    {
        Delay,
        Speed,
        Voltage
    }
    
    /// <summary>
    /// プレイヤーによって設定される値
    /// </summary>
    [System.Serializable]
    public class MusicStatus
    {
        /// <summary>
        /// 曲名
        /// </summary>
        public string MusicName;
        
        /// <summary>
        /// 判定位置のずれ(+->奥に移動)
        /// </summary>
        public float DelayPosition;
        
        /// <summary>
        /// ノーツ移動速度
        /// </summary>
        public float NotesSpeed;
        
        /// <summary>
        /// レーンの発光度合い
        /// </summary>
        public float Voltage;

        public MusicStatus(string name, float position, float speed,float valtage)
        {
            MusicName = name;
            DelayPosition = position;
            NotesSpeed = speed;
            Voltage = valtage;
        }
    }
}