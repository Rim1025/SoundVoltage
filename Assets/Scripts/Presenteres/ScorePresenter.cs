using System;
using System.Collections.Generic;
using View;
using Defaults;
using Interfaces;
using UniRx;
using UnityEngine;
using Model;
using Services;
using Zenject;

namespace Presenters
{
    /// <summary>
    /// スコアとコンボ数の管理
    /// </summary>
    public class ScorePresenter: MonoBehaviour
    {
        [SerializeField] private ScoreViewer _viewer;
        private IScoreModel _score;
        private MusicStatus _status;

        [Inject]
        public void Construct(IScoreModel score,MusicStatus status)
        {
            _score = score;
            _status = status;
        }
        
        // 表示中に再表示を行いたい場合のリスト
        private List<IDisposable> _judgeDisposable = new();
        private List<IDisposable> _bloomDisposable = new();
        // 表示時間管理用
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
                // ジャッチ時に光らせる
                SetBloom(j);
            });
        }
        
        private void SetScore(float score)
        {
            _viewer.SetScore(((int)score).ToString());
        }

        private void SetCombo(int combo)
        {
            // コンボ数が0なら表示しない
            _viewer.SetCombo(combo == 0 ? "" : combo.ToString());
        }

        private void SetJudgeResult(JudgeType type,Color color)
        {
            // 今までの表示を消去
            foreach (var _disposable in _judgeDisposable)
            {
                _disposable.Dispose();
            }
            // 一定時間表示して消去
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
                })
            );
        }
        
        private void SetBloom(JudgeType type)
        {
            // 今までの光を消去
            foreach (var _disposable in _bloomDisposable)
            {
                _disposable.Dispose();
            }
            // 一定時間光らせてデフォルト値に
            _bloomViewTime = 0f;
            _viewer.SetBloom(GameData.JudgeBloom[type] * _status.Voltage);
            _bloomDisposable.Add(GameEvents.UpdateGame
                .Select(_ => _bloomViewTime += Time.deltaTime)
                .Where(_ => _bloomViewTime > GameData.BloomViewTime)
                .Subscribe(_ =>
                {
                    _viewer.SetBloom(GameData.JudgeBloom[JudgeType.Miss] * _status.Voltage);
                })
            );
        }
    }
}