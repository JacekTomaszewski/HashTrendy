using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApiHash.Models;

namespace WebApiHash.Context
{
    public class HashContext: DbContext
    {
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}