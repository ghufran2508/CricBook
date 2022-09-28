using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string type { get; set; }
        public string Logo { get; set; }
        public int playerCount { get; set; }

        public virtual ICollection<Fixture> HomeFixtures { get; set; }
        public virtual ICollection<Fixture> AwayFixtures { get; set; }

        public virtual ICollection<Player> players { get; set; }
        public Team() { }
        public Team(string teamName, string type, string logo, int playerCount)
        {
            TeamName = teamName;
            this.type = type;
            Logo = logo;
            this.playerCount = playerCount;
        }
    }
}