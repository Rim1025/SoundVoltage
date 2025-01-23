using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using System.Text.Json.Serialization;

namespace Model
{
    [System.Serializable]
    public class MusicStatus
    {
        public string MusicName;
        public float DelayPosition;
        public float NotesSpeed;

        public MusicStatus(string name, float position, float speed)
        {
            MusicName = name;
            DelayPosition = position;
            NotesSpeed = speed;
        }
    }
}