using Abstracts;
using Model;

namespace View.Notes
{
    public class Normal: NotesCore
    {
        public override NotesType Type => NotesType.Normal;
        
        public override void OnPush()
        {
            DeActivate();
        }

        public override void OnActivate()
        {
            
        }
    }
}