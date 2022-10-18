using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FundooNoteApplications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class UserController : ControllerBase
        {
            private readonly IUserBL userBL;
            private readonly ILogger<UserController> _logger;
        public UserController(IUserBL userBL, ILogger<UserController> _logger)
            {
                this.userBL = userBL;
                this._logger = _logger;
            }
            [HttpPost("Register")]//Custom route
            public IActionResult UserRegistration(Registration Registration)
            {
                try
                {
                    var result = userBL.UserRegistration(Registration);
                    if (result != null)
                    {
                    _logger.LogInformation("UserRegistration Successfull");
                    return this.Ok(new { success = true, message = "UserRegistration Successfull", data = result });
                    }
                    else
                    {
                    _logger.LogInformation("UserRegistration UnSuccessfull");

                    return this.BadRequest(new { success = true, message = "UserRegistration UnSuccessfull" });
                    }
                }
                catch (Exception  ex)
                {
                _logger.LogError(ex.Message);
                    throw ex ;
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
                    _logger.LogInformation("Email And PassWord Is Valid ");
                    return this.Ok(new { success = true, message = "Email And PassWord Is Valid" });
                }
                else
                {
                    _logger.LogInformation("Email And PassWord Is Invalid");
                    return this.BadRequest(new { success = false, message = $"Email And PassWord Is Invalid" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                    _logger.LogInformation("Email sends successfully ");
                    return this.Ok(new { Sucess = true, message = "Email sends successfully" });
                }
                else
                {
                    _logger.LogInformation("Email doesnot sends successfully ");
                    return this.BadRequest(new { Success = false, message = "Email doesnot send successfully" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        
        [HttpPut]
        [Route("ResetPassword")]
        public ActionResult ResetPassword(string Password, string ConfirmPassword)
        {
            try
            {
                var Email = User.FindFirst(ClaimTypes.Email).Value.ToString();

                if (userBL.ResetPassword(Password, ConfirmPassword))
                {
                    _logger.LogInformation("Reset Password is Succesfull ");
                    return Ok(new { success = true, message = "Reset Password is Succesfull" });
                }
                else
                {
                    _logger.LogInformation("Reset Password Link Could Not Be Sent ");
                    return BadRequest(new { success = false, message = "Reset Password Link Could Not Be Sent" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

    }
    }

