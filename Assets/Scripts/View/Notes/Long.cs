using Abstracts;
using Model;

namespace View.Notes
{
    public class Long: NotesCore
    {
        public override NotesType Type => NotesType.Long;

        public override void OnPush()
        {
            DeActivate();
        }

        public override void OnActivate()
        {
            
        }
    }
}