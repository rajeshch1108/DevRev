using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using System;
using System.Linq;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController  : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly FundooContext fundoocontext;

        public NoteController(INoteBL notesBL, FundooContext fundoocontext)
        {
            this.noteBL = notesBL;
            this.fundoocontext = fundoocontext;
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
                long userId = long.Parse(User.FindFirst("userId").Value.ToString());
                var result = this.noteBL.GetNote(userId);
                if (result != null)
                {
                    return this.Ok(new { sucess = true, message = "All notes are fetched and now you can read your notes", data = result });
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
    }
}
