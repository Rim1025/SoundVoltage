using Status;
using UnityEngine;
using UniRx;

namespace InGame
{
    public class LongMiddleNotes: NotesCore
    {
        private LongNotes _longNotes;
        private float _direction;
        public override void Push()
        {
            if (Active)
            {
                Active = false;
                gameObject.SetActive(false);
                Dispose();
            }
        }

        public override void Down()
        {
            
        }
        public override void Up()
        {
            
        }
        public override void Activate()
        {
            StatusManager _status = new StatusManager();
            Active = true;
            gameObject.SetActive(true);
            Disposable = UpdateNotes.Timer.Subscribe(x =>
            {
                if (gameObject.transform.position.z + _direction / 2 < _status.AdjustTouch)
                {
                    Push();
                    _longNotes.Down();
                }
            }).AddTo(this);
        }

        public void InitMiddle(LongNotes longNotes,float direction)
        {
            _longNotes = longNotes;
            _direction = direction;
        }

        public override void EndActivate()
        {
            
        }
    }
}