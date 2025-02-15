using Zenject;

namespace Installer
{
    public class AudioReaderInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Mp3ToAudioClip>()
                .AsSingle()
                .NonLazy();
        }
    }
}