using Interfaces;
using Model;
using Zenject;

namespace Installer
{
    public class NotesSpawnerInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NotesSpawner>()
                .AsSingle();
            Container.Bind<INotesSpawner>()
                .To<NotesSpawner>()
                .FromResolve()
                .NonLazy();
            Container.Bind<IEndFlag>()
                .To<NotesSpawner>()
                .FromResolve()
                .NonLazy();
        }
    }
}