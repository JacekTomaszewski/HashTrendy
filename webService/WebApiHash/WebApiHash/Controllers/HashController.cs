﻿using System;
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
        public ActionResult Create(String imei)
        {
            Device device = new Device();
            ViewBag.imei = imei;
            device.DeviceUniqueId = imei;
            db.Devices.Add(device);
            db.SaveChanges();
            

            //db.Database.ExecuteSqlCommand("INSERT INTO Devices (DeviceUniqueId) VALUES ('" + imei + "') ON DUPLICATE KEY UPDATE 'DeviceUniqueId' = 'DeviceUniqueId'", DeviceId);
            //db.Database.ExecuteSqlCommand("UPDATE Devices SET DeviceUniqueId='"+imei+"' WHERE DeviceUniqueId='"+imei+"' IF @@ROWCOUNT=0 INSERT INTO Devices (DeviceUniqueId) VALUES('"+imei+"')",DeviceId);
            return View();
        }

        public ActionResult Delete(int id = 0)
        {
            Device DeviceId = db.Devices.Find(id);
            if (DeviceId == null)
            {
                return HttpNotFound();
            }
            return View(DeviceId);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            Device DeviceId = db.Devices.Find(id);
            db.Devices.Remove(DeviceId);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    
    }
}