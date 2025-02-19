using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// イベント管理
    /// </summary>
    public class GameEvents
    {
        private static Subject<Unit> _start = new();
        
        /// <summary>
        /// シーンに入った際に一度だけ実行
        /// </summary>
        public static IObservable<Unit> StartGame => _start;

        private static Subject<float> _update = new();
        
        /// <summary>
        /// 毎フレーム実行
        /// </summary>
        public static IObservable<float> UpdateGame => _update;

        /// <summary>
        /// シーンに入った際に実行される内容の実行
        /// </summary>
        public void GameStart()
        {
            _start.OnNext(Unit.Default);
        }

        /// <summary>
        /// 毎フレーム実行される処理の実行
        /// </summary>
        /// <param name="time">前回から経過時間</param>
        public void GameUpdate(float time)
        {
            _update.OnNext(time);
        }

        /// <summary>
        /// イベントを破棄
        /// </summary>
        public void Complete()
        {
            _start.OnCompleted();
            _update.OnCompleted();
            _start = new Subject<Unit>();
            _update = new Subject<float>();
        }
    }
}