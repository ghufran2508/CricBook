using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string email { get; set; }
        public string Password { get; set; }

        public string ProfilePic { get; set; }
        public bool isAdmin { get; set; }

        public LoginUser(int id, string userName, string email, string password, string profilePic, bool isAdmin)
        {
            Id = id;
            UserName = userName;
            this.email = email;
            Password = password;
            ProfilePic = profilePic;
            this.isAdmin = isAdmin;
        }       
    }
}