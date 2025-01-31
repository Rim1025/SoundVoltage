using System;
using Model;
using UnityEngine;

namespace Interfaces
{
    public interface IJudgeNotes
    {
        public IObservable<JudgeType> CalScore { get; }
        public void Judge(Vector3 position);
        public void Judge(JudgeType type);
    }
}