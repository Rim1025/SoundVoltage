namespace Services
{
    public enum StatusType
    {
        Delay,
        Speed,
        Voltage
    }
    [System.Serializable]
    public class MusicStatus
    {
        public string MusicName;
        public float DelayPosition;
        public float NotesSpeed;
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