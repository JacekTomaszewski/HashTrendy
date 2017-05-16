using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiHash.Models
{
    public class Hashtag
    {
        public int HashtagId { get; set; }

        [Index("Hashtah", IsUnique = true)]
        [StringLength(450)]
        public string HashtagName { get; set; }

        [Index("IX_HashAndDevice", 2, IsUnique = true)]
        [StringLength(450)]
        public virtual ICollection<Device> Devices { get; set; }
    }
}