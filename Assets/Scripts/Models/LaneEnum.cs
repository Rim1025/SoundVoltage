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
        End = 0,
        Normal = 1,
        Long = 2,
        LongEnd = 3
    }
}