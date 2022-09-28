using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserFixtureView
    {
        public User myProfile { get; set; }
        public IEnumerable<Fixture> fixtures { get; set; }
    }
}