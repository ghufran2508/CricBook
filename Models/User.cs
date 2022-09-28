using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }    
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePic { get; set; } 
        public DateTime dateCreated { get; set; }

        public ICollection<FriendRequest> SenderRequests { get; set; }
        public ICollection<FriendRequest> RecieverRequests { get; set; }
        public ICollection<Friends> SenderFriend1 { get; set; }
        public ICollection<Friends> RecieverFriend2 { get; set; }

        public User()
        {

        }
        public User(string name, string email, string password, string profilePic)
        {
            Name = name;
            Email = email;
            Password = password;
            ProfilePic = profilePic;
        }

    }
}