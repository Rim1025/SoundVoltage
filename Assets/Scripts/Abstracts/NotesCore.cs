﻿using System;
using Defaults;
using Interfaces;
using Model;
using UnityEngine;
using UniRx;
using Zenject;

namespace Abstracts
{
    public abstract class NotesCore: MonoBehaviour, INotes
    {
        private IDisposable _updateDisposable;
        public abstract NotesType Type { get; }

        public abstract void OnActivate();

        public abstract void OnPush();
        public bool Active { get; private set; } = false;
        public LaneName MyLane { get; private set; }

        public void Activate(LaneName laneName)
        {
            Active = true;
            MyLane = laneName;
            this.transform.position = GameData.LanePositions[laneName];
            _updateDisposable = GameEvents.UpdateGame.Subscribe(t =>
            {
                transform.position += Vector3.back * GameData.NotesMoveSpeed * t;
            });
        }

        protected void DeActivate()
        {
            Active = false;
            _updateDisposable.Dispose();
        }

        public void Push()
        {
            OnPush();
        }
        
        public abstract class Factory : PlaceholderFactory<NotesCore>
        {
        };
    }
}