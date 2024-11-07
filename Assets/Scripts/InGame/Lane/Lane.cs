using System.Collections;
using System.Collections.Generic;
using InGame.Score;
using Status;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

namespace InGame
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] private NotesCore _normalNotesPrefab;
        [SerializeField] private NotesCore _longNotesPrefab;
        
        private NotesUpdate _update;
        private Dictionary<NotesType, NotesCore> _notesPrefab = new Dictionary<NotesType, NotesCore>();
        private List<NotesCore> _notesList = new List<NotesCore>();
        private StatusManager _statusManager;
        private bool _isPushing;

        private List<LongNotes> _longNotes = new List<LongNotes>();

        public void Init(NotesUpdate update)
        {
            _update = update;
            _statusManager = new StatusManager();
            _notesPrefab[NotesType.Normal] = _normalNotesPrefab;
            _notesPrefab[NotesType.LongStart] = _longNotesPrefab;
            _notesPrefab[NotesType.LongEnd] = _longNotesPrefab;
        }

        public void Generate(NotesType type)
        {
            foreach (var _notes in _notesList)
            {
                if (_notes.Type == type && !_notes.Active)
                {
                    _notes.Activate();
                    return;
                }
            }
            var _newNotes = Instantiate(_notesPrefab[type], this.transform.position, Quaternion.identity, this.transform);

            if (type == NotesType.LongStart)
            {
                var _long =_newNotes.gameObject.GetComponent<LongNotes>();
                _longNotes.Add(_long);
                _long.LongNotesSubject.Subscribe(x =>
                {
                    if (_isPushing)
                    {
                        ScoreManager.Judge(JudgeType.Perfect,0.5f);
                    }
                    else
                    {
                        ScoreManager.Judge(JudgeType.Miss,0.5f);
                    }
                }).AddTo(this);
            }

            _newNotes.MissSubject.Subscribe(x =>
            {
                if (x == NotesType.LongMiddle)
                {
                    ScoreManager.Judge(JudgeType.Miss,0.5f);
                }
                ScoreManager.Judge(JudgeType.Miss);
            });
            
            _newNotes.Init(type,_update);
            _newNotes.Activate();
            _notesList.Add(_newNotes);
        }

        public void EndLong()
        {
            foreach (var _long in _longNotes)
            {
                if (_long.InGenerate)
                {
                    _long.EndActivate();
                }
            }
        }
        
        public void Push()
        {
            NotesCore _bestNotes = null;
            var _maxJudgePosition = Mathf.Abs(_statusManager.JudgePosition[(int)JudgeType.Miss] *
                _statusManager.MoveSpeed / Defaults.ConstMoveSpeed);
            foreach (var _notes in _notesList)
            {
                var _notesPos = Mathf.Abs(_notes.transform.position.z - _statusManager.AdjustTouch);
                if (_notesPos < _maxJudgePosition && _notes.Active)
                {
                    if (_bestNotes == null || Mathf.Abs(_bestNotes.transform.position.z - _statusManager.AdjustTouch) > _notesPos)
                    {
                        _bestNotes = _notes;
                    }
                }
            }

            if (_bestNotes != null)
            {
                if (_bestNotes.Type == NotesType.LongStart)
                {
                    _isPushing = true;
                }
                _bestNotes.Push();
                var _pos = Mathf.Abs(_bestNotes.transform.position.z - _statusManager.AdjustTouch);
                if (_pos < _statusManager.JudgePosition[(int)JudgeType.Perfect] *
                    _statusManager.MoveSpeed / Defaults.ConstMoveSpeed)
                {
                    ScoreManager.Judge(JudgeType.Perfect);
                }
                else if (_pos < _statusManager.JudgePosition[(int)JudgeType.Good] *
                    _statusManager.MoveSpeed / Defaults.ConstMoveSpeed)
                {
                    ScoreManager.Judge(JudgeType.Good);
                }
                else
                {
                    ScoreManager.Judge(JudgeType.Miss);
                }
            }
        }

        public void Down()
        {
            
        }

        public void Up()
        {
            _isPushing = false;
            foreach (var _long in _longNotes)
            {
                _long.Up();
            }
        }
    }
}

