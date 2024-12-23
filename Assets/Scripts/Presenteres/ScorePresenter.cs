using System;
using View;
using Defaults;
using Interfaces;
using UniRx;
using UnityEngine;
using Model;
using Zenject;

namespace Classes
{
    /// <summary>
    /// スコアとコンボ数の管理
    /// </summary>
    public class ScorePresenter: MonoBehaviour
    {
        [SerializeField] private ScoreViewer _viewer;
        private IScoreModel _score;

        [Inject]
        public void Construct(IScoreModel score)
        {
            _score = score;
        }

        private IDisposable _disposable;
        public void Start()
        {
            _score.Score.Subscribe(x =>
            {
                SetScore(x);
            });
            _score.Combo.Subscribe(x =>
            {
                SetCombo(x);
            });
            _score.JudgeResult.Subscribe(j =>
            {
                SetJudgeResult(j,GameData.JudgeColors[j]);
            });
        }
        
        public void SetScore(float score)
        {
            _viewer.SetScore(((int)score).ToString());
        }

        public void SetCombo(int combo)
        {
            _viewer.SetCombo(combo == 0 ? "" : combo.ToString());
        }

        public void SetJudgeResult(JudgeType type,Color color)
        {
            var _viewTime = 0f;
            _viewer.SetJudge(type.ToString(),color);
            _disposable = GameEvents.UpdateGame
                .Select(_=> _viewTime += Time.deltaTime)
                .Where(_=>_viewTime > GameData.JudgeViewTime)
                .Subscribe(_=>
                {
                    _viewer.SetJudge("",color);
                    _disposable.Dispose();
                });
        }
    }
}