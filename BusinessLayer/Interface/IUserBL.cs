using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {  
        public UserEntity UserRegistration(Registration registration);
        public string LoginUser(LoginModel loginModel);
        public string ForgetPassword(string emailId);

    }
}
