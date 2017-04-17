using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApiHash.Context;
using WebApiHash.Models;

namespace WebApiHash.Controllers
{
    public class HashController : Controller
    {
        HashContext db = new HashContext();

        public object DeviceId { get; private set; }

        public ActionResult Index()
        {

            return View(db.Devices.ToList());
        }
        public ActionResult Create()
        {
            String UniqueIdOfDeviceToCreate = "35444444440000400";
            ViewBag.UniqueIdOfDeviceToCreate = UniqueIdOfDeviceToCreate;
            db.Database.ExecuteSqlCommand("INSERT INTO Devices (DeviceUniqueId) VALUES (" + UniqueIdOfDeviceToCreate +")", DeviceId);
            return View();
        }

        public ActionResult DeleteImei()
        {
            int IdOfDeviceToDelete = 6;
            db.Database.ExecuteSqlCommand("Delete from Devices where DeviceId = "+IdOfDeviceToDelete, DeviceId);
            return View(db.Devices.ToList());
        }

    }
}