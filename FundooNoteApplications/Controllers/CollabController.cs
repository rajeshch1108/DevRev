using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : Controller
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundoocontext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public CollabController(ICollabBL collabBL, FundooContext fundoocontext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.fundoocontext = fundoocontext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;

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
        [HttpDelete("DeleteCollaborator")]
        public IActionResult DeleteCollab(long collabId)
        {
            var result = this.collabBL.DeleteCollab(collabId);
            if (result)
            {
                return this.Ok(new { Success = true, message = "Deleted Successfully" });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Unable to delete" });
            }
        }
        [Authorize]
        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllCollabUsingRedisCache()
        {
            var cacheKey = "CollabList";
            string serializedCollabList;
            var collabList = new List<CollabEntity>();
            var redisCollabList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabList != null)
            {
                serializedCollabList = Encoding.UTF8.GetString(redisCollabList);
                collabList = JsonConvert.DeserializeObject<List<CollabEntity>>(serializedCollabList);
            }
            else
            {
                collabList = await fundoocontext.CollabTable.ToListAsync();
                serializedCollabList = JsonConvert.SerializeObject(collabList);
                redisCollabList = Encoding.UTF8.GetBytes(serializedCollabList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabList, options);
            }
            return Ok(collabList);
        }
    }
}
