using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        public NoteEntity UserNoteCreate(long userId, Notes createNote);
        public List<NoteEntity> GetNote(long userId);
        public bool UpdateNote(long userId, long noteId, Notes note);
        public bool DeleteNote( long noteId);
    }
}
