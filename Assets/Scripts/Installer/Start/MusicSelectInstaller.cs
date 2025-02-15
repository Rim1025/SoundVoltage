using Model;
using Zenject;

namespace Installer
{
    public class MusicSelectInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MusicSelect>()
                .AsSingle()
                .NonLazy();
        }
    }
}