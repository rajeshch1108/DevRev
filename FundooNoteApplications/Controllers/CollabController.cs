using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : Controller
    {
        private readonly ICollabBL collabBL;

        public CollabController(ICollabBL collabBL)
        {
            this.collabBL = collabBL;
        }
        [Authorize]
        [HttpPost("AddCollab")]
        public IActionResult Collab(long noteId, string receiver_email)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = collabBL.AddCollab(userId, noteId, receiver_email);
            if (userdata != null)
                return this.Ok(new { success = true, message = "Collaborated Successfull", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Not able to collaborate note" });
        }
        [Authorize]
        [HttpGet("GetCollab")]
        public IActionResult GetCollab()
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = collabBL.GetCollab(userId);
            if (userdata != null)
                return this.Ok(new { success = true, message = "Fetch Successfull", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Fetch operation failed" });

        }

    }
}
