using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        public NoteEntity UserNoteCreate(long userId, Notes createNote);
        public List<NoteEntity> GetNote(long userId);
        public bool UpdateNote(long userId, long noteId, Notes note);
        public bool DeleteNote(long noteId, long userId);
        public NoteEntity PinnedNote(long noteId, long userId);
        public NoteEntity Archive(long userId, long noteId);

        public bool TrashNote(long noteId, long userId);
        public bool NoteColour(long noteId, string colour);
        public NoteEntity Image(long noteId, IFormFile img);

    }
}
