using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public DateTime dateUpload { get; set; }
        public News() { }
        public News(string description, string media, DateTime dateUpload)
        {
            Description = description;
            Media = media;
            this.dateUpload = dateUpload;
        }
    }
}