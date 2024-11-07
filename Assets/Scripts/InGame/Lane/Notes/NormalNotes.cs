using Status;
using UnityEngine;
using UniRx;

namespace InGame
{
    public class NormalNotes: NotesCore
    {
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
            gameObject.transform.position = FirstPosition;
            Active = true;
            gameObject.SetActive(true);
            StatusManager _status = new StatusManager();
            Disposable = UpdateNotes.Timer.Subscribe(x =>
            {
                if (Active)
                {
                    gameObject.transform.position -= new Vector3(0, 0, _status.MoveSpeed / UpdateNotes.FPS);
                }

                if (transform.position.z < -_status.JudgePosition[(int)JudgeType.Miss])
                {
                    Miss(Type);
                    Push();
                }
            }).AddTo(this);
        }

        public override void EndActivate()
        {
            
        }
    }
}