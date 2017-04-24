using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiHash.Models
{
    public class Hashtag
    {
        public int HashtagId { get; set; }
        public string HashtagName { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}