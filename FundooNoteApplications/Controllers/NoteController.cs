﻿using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController  : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly FundooContext fundoocontext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public NoteController(INoteBL notesBL, FundooContext fundoocontext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.noteBL = notesBL;
            this.fundoocontext = fundoocontext;
            this.memoryCache = memoryCache; 
            this.distributedCache = distributedCache;   
        }
        
        [HttpPost]
        [Route("CreateNotes")]
        public ActionResult UserNoteCreate(Notes createNote)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = noteBL.UserNoteCreate(userId, createNote);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Notes Created Successfully", data = result });
                }
                else
                {
                    return NotFound(new { success = false, message = "Notes is not created " });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpGet("GetNote")]
        public IActionResult GetNote()
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = this.noteBL.GetNote(userId);
                if (result != null)
                {
                    return this.Ok(new { sucess = true, message = "All notes are created and now you can read your notes", data = result });
                }
                else
                {
                    return this.BadRequest(new { sucess = false, message = "Unable to show the notes." });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPut("UpdateNote")]
        public IActionResult UpdateNote(long noteId, Notes note)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                var result = noteBL.UpdateNote(userId, noteId, note);
                if (result)
                {
                    return this.Ok(new { success = true, message = "Note updated Successfully" });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Unable to update note" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNotesofuser(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result =noteBL.DeleteNote(noteId, userId);
                if (result)
                {
                    return this.Ok(new { success = true, message = "note Deleted Successfully" });

                }
                else
                {
                    return this.BadRequest(new { success = false, message = "unable to Delete Note" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("PinnedNote")]
        public IActionResult PinnedNote(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = this.noteBL.PinnedNote(noteId, userId);
                if (result != null) 
                
                {
                    return this.Ok(new { success = true, message = "Note is pinned successfully" ,Response = result});
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Note is un pinned" });
                }
            }
            catch (System.Exception )
            {
                throw  ;
            }
        }
        [HttpPut]
        [Route("Archive")]
        public ActionResult ArchiveNote(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = this.noteBL.Archive(userId, noteId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Archived Note Successfully", data = result });
                }
                else if (result == null)
                {
                    return Ok(new { success = true, message = "Archived Note UnSuccessfull", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Could not perform Archive Operation" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        [HttpPut("Trash")]
        public IActionResult Trash(long noteId)
        {
            try

            {
               long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
             var result = this.noteBL.TrashNote(noteId,userId);
               if (result == true)

               {
                  return this.Ok(new { success = true, message = "Note is Trash successfully." });
               }
               else
              {
                   return this.BadRequest(new { success = false, message = "Note is not found" });
              }
              }
              catch (Exception ex)
            {
               throw ex;
          }
         }
        [HttpPut("NoteColour")]
        public IActionResult NoteColour(long noteId, string colour)
        {
            try
            {
                var result = this.noteBL.NoteColour(noteId, colour);
                if (result)
                {
                    return this.Ok(new { success = true, message = "colour  changed successfully." });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = " Note colour is not found." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UploadImage")]
        public IActionResult Image(long noteId, IFormFile img)
        {
            try
            {
                var result = this.noteBL.Image(noteId, img);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Image uploaded sucessfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Failed to upload Image" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize]
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNoteUsingRedisCache()
        {
            var cacheKey = "NoteList";
            string serializedNoteList;
            var notesList = new List<NoteEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNoteList = Encoding.UTF8.GetString(redisNotesList);
                notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNoteList);
            }
            else
            {
                notesList = await fundoocontext.NoteTable.ToListAsync();
                serializedNoteList = JsonConvert.SerializeObject(notesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNoteList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(notesList);
        }

    }
}
