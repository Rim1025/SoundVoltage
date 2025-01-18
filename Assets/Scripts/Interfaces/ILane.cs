using Model;

namespace Interfaces
{
    public interface ILane
    {
        public void Push(LaneName lane);
        public void OnPush(LaneName lane);
    }
}