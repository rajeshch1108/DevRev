using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NoteController> _logger;

        public NoteController(INoteBL notesBL, FundooContext fundoocontext, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<NoteController> _logger)
        {
            this.noteBL = notesBL;
            this.fundoocontext = fundoocontext;
            this.memoryCache = memoryCache; 
            this.distributedCache = distributedCache;
            this._logger = _logger;
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
                    _logger.LogInformation("Notes Created Successfully");
                    return Ok(new { success = true, message = "Notes Created Successfully", data = result });
                }
                else
                {
                    _logger.LogInformation("Notes is not created");
                    return NotFound(new { success = false, message = "Notes is not created " });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
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
                    _logger.LogInformation("All notes are created and now you can read your notes");
                    return this.Ok(new { sucess = true, message = "All notes are created and now you can read your notes", data = result });
                }
                else
                {
                    _logger.LogInformation("Unable to show the notes");
                    return this.BadRequest(new { sucess = false, message = "Unable to show the notes" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                    _logger.LogInformation("Note updated Successfully");
                    return this.Ok(new { success = true, message = "Note updated Successfully" });
                }
                else
                {
                    _logger.LogInformation("Unable to update notel");
                    return this.BadRequest(new { success = false, message = "Unable to update note" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                    _logger.LogInformation("note Deleted Successfully");
                    return this.Ok(new { success = true, message = "note Deleted Successfully" });

                }
                else
                {
                    _logger.LogInformation("unable to Delete Note");
                    return this.BadRequest(new { success = false, message = "unable to Delete Note" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
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
                    _logger.LogInformation("Note is pinned successfully");
                    return this.Ok(new { success = true, message = "Note is pinned successfully" ,Response = result});
                }
                else
                {
                    _logger.LogInformation("Note is un pinned");
                    return this.BadRequest(new { success = false, message = "Note is un pinned" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex ;
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
                    _logger.LogInformation("Archived Note Successfully");
                    return Ok(new { success = true, message = "Archived Note Successfully", data = result });
                }
                else if (result == null)
                {
                    _logger.LogInformation("Archived Note UnSuccessfull");
                    return Ok(new { success = true, message = "Archived Note UnSuccessfull", data = result });
                }
                else
                {
                    _logger.LogInformation("Could not perform Archive Operation");
                    return BadRequest(new { success = false, message = "Could not perform Archive Operation" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
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
                    _logger.LogInformation("Note is Trash successfully");
                    return this.Ok(new { success = true, message = "Note is Trash successfully." });
               }
               else
              {
                    _logger.LogInformation("Note is not found");
                    return this.BadRequest(new { success = false, message = "Note is not found" });
              }
              }
              catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                    _logger.LogInformation("colour  changed successfully");
                    return this.Ok(new { success = true, message = "colour  changed successfully" });
                }
                else
                {
                    _logger.LogInformation("Note colour is not found");
                    return this.BadRequest(new { success = false, message = " Note colour is not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                    _logger.LogInformation("Image uploaded sucessfully");
                    return this.Ok(new { success = true, message = "Image uploaded sucessfully", Response = result });
                }
                else
                {
                    _logger.LogInformation("Failed to upload Image");
                    return this.BadRequest(new { success = false, message = "Failed to upload Image" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        [Authorize]
        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNoteUsingRedisCache()
        {
            var cacheKey = "NoteList";
            string serializedNoteList;
            var notesList = new List<NoteEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                _logger.LogInformation("Retrive note Successfull");
                serializedNoteList = Encoding.UTF8.GetString(redisNotesList);
                notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNoteList);
            }
            else
            {
                _logger.LogInformation("Retrive note unSuccessfull");
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
