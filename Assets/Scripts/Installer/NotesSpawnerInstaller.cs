using Model;
using Zenject;

namespace Installer
{
    public class NotesSpawnerInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NotesSpawner>()
                .AsSingle()
                .NonLazy();
        }
    }
}