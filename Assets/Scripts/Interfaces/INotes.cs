using Model;

namespace Interfaces
{
    public interface INotes
    {
        public NotesType Type { get; }
        public bool Active { get;}
        public LaneName MyLane { get; }
        public void Activate(LaneName laneName,float speed);
        public void Push();
    }
}