using System;
using Abstracts;
using JetBrains.Annotations;
using Model;

namespace View.Notes
{
    public class End: NotesCore
    {
        public override NotesType Type => NotesType.End;
        public override void OnActivate(LaneName laneName, float speed)
        {
            
        }

        protected override void OnPush()
        {
            DeActivate();
        }

        protected override void OnDeActivate()
        {
            
        }
    }
}