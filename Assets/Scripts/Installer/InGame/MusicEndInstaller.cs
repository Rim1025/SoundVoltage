using Interfaces;
using Model;
using Zenject;

namespace Installer
{
    public class MusicEndInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MusicEnd>()
                .AsSingle()
                .NonLazy();
        }
    }
}