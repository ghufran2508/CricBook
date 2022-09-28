using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public DateTime dateUpload  { get; set; }
        public Post() { }
        public Post(int userID, string description, string media, DateTime dateUpload)
        {
            UserID = userID;
            Description = description;
            Media = media;
            this.dateUpload = dateUpload;
        }

        public virtual User User { get; set; } 
    }
}