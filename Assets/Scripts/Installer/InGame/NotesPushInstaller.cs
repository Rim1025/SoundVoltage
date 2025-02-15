using Model;
using Zenject;

namespace Installer
{
    public class NotesPushInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NotesPush>()
                .AsSingle()
                .NonLazy();
        }
    }
}