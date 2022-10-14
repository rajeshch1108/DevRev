using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using RepositoryLayer.Context;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labeBL;
        private readonly FundooContext fundoocontext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;


        public LabelController(ILabelBL labelBL, FundooContext fundoocontext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.labeBL = labelBL;
            this.fundoocontext = fundoocontext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }

        [Authorize]
        [HttpPost("CreateLabel")]

        public IActionResult CreateLabel(long noteId, string LabelName)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = labeBL.CreateLabel(userId, noteId, LabelName);
            if (userdata != null)
                return this.Ok(new { success = true, message = "Label created Successfully", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Not able to Label note" });
        }

        [Authorize]
        [HttpPatch("UpdateLabel")]

        public IActionResult UpdateLabel(long noteId, long LabelID, string LabelName)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
            var userdata = labeBL.UpdateLabel(userId, noteId, LabelID, LabelName);
            if (userdata != null)
                return this.Ok(new { success = true, message = "Label Updated Successfully", data = userdata });
            else
                return this.BadRequest(new { success = false, message = "Not able to Update Label" });
        }
        [HttpGet("GetAllLabel")]
        public IActionResult GetAllLabel()
        {
            try
            {
                long userId = long.Parse(User.FindFirst("userId").Value.ToString());
                var result = this.labeBL.GetLabel(userId);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Label Fetched Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Unable to Fetch Label" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete("DeleteLabel")]
        public IActionResult DeleteLabel(string labelName)
        {
            try
            {
                var result = this.labeBL.DeleteLabel(labelName);
                if (result)
                {
                    return this.Ok(new { success = true, message = "Label Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Unable to Delete Label note" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
            var cacheKey = "LabelList";
            string serializedLabelList;
            var labelList = new List<LabelEntity>();
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                labelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                labelList = await fundoocontext.LabelTable.ToListAsync();
                serializedLabelList = JsonConvert.SerializeObject(labelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }
            return Ok(labelList);
        }
    
    }
}
