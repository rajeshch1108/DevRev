using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public bool DeleteNote(long noteId, long userId)
        {
            try
            {
                return this.noteRL.DeleteNote(noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity PinnedNote(long noteId, long userId)
        {
            try
            {
                return this.noteRL.PinnedNote(noteId, userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public NoteEntity Archive(long userId, long noteId)
        {
            try
            {
                return noteRL.Archive(userId, noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool TrashNote(long noteId, long userId)
        {
            try
            {
                return this.noteRL.TrashNote(noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool NoteColour(long noteId, string colour)
        {
            try
            {
                return this.noteRL.NoteColour(noteId, colour);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity Image(long noteId, IFormFile img)
        {
            try
            {
                return this.noteRL.Image(noteId, img);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
