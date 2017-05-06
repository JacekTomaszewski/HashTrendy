using Hammock.Serialization;
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TweetSharp;
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

        public ActionResult Twitterli()
        {
            List<TwitterStatus> listTwitterStatus = new List<TwitterStatus>();
            var service = new TwitterService("O5YRKrovfS42vADDPv8NdC4ZS", "tDrCy3YypKhnIOBm0qgCipwGjoJVf7akHV6srkHnLHJm62WvMF");
            service.AuthenticateWith("859793491941093376-kqRIYWY9bWyS10ATfqAVdwk1ZaxloEJ", "hbOXipioFNcyOUyWbGdVAXvoVquETMl57AZUTcbMh3WRv");
            var twitterSearchResult = service.Search(new SearchOptions { Q = "#cr7", Count = 20, Resulttype = TwitterSearchResultType.Recent });

            if (twitterSearchResult != null)
            {
                listTwitterStatus = ((List<TwitterStatus>)twitterSearchResult.Statuses);
            }
            for(int i=0; i<listTwitterStatus.Count; i++)
            { 
            ViewData["MyList" + i +0] = listTwitterStatus[i].User.ProfileImageUrl;
            ViewData["MyList"+ i + 1] = listTwitterStatus[i].User.Name;
            ViewData["MyList"+i+2] = listTwitterStatus[i].Text;
            }
            return View(ViewData);
        }
        public ActionResult TwitterAuth()
        {

            string Key = "O5YRKrovfS42vADDPv8NdC4ZS";
            string Secret = "tDrCy3YypKhnIOBm0qgCipwGjoJVf7akHV6srkHnLHJm62WvMF";

            TwitterService service = new TwitterService(Key, Secret);

            OAuthRequestToken requestToken = service.GetRequestToken("http://localhost:51577/Hash/TwitterCallback");
            
            Uri uri = service.GetAuthenticationUrl(requestToken);

            return Redirect(uri.ToString());
        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
        {
            var requestToken = new OAuthRequestToken { Token = oauth_token };

            string Key = "O5YRKrovfS42vADDPv8NdC4ZS";
            string Secret = "tDrCy3YypKhnIOBm0qgCipwGjoJVf7akHV6srkHnLHJm62WvMF";

           
                TwitterService service = new TwitterService(Key, Secret);

                OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);

                service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);

                VerifyCredentialsOptions option = new VerifyCredentialsOptions();

                TwitterUser user = service.VerifyCredentials(option);
                TempData["Name"] = user.Name;
                TempData["Userpic"] = user.ProfileImageUrl;
                TempData["Status"] = user;
            TempData["access"] = accessToken.Token;
            TempData["access secret"] = accessToken.TokenSecret;
            return View(TempData);

        }



        public ActionResult Create(String imei)
        {
            ViewBag.imei = imei;
            //db.Database.ExecuteSqlCommand("INSERT INTO Devices (DeviceUniqueId) VALUES ('" + imei + "') ON DUPLICATE KEY UPDATE 'DeviceUniqueId' = 'DeviceUniqueId'", DeviceId);
            db.Database.ExecuteSqlCommand("UPDATE Devices SET DeviceUniqueId='" + imei + "' WHERE DeviceUniqueId='" + imei + "' IF @@ROWCOUNT=0 INSERT INTO Devices (DeviceUniqueId) VALUES('" + imei + "')", DeviceId);
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