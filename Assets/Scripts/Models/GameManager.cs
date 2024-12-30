﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using View;
using Interfaces;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Services;

namespace Model
{
    public class GameManager: MonoBehaviour
    {
        private INotesSpawner _notesSpawner;

        private IJudgeNotes _judgeNotes; // testo

        [Inject]
        public void Construct(IJudgeNotes judgeNotes,INotesSpawner notesSpawner)
        {
            _judgeNotes = judgeNotes;// testo
            _notesSpawner = notesSpawner;
        }
        private async void Start()
        {
            GameEvents _gameEvents = new GameEvents();
            await Task.Run(() => _gameEvents.GameStart());

            this.UpdateAsObservable()
                .Select(_ => Time.deltaTime)
                .Subscribe(t =>
                {
                    _gameEvents.GameUpdate(t);
                })
                .AddTo(this);
            // tesuto
            _judgeNotes.Judge(new Vector3(0,0,10));
            CsvSaver.Save(Application.dataPath + @"/Data/test.csv",new List<List<string>>());
        }
    }
}