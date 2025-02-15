using System;
using Model;
using UnityEngine;

namespace Interfaces
{
    public interface IJudgeNotes
    {
        public IObservable<JudgeType> CalScore { get; }
        public JudgeType Judge(Vector3 position);
        public JudgeType Judge(JudgeType type);
    }
}