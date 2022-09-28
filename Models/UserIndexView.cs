using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserIndexView
    {
        public User myProfile { get; set; }
        public int followersCount { get; set; }
        public int followingCount { get; set; }
        public IEnumerable<Fixture> fixtures { get; set; }
        public IEnumerable<User> suggestions { get; set; }
        public IEnumerable<Post> posts { get; set; }
    }
}