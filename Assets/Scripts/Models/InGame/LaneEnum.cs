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
        BigLeft,
    }

    public enum NotesType
    {
        Normal = 1,
        Long = 2,
        LongEnd = 3
    }
}