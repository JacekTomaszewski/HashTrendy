using LinqToTwitter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
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
        public ActionResult GooglePlusJSONencode()
        {
            //string result;
            //string GPquery = "%23trump";
            //string requestString = "https://www.googleapis.com/plus/v1/activities?" + GPquery + "&key=AIzaSyBZJabrdIgDO8rsZ-GMvi_ZTrFsJCHfpwA";
            //WebRequest objWebRequest = WebRequest.Create(requestString);
            //WebResponse objWebResponse = objWebRequest.GetResponse();
            //Stream objWebStream = objWebResponse.GetResponseStream();
            //using (StreamReader objStreamReader = new StreamReader(objWebStream))
            //{
            //    result = objStreamReader.ReadToEnd();
            //}
            //jsonzgoogla j = new jsonzgoogla();
            //string x = j.json;


            jsonzgoogla jsonZgoogla = new jsonzgoogla();

            
                GooglePlusPost post = JsonConvert.DeserializeObject<GooglePlusPost>(jsonZgoogla.json);

            GooglePost googlePost = new GooglePost();
            Hashtag hashtag = new Hashtag();
            ViewData["rozmiar"] = post.items.Count;
            IEnumerable<String> tags;
            for (int i = 0; i < post.items.Count; i++)
            {
                ViewData["Avatar" + i] = post.items[i].actor.image.url;
                googlePost.Avatar = post.items[i].actor.image.url;
                 ViewData["Date" + i] = post.items[i].published;
                googlePost.Date= System.DateTime.Parse(post.items[i].published);
            ViewData["Author" + i] = post.items[i].actor.displayName;
                googlePost.Username = post.items[i].actor.displayName;
                googlePost.DirectLinkToStatus = post.items[i].url;                                 
                tags = Regex.Split(post.items[i].@object.attachments[0].content, @"\s+").Where(b => b.StartsWith("#"));
                for (int x = 0; x < tags.Count(); x++)
                { 
                    hashtag.HashtagName = tags.ElementAt(x);
                    db.Hashtags.Add(hashtag);
                    db.SaveChanges();
                    //POMOCY Z FOREIGN KEY !!!
                }
                if (!post.items[i].@object.attachments[0].content.Contains(".png"))
                {
                    if (!post.items[i].@object.attachments[0].content.Contains(".jpg"))
                    {
                        ViewData["Content" + i] = post.items[i].title+" "+ post.items[i].@object.attachments[0].content;
                        googlePost.ContentDescription= post.items[i].@object.attachments[0].content;
                    }
                }
                if (post.items[i].@object.attachments[0].image.url != null)
                {
                    ViewData["Image" + i] = post.items[i].@object.attachments[0].image.url;
                    googlePost.ContentImageUrl = post.items[i].@object.attachments[0].image.url;
                }
                db.Posts.Add(googlePost);                
                db.SaveChanges();
            }
            

            return View(ViewData);
        }
        public ActionResult Twitterli(string hashtagname)
        {
            //localhost:50707/hash/twitterli?hashtagname=%23nazwahashtaga
            Hashtag hashtag = new Hashtag();
            IEnumerable<string> tags;
            TwitterPost twitterPost = new TwitterPost();
            List<TwitterStatus> listTwitterStatus = new List<TwitterStatus>();
            var service = new TwitterService("O5YRKrovfS42vADDPv8NdC4ZS", "tDrCy3YypKhnIOBm0qgCipwGjoJVf7akHV6srkHnLHJm62WvMF");
            service.AuthenticateWith("859793491941093376-kqRIYWY9bWyS10ATfqAVdwk1ZaxloEJ", "hbOXipioFNcyOUyWbGdVAXvoVquETMl57AZUTcbMh3WRv");
            var twitterSearchResult = service.Search(new SearchOptions { Q = hashtagname, Count = 5, Resulttype = TwitterSearchResultType.Recent });
            if (twitterSearchResult != null)

            {
                listTwitterStatus = ((List<TwitterStatus>)twitterSearchResult.Statuses);
            }
            for(int i=0; i<listTwitterStatus.Count; i++)
            {
            ViewData["Avatar" + i +0] = listTwitterStatus[i].User.ProfileImageUrl;
                twitterPost.Avatar = listTwitterStatus[i].User.ProfileImageUrl;
                ViewData["Date" + i + 1] = listTwitterStatus[i].User.CreatedDate;
                twitterPost.Date= listTwitterStatus[i].User.CreatedDate;
                ViewData["Username"+ i + 2] = listTwitterStatus[i].User.Name;
                twitterPost.Username= listTwitterStatus[i].User.Name;
                ViewData["Content"+i+3] = listTwitterStatus[i].Text;
                twitterPost.ContentDescription= listTwitterStatus[i].Text;
                tags = Regex.Split(listTwitterStatus[i].Text, @"\s+").Where(b => b.StartsWith("#"));
                for (int x = 0; x < tags.Count(); x++)
                {
                    hashtag.HashtagName = tags.ElementAt(x);
                    db.Hashtags.Add(hashtag);
                    db.SaveChanges();
                    //POMOCY Z FOREIGN KEY !!!
                }
                // nie moge znalezc direct linka twitterPost.DirectLinkToStatus = listTwitterStatus[i].
                db.Posts.Add(twitterPost);
                db.SaveChanges();
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