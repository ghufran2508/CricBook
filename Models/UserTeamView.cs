using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserTeamView
    {
        public User myProfile
        {
            get; set;
        }

        public IEnumerable<Team> teams { get; set; }
    }
}