using InGame.Audio;
using Zenject;

namespace Installer
{
    public class AudioReaderInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AudioReader>()
                .AsSingle()
                .NonLazy();
        }
    }
}