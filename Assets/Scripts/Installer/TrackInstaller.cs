using Model;
using Interfaces;
using UnityEngine;
using Zenject;

public class TrackInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var _music = new MusicStatus("hoge", 0, 100);
        Container
            .Bind<IMusicStatus>()
            .To<MusicStatus>()
            .FromInstance(_music);
    }
}