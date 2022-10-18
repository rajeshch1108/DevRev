using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity UserRegistration(Registration registration);
        public string LoginUser(LoginModel loginModel);
        public string ForgetPassword(string emailId);
        public bool ResetPassword( string password, string confirmPassword);
        public string Encrypt(string password);
         public string Decrypt(string base64EncodeData);
    }
}
