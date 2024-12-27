using Abstracts;
using Model;
using Zenject;

namespace View.Notes
{
    public class Normal: NotesCore
    {
        public override NotesType Type => NotesType.Normal;
        
        protected override void OnPush()
        {
            DeActivate();
        }

        public override void OnActivate()
        {
            
        }
    }
}