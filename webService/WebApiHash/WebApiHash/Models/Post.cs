using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiHash.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public DateTime Date { get; set; }

        public string Avatar { get; set; }

        public string Username { get; set; }

        public string Title { get; set; }

        public string ContentDescription { get; set; }

        public string ContentImageUrl { get; set; }

        public string DirectLinkToStatus { get; set; }
    }

    public class GooglePost : Post
    {
        public string PostSource = "GooglePlus";
    }
    public class TwitterPost : Post
    {
        public string PostSource = "Twitter";
    }
}