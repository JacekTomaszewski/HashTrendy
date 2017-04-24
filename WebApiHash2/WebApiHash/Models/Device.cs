using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiHash.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        public string DeviceUniqueId { get; set; }
        public virtual ICollection<Hashtag> Hashtags { get; set; }

    }
}