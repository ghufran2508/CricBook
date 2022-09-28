using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Fixture
    {
        public int ID { get; set; }

        //[ForeignKey("TeamId")]
        public int? HomeTeamId { get; set; }
        //[ForeignKey("Team")]
        public int AwayTeamId { get; set; }
        public DateTime time { get; set; }

        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }

        public Fixture()
        {

        }
        public Fixture(int? homeTeamId, int awayTeamId, DateTime time)
        {
            HomeTeamId = homeTeamId;
            AwayTeamId = awayTeamId;
            this.time = time;
        }
    }
}