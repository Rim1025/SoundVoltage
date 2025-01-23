using Model;
using Zenject;
using Interfaces;

namespace Installer
{
    public class LaneInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILane>()
                .To<Lane>()
                .AsSingle()
                .NonLazy();
        }
    }
}