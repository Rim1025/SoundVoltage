using Abstracts;
using Model;

namespace View.Notes
{
    public class Long: NotesCore
    {
        public override NotesType Type => NotesType.Long;

        protected override void OnPush()
        {
            DeActivate();
        }

        public override void OnActivate()
        {
            
        }
    }
}