using CommonLayer.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        public IConfiguration configuration { get; }

        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }
        public UserEntity UserRegistration(Registration registration)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = registration.FirstName;
                userEntity.LastName = registration.LastName;
                userEntity.Email = registration.Email;
                userEntity.Password = Encrypt(registration .Password);
                fundooContext.userTable.Add(userEntity);
                int result = fundooContext.SaveChanges();
                if (result > 0)
                {
                    return userEntity;
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
        private string GetJWTToken(string email, long userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(configuration["key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("userID",userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string LoginUser(LoginModel loginModel)
        {
            try
            {
                var result = fundooContext.userTable.Where(u => u.Email == loginModel.Email && u.Password == loginModel.Password).FirstOrDefault();
                if (result != null)
                {
                    return GetJWTToken(loginModel.Email, result.userId);
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string ForgetPassword(string emailId)
        {
            try
            {
                var emailCheck = fundooContext.userTable.FirstOrDefault(e => e.Email == emailId);
                if (emailCheck != null)
                {
                    var token = GetJWTToken(emailCheck.Email, emailCheck.userId);
                    MSMQModel mSMQModel = new MSMQModel();
                    mSMQModel.sendData2Queue(token);
                    return token.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool ResetPassword(string Password, string ConfirmPassword)
        {
            try
            {
                if (Password.Equals(ConfirmPassword))
                {
                    var emailCheck = fundooContext.userTable.FirstOrDefault();
                    emailCheck.Password = Password;
                    fundooContext.SaveChanges();
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
        public string Encrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += "";
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }
        public string Decrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return "";
            var base64EncoddeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncoddeBytes);
            result = result.Substring(0, result.Length );
            return result;  
        }
    }
}

    
   

    
