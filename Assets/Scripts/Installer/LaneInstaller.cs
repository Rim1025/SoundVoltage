using Model;
using Zenject;

namespace Installer
{
    public class LaneInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Lane>()
                .AsSingle()
                .NonLazy();
        }
    }
}