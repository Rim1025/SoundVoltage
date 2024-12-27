using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Defaults
{
    public static class GameData
    {
        public static readonly Dictionary<LaneName, Vector3> LanePositions = new Dictionary<LaneName, Vector3>()
        {
            {LaneName.OuterRight,new Vector3(0,0,500)},
            {LaneName.InnerRight,new Vector3(10,0,500)},
            {LaneName.InnerLeft,new Vector3(20,0,500)},
            {LaneName.OuterLeft,new Vector3(30,0,500)},
            
            {LaneName.BigRight,new Vector3(5,-0.1f,500)},
            {LaneName.BigLeft,new Vector3(25,-0.1f,500)}
        };

        public const float NotesMoveSpeed = 100;
        public static readonly Vector3 JudgeViewerPosition = new Vector3(0, -14, 0);
        public static readonly float[] JudgePosition = { 10, 20, 50 };
        public static readonly float[] JudgeScore = { 1000, 500, 10};
        public const float JudgeViewTime = 3;

        public static readonly Dictionary<JudgeType, Color> JudgeColors = new Dictionary<JudgeType, Color>()
        {
            { JudgeType.Perfect, new Color(0.9568627f, 0.3686275f, 1) },
            { JudgeType.Good, new Color(1, 0.5647059f, 0.372549f) },
            { JudgeType.Miss, new Color(0.3686275f, 1, 0.8509804f) }
        };
    }
}