using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class UserController : ControllerBase
        {
            private readonly IUserBL userBL;
            public UserController(IUserBL userBL)
            {
                this.userBL = userBL;
            }
            [HttpPost("Register")]//Custom route
            public IActionResult UserRegistration(Registration Registration)
            {
                try
                {
                    var result = userBL.UserRegistration(Registration);
                    if (result != null)
                    {
                        return this.Ok(new { success = true, message = "UserRegistration Successfull", data = result });
                    }
                    else
                    {
                        return this.BadRequest(new { success = true, message = "UserRegistration UnSuccessfull" });
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        [HttpPost("login")]
        public IActionResult LoginUser(LoginModel loginModel)
        {
            try
            {
                var userdata = userBL.LoginUser(loginModel);
                if (userdata != null)
                {
                    return this.Ok(new { success = true, message = userdata });
                }
                return this.BadRequest(new { success = false, message = $"Email And PassWord Is Invalid" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string emailId)
        {
            try
            {
                var result = userBL.ForgetPassword(emailId);
                if (result != null)
                {
                    return this.Ok(new { Sucess = true, message = "email sends successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "email doesnot send successfully" });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        

    }
    }

