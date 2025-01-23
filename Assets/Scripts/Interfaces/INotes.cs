using Model;
using UniRx;
using UnityEngine;

namespace Interfaces
{
    public interface INotes
    {
        public NotesType Type { get; }
        public Vector3 Position { get; }
        public bool Active { get;}
        public LaneName MyLane { get; }
        public void Activate(LaneName laneName,MusicStatus status);
        public void Push();
    }
}