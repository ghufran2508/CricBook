using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string playerName { get; set; }
        public int TeamID { get; set; }

        public virtual Team Team { get; set; }

        public Player() { }
        public Player(string playerName, int teamID)
        {
            this.playerName = playerName;
            TeamID = teamID;
        }
    }
}