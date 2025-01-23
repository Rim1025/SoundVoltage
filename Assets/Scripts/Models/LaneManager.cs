using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Model
{
    public enum LaneName
    {
        OuterRight,
        BigRight,
        InnerRight,
        InnerLeft,
        BigLeft,
        OuterLeft,
    }

    public enum NotesType
    {
        Normal = 1,
        Long = 2
    }
    public class LaneManager
    {
        public LaneManager()
        {
            GameEvents.StartGame
                .Subscribe(_ =>
                {
                });
        }
    }
}