using CommonLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext fundooContext;
        public NoteRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public NoteEntity UserNoteCreate(long userId, Notes createNote)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();
                var result = fundooContext.userTable.Where(user => user.userId == userId).FirstOrDefault();
                noteEntity.Title = createNote.Title;
                noteEntity.Description = createNote.Description;
                noteEntity.Reminder = createNote.Reminder;
                noteEntity.Colour = createNote.Colour;
                noteEntity.Image = createNote.Image;
                noteEntity.Archive = createNote.Archive;
                noteEntity.Pinned = createNote.Pinned;
                noteEntity.Trash = createNote.Trash;
                noteEntity.Created = createNote.Created;
                noteEntity.Edited = createNote.Edited;
                noteEntity.userId = result.userId;

                fundooContext.NoteTable.Add(noteEntity);
                int update = fundooContext.SaveChanges();
                if (update > 0)
                {
                    return noteEntity;
                }
                else
                {
                    return null;
                }
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
                var result = fundooContext.NoteTable.Where(note => note.userId == userId).ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
