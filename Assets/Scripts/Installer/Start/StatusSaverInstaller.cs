using Model;
using Zenject;

namespace Installer
{
    public class StatusSaverInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StatusSaver>()
                .AsSingle()
                .NonLazy();
        }
    }
}