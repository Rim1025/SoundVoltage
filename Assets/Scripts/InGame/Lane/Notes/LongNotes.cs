using System;
using System.Collections.Generic;
using Status;
using UnityEngine;
using UniRx;

namespace InGame
{
    public class LongNotes: NotesCore
    {
        [SerializeField] private LongFirstNotes _firstNotes;
        [SerializeField] private LongMiddleNotes _longNotes;
        
        private Subject<NotesType> _subject = new Subject<NotesType>();

        public IObservable<NotesType> LongNotesSubject => _subject;

        private List<NotesCore> _notesMiddle = new List<NotesCore>();
        private Vector3 _firstPosition;
        private IDisposable _disposable;
        private int _fpsCounter;
        private float _directionFps;
        public override void Push()
        {
            if (Active)
            {
                _firstNotes.Push();
            }
        }

        public override void Down()
        {
            _subject.OnNext(NotesType.LongMiddle);
            var _counter = 0;
            foreach (var _notes in _notesMiddle)
            {
                if (_notes.Active)
                {
                    _counter++;
                }
            }

            if (_counter == 0)
            {
                Active = false;
                gameObject.SetActive(false);
                Dispose();
            }
        }

        public override void Up()
        {
            if (!_firstNotes.Active)
            {
                foreach (var _notes in _notesMiddle)
                {
                    _notes.Push();
                    Miss(NotesType.LongMiddle);
                    Down();
                }
            }
        }

        public override void Activate()
        {
            gameObject.transform.position = FirstPosition;
            gameObject.SetActive(true);
            Active = true;
            InGenerate = true;
            var _status = new StatusManager();
            Disposable = UpdateNotes.Timer.Subscribe(x =>
            {
                if (Active)
                {
                    _status.LoadStatus();
                    gameObject.transform.position -= new Vector3(0, 0, _status.MoveSpeed / UpdateNotes.FPS);
                }
            }).AddTo(this);

            var _nextTime = UpdateNotes.TimeCount;
            _directionFps = _status.MoveSpeed / UpdateNotes.FPS;
            _fpsCounter = 0;
            _disposable = UpdateNotes.Timer.Subscribe(x =>
            {
                _fpsCounter++;
                if (x >= _nextTime && InGenerate)
                {
                    var _direction = _directionFps * _fpsCounter;
                    _fpsCounter = 0;
                    _nextTime += (float)1 / ((float)_status.Bpm / 60);
                    Generate(_direction);
                }
            }).AddTo(this);
            _firstNotes.Activate();
        }

        private void Generate(float direction)
        {
            foreach (var _notes in _notesMiddle)
            {
                if (_notes.Type == NotesType.LongMiddle && !_notes.Active)
                {
                    var _position = FirstPosition;
                    _position.z -= direction / 2;
                    _notes.transform.position = _position;
                    _notes.Activate();
                    return;
                }
            }
            var _middleNotes = Instantiate(_longNotes, FirstPosition, Quaternion.identity, this.transform);
            NotesCore _notesCore = _middleNotes;
            _notesCore.Init(NotesType.LongMiddle,UpdateNotes);

            var _scale = _middleNotes.transform.localScale;
            _scale.z = direction / Defaults.ConstNotesScale;
            _middleNotes.transform.localScale = _scale;
            
            var _middlePosition = FirstPosition;
            _middlePosition.z -= direction / 2;
            _middleNotes.transform.position = _middlePosition;

            _middleNotes.InitMiddle(this, direction);
            _middleNotes.Activate();
            _notesMiddle.Add(_middleNotes);
        }

        public override void EndActivate()
        {
            Generate(_directionFps * _fpsCounter);
            InGenerate = false;
            _disposable.Dispose();
        }
    }
}