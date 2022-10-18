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
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<CollabController> _logger;

        public CollabController(ICollabBL collabBL, FundooContext fundoocontext, IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<CollabController> _logger)
        {
            this.collabBL = collabBL;
            this.fundoocontext = fundoocontext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this._logger = _logger;

        }
    [Authorize]
        [HttpPost("AddCollab")]
        public IActionResult Collab(long noteId, string receiver_email)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = collabBL.AddCollab(userId, noteId, receiver_email);
                if (userdata != null)
                {
                    _logger.LogInformation("Collaborated Successfull");
                    return this.Ok(new { success = true, message = "Collaborated Successfull", data = userdata });
                }
                else
                {
                    _logger.LogInformation("Unable to collaborate note");
                    return this.BadRequest(new { success = false, message = "Unable to collaborate note" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        [Authorize]
        [HttpGet("GetCollab")]
        public IActionResult GetCollab()
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var userdata = collabBL.GetCollab(userId);
                if (userdata != null)
                {
                    _logger.LogInformation("Fetch Successfull");
                    return this.Ok(new { success = true, message = "Fetch Successfull", data = userdata });
                }
                else
                {
                    _logger.LogInformation("Fetch operation failed");
                    return this.BadRequest(new { success = false, message = "Fetch operation failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        [HttpDelete("DeleteCollaborator")]
        public IActionResult DeleteCollab(long collabId)
        {
            try
            {
                var result = this.collabBL.DeleteCollab(collabId);
                if (result)
                {
                    _logger.LogInformation("Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Deleted Successfully" });
                }
                else
                {
                    _logger.LogInformation("Unable to delete");
                    return this.BadRequest(new { success = false, message = "Unable to delete" });
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
        public async Task<IActionResult> GetAllCollabUsingRedisCache()
        {
            var cacheKey = "CollabList";
            string serializedCollabList;
            var collabList = new List<CollabEntity>();
            var redisCollabList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabList != null)
            {
                _logger.LogInformation("Retrive Collab Successfull");
                serializedCollabList = Encoding.UTF8.GetString(redisCollabList);
                collabList = JsonConvert.DeserializeObject<List<CollabEntity>>(serializedCollabList);
            }
            else
            {
                _logger.LogInformation("Retrive Collab unSuccessfull");
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
