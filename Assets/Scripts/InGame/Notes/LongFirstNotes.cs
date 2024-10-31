using InGame.Status;
using UnityEngine;
using UniRx;

namespace InGame
{
    public class LongFirstNotes: NotesCore
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
            Active = true;
            gameObject.SetActive(true);
        }

        public override void EndActivate()
        {
            
        }
    }
}