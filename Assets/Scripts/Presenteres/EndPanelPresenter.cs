using Interfaces;
using Model;
using UnityEngine;
using View;
using Zenject;
using UniRx;

namespace Presenters
{
    /// <summary>
    /// リザルト管理
    /// </summary>
    public class EndPanelPresenter: MonoBehaviour
    {
        [SerializeField] private ResultViewer _viewer;
        private IScoreModel _score;
        // 最大コンボ数記録
        private int _maxCombo = 0;
        
        [Inject]
        public void Construct(IEndFlag flag,IScoreModel score)
        {
            _score = score;
            score.Combo.Subscribe(c =>
            {
                if (c > _maxCombo)
                {
                    _maxCombo = c;
                }
            });
            flag.EndSubject.Subscribe(_ => _viewer.View(_score.Score.Value, _maxCombo));
        }
    }
}