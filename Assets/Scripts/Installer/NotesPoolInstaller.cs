using Interfaces;
using Model;
using Zenject;

namespace Installer
{
    public class NotesPoolInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            var _notesPool = new NotesPool();
            Container.Bind<IGetNotesPool>()
                .To<NotesPool>()
                .FromInstance(_notesPool)
                .AsCached()
                .NonLazy();
            Container.Bind<IAddNotesPool>()
                .To<NotesPool>()
                .FromInstance(_notesPool)
                .AsCached()
                .NonLazy();
        }
    }
}