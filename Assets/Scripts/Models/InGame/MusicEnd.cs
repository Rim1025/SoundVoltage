using Defaults;
using Interfaces;
using Services;
using UniRx;
using Zenject;

namespace Model
{
    /// <summary>
    /// ゲームシーンの終了
    /// </summary>
    public class MusicEnd
    {
        [Inject]
        public MusicEnd(IInputProvider input,IEndFlag endFlag)
        {
            endFlag.EndSubject
                .First()
                .Subscribe(_ =>
                {
                    input.Push
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