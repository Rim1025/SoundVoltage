using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Model
{
    /// <summary>
    /// イベントを実行
    /// </summary>
    public class GameManager: MonoBehaviour
    {
        private GameEvents _gameEvents;

        private void Awake()
        {
            _gameEvents = new GameEvents();
        }

        private async void Start()
        {
            await Task.Run(() => _gameEvents.GameStart());

            // 毎フレーム実行
            this.UpdateAsObservable()
                .Select(_ => Time.deltaTime)
                .Subscribe(t =>
                {
                    _gameEvents.GameUpdate(t);
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            // オブジェクト破壊時に終了
            _gameEvents.Complete();
        }
    }
}