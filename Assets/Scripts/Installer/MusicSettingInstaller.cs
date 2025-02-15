using Model;
using Zenject;

namespace Installer
{
    public class MusicSettingInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MusicSetting>()
                .AsSingle()
                .NonLazy();
        }
    }
}