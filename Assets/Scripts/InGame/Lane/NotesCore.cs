using System;
using UnityEngine;
using UniRx;

namespace InGame
{
    public abstract class NotesCore: MonoBehaviour, INotes
    {
        public abstract void Push();
        public abstract void Down();
        public abstract void Up();
        public abstract void Activate();
        public abstract void EndActivate();
        public bool Active { get; protected set; }
        
        public bool InGenerate { get; protected set; }

        public NotesType Type { get; private set; }
        
        public IDisposable Disposable { get; protected set; }
        
        protected Vector3 FirstPosition { get; private set; }

        protected NotesUpdate UpdateNotes;

        private Subject<NotesType> _subject = new Subject<NotesType>();

        public IObservable<NotesType> MissSubject => _subject;

        public void Dispose()
        {
            if (Disposable != null)
            {            
                Disposable.Dispose();
            }
        }

        public void Init(NotesType type, NotesUpdate update)
        {
            FirstPosition = gameObject.transform.position;
            UpdateNotes = update;
            Disposable = update.Timer.Subscribe().AddTo(this);
            Type = type;
        }

        protected void Miss(NotesType type)
        {
            _subject.OnNext(type);
        }
    }
}