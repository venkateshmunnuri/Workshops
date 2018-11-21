using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WorkShopsBucket.Controllers
{
    [RequireHttps()]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public void Success()
        {
            ParseAuthorizationCode();
        }

        private void GetUserInformation(string accessToken)
        {
            try

            {

                HttpClient client = new HttpClient();

                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken;

                client.CancelPendingRequests();

                HttpResponseMessage output = client.GetAsync(urlProfile).Result;

                if (output.IsSuccessStatusCode)

                {

                    string outputData = output.Content.ReadAsStringAsync().Result;

                    var serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                }

            }

            catch (Exception ex)

            {

                //catching the exception

            }
        }

        private void ParseAuthorizationCode()
        {
            try

            {
                var url = Request.Url.Query;

                if (url != "")

                {
                    string queryString = url.ToString();

                    char[] delimiterChars = { '=' };

                    string[] words = queryString.Split(delimiterChars);

                    string code = words[1];

                    var client_id = "278794354905-bqio775t8slrenom701km5gjnilpvfug.apps.googleusercontent.com";
                    var client_secret = "t1zsX5YOwh9fdymvTXHasGET";
                    var redirect_url = "https://localhost:44306/Login/Success";

                    if (code != null)

                    {

                        //get the access token

                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");

                        webRequest.Method = "POST";

                        string Parameters = "code=" + code + "&client_id=" + client_id + "&client_secret=" + client_secret + "&redirect_uri=" + redirect_url + "&grant_type=authorization_code";

                        byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);

                        webRequest.ContentType = "application/x-www-form-urlencoded";

                        webRequest.ContentLength = byteArray.Length;

                        Stream postStream = webRequest.GetRequestStream();

                        // Add the post data to the web request

                        postStream.Write(byteArray, 0, byteArray.Length);

                        postStream.Close();

                        WebResponse response = webRequest.GetResponse();

                        postStream = response.GetResponseStream();

                        StreamReader reader = new StreamReader(postStream);

                        string responseFromServer = reader.ReadToEnd();

                        GoogleAccessToken serStatus = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);

                        if (serStatus != null)

                        {

                            string accessToken = string.Empty;

                            accessToken = serStatus.access_token;

                            Session["Token"] = accessToken;

                            if (!string.IsNullOrEmpty(accessToken))

                            {
                                GetUserInformation(accessToken);
                                //call get user information function with access token as parameter

                            }

                        }

                    }

                }

            }

            catch (Exception ex)
            {
                //return RedirectToAction("Index", "Home");
            }

        }


        public void Failure()
        {
            
        }
    }

    public class GoogleAccessToken

    {

        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string id_token { get; set; }

        public string refresh_token { get; set; }

    }



    public class GoogleUserOutputData

    {

        public string id { get; set; }

        public string name { get; set; }

        public string given_name { get; set; }

        public string email { get; set; }

        public string picture { get; set; }

    }
}