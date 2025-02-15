using Defaults;
using Interfaces;
using Services;
using UniRx;
using Zenject;

namespace Model
{
    public class MusicEnd
    {
        private IInputProvider _input;
        [Inject]
        public MusicEnd(IInputProvider input,IEndFlag endFlag)
        {
            _input = input;
            endFlag.EndSubject
                .First()
                .Subscribe(_ =>
                {
                    _input.Push
                        .First()
                        .Subscribe(_ =>
                        {
                            var _changer = new SceneChanger();
                            _changer.Change(GameData.StartScene);
                        });
                });
        }
    }
}