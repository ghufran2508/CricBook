using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MyDatabase: DbContext
    {
        public MyDatabase() : base("name=MyConnection")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<MyDatabase>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MyDatabase>());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> players { get; set; }
        public DbSet<Fixture> fixtures { get; set; }
        public DbSet<News> news { get; set; }
    }
}