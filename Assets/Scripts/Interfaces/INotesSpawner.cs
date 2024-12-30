using Model;
using Abstracts;
using System.Collections.Generic;

namespace Interfaces
{
    public interface INotesSpawner
    {
        public List<INotes> NotesList { get; }
        public INotes Spawn(LaneName lane, NotesType type,float speed);
    }
}