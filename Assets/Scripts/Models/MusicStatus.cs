using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using System.Text.Json.Serialization;

namespace Model
{
    public class MusicStatus
    {
        public string MusicName;
        public float DelayTime;
        public float NotesSpeed;
        
        public MusicStatus(string name, float time, float speed)
        {
            MusicName = name;
            DelayTime = time;
            NotesSpeed = speed;
        }
    }
}