using Interfaces;
using Model;
using Zenject;

namespace Installer
{
    public class ScoreInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IScoreModel>()
                .To<ScoreModel>()
                .AsSingle()
                .NonLazy();
        }
    }
}