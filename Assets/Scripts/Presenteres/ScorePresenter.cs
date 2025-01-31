using System;
using System.Collections.Generic;
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

        private List<IDisposable> _judgeDisposable = new();
        private IDisposable _bloomDisposable;
        private float _judgeViewTime = 0;
        private float _bloomViewTime = 0;
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
                SetBloom(j);
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
            foreach (var _disposable in _judgeDisposable)
            {
                _disposable.Dispose();
            }
            _judgeViewTime = 0f;
            _viewer.SetJudge(type.ToString(),color);
            _judgeDisposable.Add(GameEvents.UpdateGame
                .Select(t =>
                {
                    return _judgeViewTime += t;
                })
                .Where(_ => _judgeViewTime > GameData.JudgeViewTime)
                .Subscribe(_ =>
                {
                    _viewer.SetJudge("", color);
                }));
        }
        
        public void SetBloom(JudgeType type)
        {
            _bloomViewTime = 0f;
            _viewer.SetBloom(GameData.JudgeBloom[type]);
            _bloomDisposable = GameEvents.UpdateGame
                .Select(_ => _bloomViewTime += Time.deltaTime)
                .Where(_ => _bloomViewTime > GameData.BloomTime)
                .Subscribe(_ =>
                {
                    _viewer.SetBloom(GameData.JudgeBloom[JudgeType.Miss]);
                });
        }
    }
}