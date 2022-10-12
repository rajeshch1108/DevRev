using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
                noteEntity.userID = result.userId;

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
                var result = fundooContext.NoteTable.Where(note => note.userID == userId).ToList();
                return result;
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
                var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                if (result != null)
                {
                    result.Title = note.Title;
                    result.Description = note.Description;
                    result.Reminder = note.Reminder;
                    result.Colour = note.Colour;
                    result.Image = note.Image;
                    result.Created = note.Created;
                    result.Edited = note.Edited;

                    fundooContext.NoteTable.Update(result);
                    var update = fundooContext.SaveChanges();
                    if (update > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                    return false;
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
                var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.NoteTable.Remove(result);
                    this.fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
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
                var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();           
                if (result.Pinned == true)
                {
                    result.Pinned = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.Pinned = true;
                    fundooContext.SaveChanges();
                    return null;
                }
            }
            catch (System.Exception )
            {
                throw ;
            }
        }
        public NoteEntity Archive(long userId, long noteId)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                if (result.Archive == true)
                {
                    result.Archive = false;
                    fundooContext.SaveChanges();
                    return null;
                }
                else
                {
                    result.Archive = true;
                    fundooContext.SaveChanges();
                    return result;
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool TrashNote(long noteId,long userId)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                if (result.Trash == true)
                {
                    result.Pinned = false;
                    fundooContext.SaveChanges();
                    return false;
                }
                else
                {
                    result.Trash = true;
                    fundooContext.SaveChanges();
                    return true;
                }
            }
            catch (System.Exception )
            {
                throw ;
            }

        }
         
           public bool NoteColour(long noteId, string colour)
              {
                try
              {
               // var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                var result = fundooContext.NoteTable.Where(note => note.NoteId == noteId).FirstOrDefault();
                if (result.Colour != colour)
                {
                    result.Colour = colour;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public NoteEntity Image(long noteId, IFormFile file)
        {
            try
            {
                var result = this.fundooContext.NoteTable.Where(note => note.NoteId == noteId).FirstOrDefault();
                if (result != null)
                {
                    Account account = new Account(
                        "dtncu62va",
                        "685349467117391",
                        "HDuB4fqd5bO28Aq5-cffWz5bcZ8");

                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                    };
                    var upload = cloudinary.Upload(uploadParams);
                    string filePath = upload.Url.ToString();
                    result.Image = filePath;
                    fundooContext.SaveChanges();
                    return result;
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
    }
}
