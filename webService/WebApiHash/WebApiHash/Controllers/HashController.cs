using Hammock.Serialization;
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public ActionResult TwitterTrends()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "O5YRKrovfS42vADDPv8NdC4ZS",
                    ConsumerSecret = "tDrCy3YypKhnIOBm0qgCipwGjoJVf7akHV6srkHnLHJm62WvMF",
                    AccessToken = "859793491941093376-kqRIYWY9bWyS10ATfqAVdwk1ZaxloEJ",
                    AccessTokenSecret = "hbOXipioFNcyOUyWbGdVAXvoVquETMl57AZUTcbMh3WRv"
                }
            };
            List<String> listTwitterStatus = new List<String>();
            TwitterContext twitterctx = new TwitterContext(auth);
            var trends = (from trend in twitterctx.Trends
                          where trend.Type == TrendType.Place
                                && trend.WoeID == 1
                                // POLAND 23424923
                                && trend.SearchUrl.Substring(28, 3).Equals("%23")
                          select trend).ToList();
            if (trends != null &&
                trends.Any() &&
                trends.First().Locations != null
                )
            {
                ViewData["Lokacja"] ="Trendy wyszukiwane dla: "+trends.First().Locations.First().Name;
                trends.ForEach(trnd => 
                    listTwitterStatus.Add("Name: " + trnd.Name+"   Created at: "+ trnd.CreatedAt+ "   SearchUrl: " + trnd.SearchUrl));
            }
            for (int i = 0; i < listTwitterStatus.Count; i++)
            {
                ViewData["MyList" + i] = listTwitterStatus[i].ToString();
                
            }
            return View(ViewData);
        }
        public ActionResult Twitterli()
        {
            List<TwitterStatus> listTwitterStatus = new List<TwitterStatus>();
            var service = new TwitterService("O5YRKrovfS42vADDPv8NdC4ZS", "tDrCy3YypKhnIOBm0qgCipwGjoJVf7akHV6srkHnLHJm62WvMF");
            service.AuthenticateWith("859793491941093376-kqRIYWY9bWyS10ATfqAVdwk1ZaxloEJ", "hbOXipioFNcyOUyWbGdVAXvoVquETMl57AZUTcbMh3WRv");
            var twitterSearchResult = service.Search(new SearchOptions { Q = "#CR7", Count = 100, Resulttype = TwitterSearchResultType.Recent });
            if (twitterSearchResult != null)

            {
                listTwitterStatus = ((List<TwitterStatus>)twitterSearchResult.Statuses);
            }
            for(int i=0; i<listTwitterStatus.Count; i++)
            { 
            ViewData["MyList" + i +0] = listTwitterStatus[i].User.ProfileImageUrl;
            ViewData["MyList" + i + 1] = listTwitterStatus[i].User.CreatedDate;
            ViewData["MyList"+ i + 2] = listTwitterStatus[i].User.Name;
            ViewData["MyList"+i+3] = listTwitterStatus[i].Text;
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
                TempData["Date"] = user.CreatedDate;
                TempData["Status"] = user;
            TempData["access"] = accessToken.Token;
            TempData["access secret"] = accessToken.TokenSecret;
            return View(TempData);

        }



        public ActionResult Create(String imei)
        {
            Device device = new Device();
            ViewBag.imei = imei;
            device.DeviceUniqueId = imei;
            db.Devices.Add(device);
            db.SaveChanges();
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