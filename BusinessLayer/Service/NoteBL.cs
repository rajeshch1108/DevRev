using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public NoteEntity UserNoteCreate(long userId, Notes createNote)
        {
            try
            {
                return this.noteRL.UserNoteCreate(userId, createNote);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<NoteEntity> GetNote(long userId)
        {
            try
            {
                return this.noteRL.GetNote(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool UpdateNote(long userId, long noteId, Notes note)
        {
            try
            {
                return this.noteRL.UpdateNote(userId, noteId, note);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool DeleteNote(long noteId)
        {
            try
            {
                return this.noteRL.DeleteNote(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
