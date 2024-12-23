using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Model
{
    public class MusicStatus: IMusicStatus
    {
        public string MusicName { get; }
        public float DelayTime { get; }
        public float NotesSpeed { get; }

        public MusicStatus(string name, float time, float speed)
        {
            MusicName = name;
            DelayTime = time;
            NotesSpeed = speed;
        }
    }
}