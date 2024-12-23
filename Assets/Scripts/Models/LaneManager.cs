using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Model
{
    public enum LaneName
    {
        OuterRight,
        InnerRight,
        InnerLeft,
        OuterLeft,
        BigRight,
        BigLeft
    }

    public enum NotesType
    {
        Normal,
        Big,
        Long
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