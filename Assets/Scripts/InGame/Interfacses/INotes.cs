namespace InGame
{
    public interface INotes
    {
        public void Push();
        public void Down();
        public void Up();
        public void Activate();
        public bool Active { get; }
    }
}