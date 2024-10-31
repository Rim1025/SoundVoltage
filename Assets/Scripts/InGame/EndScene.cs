using System;
using DefaultNamespace;
using Input;
using UnityEngine;
using UniRx;

namespace InGame
{
    public class EndScene: MonoBehaviour
    {
        private LoadScene _loadScene;
        private IDisposable _disposable;
        private void OnEnable()
        {
            _loadScene = new LoadScene();
            _disposable = KeyManager.KeyDownSubject.Subscribe(x =>
            {
                PushButton();
            }).AddTo(this);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        public void PushButton()
        {
            //LoadScene.SceneLoad("Start");
            LoadScene.SceneLoad("InGame");
        }
    }
}