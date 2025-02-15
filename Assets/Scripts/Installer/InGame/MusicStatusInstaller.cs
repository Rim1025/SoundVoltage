using Interfaces;
using Model;
using Services;
using Zenject;

namespace Installer
{
    public class MusicStatusInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            var _status = JsonReader.Read();
            Container.Bind<MusicStatus>()
                .FromInstance(_status)
                .AsSingle()
                .NonLazy();
        }
    }
}