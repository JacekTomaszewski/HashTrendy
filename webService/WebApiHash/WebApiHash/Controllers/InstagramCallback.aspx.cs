using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApiHash.Controllers
{
    public partial class InstagramCallback : System.Web.UI.Page
    {
        static String code = "";
        InstagramRegister fromRegisterClass = new InstagramRegister();
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!String.IsNullOrEmpty(Request["code"]) && !Page.IsPostBack)
            {
                code = Request["code"].ToString();
                //  GetDataInstagramToken();
                GetToken();
            }
        }

        void GetToken()
        {
            var json = "";
            try
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("client_id", fromRegisterClass.ClientId);
                parameters.Add("client_secret", fromRegisterClass.ClientSecret);
                parameters.Add("grant_type", "authorization_code");
                parameters.Add("grant_type", "client_credentials"); 
                parameters.Add("redirect_uri", fromRegisterClass.RedirectURI);
                parameters.Add("code", code);

                WebClient client = new WebClient();
                var result = client.UploadValues("https://api.instagram.com/oauth/access_token", "POST", parameters);
                var response = System.Text.Encoding.Default.GetString(result);
                var jsResult = (JObject)JsonConvert.DeserializeObject(response);

                Label.Text = "var response: " + response + "/n var jsResult: " + jsResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}