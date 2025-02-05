namespace Interfaces
{
    public interface ILongNotes
    {
        public void Grow();
        public void Miss();
        public bool IsPushed { get; }
    }
}