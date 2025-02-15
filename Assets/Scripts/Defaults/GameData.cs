using System.Collections.Generic;
using Model;
using Services;
using UnityEngine;

namespace Defaults
{
    public static class GameData
    {
        public static readonly Dictionary<LaneName, Vector3> LanePositions = new ()
        {
            {LaneName.OuterRight,new Vector3(0,0,500)},
            {LaneName.InnerRight,new Vector3(10,0,500)},
            {LaneName.InnerLeft,new Vector3(20,0,500)},
            {LaneName.OuterLeft,new Vector3(30,0,500)},
            
            {LaneName.BigRight,new Vector3(5,-0.1f,500)},
            {LaneName.BigLeft,new Vector3(25,-0.1f,500)}
        };
        
        public static readonly Vector3 JudgeViewerPosition = new Vector3(0, -14, 0);
        public static readonly float[] JudgePosition = { 10, 20, 50 };
        public static readonly float[] JudgeScore = { 1000, 500, 10};
        public static readonly float JudgeViewTime = 3;
        public static readonly int Bpm = 120;

        public static readonly Dictionary<JudgeType, Color> JudgeColors = new ()
        {
            { JudgeType.Perfect, new Color(0.9568627f, 0.3686275f, 1) },
            { JudgeType.Good, new Color(1, 0.5647059f, 0.372549f) },
            { JudgeType.Miss, new Color(0.3686275f, 1, 0.8509804f) }
        };
        
        public static readonly Vector3 NormalNotesScale = new Vector3(0.9f, 1, 0.3f);
        public static readonly Vector3 BigNotesScale = new Vector3(1.9f, 1, 0.3f);
        
        public static readonly Dictionary<JudgeType, float> JudgeBloom = new()
        {
            { JudgeType.Perfect, 1.4f },
            { JudgeType.Good, 1.2f },
            { JudgeType.Miss, 1 }
        };

        public static readonly float BloomTime = 0.5f;
        public const int BpmLane = 6;

        public static readonly string MusicDataPath = Application.dataPath + @"\Data\MusicData.json";
        public static readonly string DataPath = Application.dataPath + @"\Data";

        public const string StartScene = "Start";
        public const string InGameScene = "InGame";
        
        // Start
        public static readonly Vector3 ButtonSos = new Vector3(0, 0, 0);
        public const float ButtonMoveDelay = 0.3f;

        public static readonly Dictionary<StatusType, float> StatusMass = new()
        {
            { StatusType.Delay, 0.1f },
            { StatusType.Speed, 5 },
            { StatusType.Voltage, 1 }
        };
        
        public static readonly Dictionary<StatusType, float> StatusDefault = new()
        {
            { StatusType.Delay, 0f },
            { StatusType.Speed, 150 },
            { StatusType.Voltage, 5 }
        };
        
        public static readonly Dictionary<StatusType, float> StatusMax = new()
        {
            { StatusType.Delay, 1000000f },
            { StatusType.Speed, 300 },
            { StatusType.Voltage, 15 }
        };
        
        public static readonly Dictionary<StatusType, float> StatusMin = new()
        {
            { StatusType.Delay, -1000000f },
            { StatusType.Speed, 50 },
            { StatusType.Voltage, 1 }
        };
    }
}