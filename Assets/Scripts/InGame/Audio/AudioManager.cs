using System;
using InGame.Status;
using UnityEngine;

namespace InGame.Audio
{
    public class AudioManager: MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Set(AudioClip audioClip, float time)
        {
            _audioSource.clip = audioClip;
            _audioSource.time = time;
            _audioSource.Play();
            _audioSource.Pause();
        }
        public void Play()
        {
            _audioSource.UnPause();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}