using Abstracts;
using Model;

namespace View.Notes
{
    public class Big: NotesCore
    {
        public override NotesType Type => NotesType.Big;

        public override void OnPush()
        {
            DeActivate();
        }

        public override void OnActivate()
        {
            
        }
    }
}