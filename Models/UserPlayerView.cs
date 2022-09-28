using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserPlayerView
    {
        public User myProfile { get; set; }
        public Team team { get; set; }
        public IEnumerable<Player> players { get; set; }
    }
}