using Model;
using UniRx;

namespace Interfaces
{
    public interface IInputProvider
    {
        Subject<LaneName> LanePush { get; }
        Subject<LaneName> LanePushing { get; }
    }
}