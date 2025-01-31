using System;
using Abstracts;
using Interfaces;
using JetBrains.Annotations;
using Model;

namespace View.Notes
{
    public class End: NotesCore,IEndNotes
    {
        public override NotesType Type => NotesType.End;
        public override void OnActivate(LaneName laneName, float speed)
        {
            
        }

        protected override void OnPush()
        {
            DeActivate();
        }
    }
}