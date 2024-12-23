using Model;
using Abstracts;
using System.Collections.Generic;

namespace Interfaces
{
    public interface INotesSpawner
    {
        public List<NotesCore> NotesList { get; }
        public NotesCore Spawn(LaneName lane, NotesType type);
    }
}