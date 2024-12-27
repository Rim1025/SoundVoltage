using Interfaces;
using Model;
using Zenject;

namespace Installer
{
    public class JudgeNotesInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IJudgeNotes>()
                .To<JudgeNotes>()
                .AsSingle()
                .NonLazy();
        }
    }
}