using DotNetOpenAuth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using OAuth;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bildirim.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {

        string oauth_consumer_key = "2gfllb4KNJcegRYaVmmhRGCUL";
        string oauth_consumer_secret = "JV28ouPCVryz1fXGgihGT77OPIMUANtaPcrKVGhWyNSzmiMs5R";


        [HttpGet("getAuthorize")]
        public async Task<IActionResult> GetAuthorize()
        {

            OAuthRequest client = OAuthRequest.ForRequestToken("2gfllb4KNJcegRYaVmmhRGCUL", "JV28ouPCVryz1fXGgihGT77OPIMUANtaPcrKVGhWyNSzmiMs5R");
            client.RequestUrl = "https://twitter.com/oauth/request_token";
            client.CallbackUrl = "https://localhost:7233/Twitter/accessToken";

            string auth = client.GetAuthorizationQuery();
            var url = client.RequestUrl + "?" + auth;
            HttpClient Httpclient = new HttpClient();

            var response = await Httpclient.GetAsync(url);
            var content = "";
            var oauthToken = "";
            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var responseString = content;
                var queryString = HttpUtility.ParseQueryString(responseString);

                oauthToken = queryString["oauth_token"];
                var oauthTokenSecret = queryString["oauth_token_secret"];
                var oauthCallbackConfirmed = queryString["oauth_callback_confirmed"];

            }
            else
            {
                Console.WriteLine("Request failed: " + response.StatusCode);
            }

            var url2 = "https://api.twitter.com/oauth/authorize";
            var url3 = url2 + "?" + content;

            var httpClient2 = new HttpClient();
            var response2 = await httpClient2.GetAsync(url3);
            var content2 = await response2.Content.ReadAsStringAsync();

            Process.Start(new ProcessStartInfo
            {
                FileName = url3,
                UseShellExecute = true
            });

            return Ok("");
        }

        //  string accessToken = "";
        //   string accessTokenSecret = "";

        [HttpGet("accessToken")]
        public async Task<IActionResult> GetAccessToken(string oauth_token, string oauth_verifier)
        {
            // Use the oauth_token and oauth_verifier parameters to exchange the request token for an access token
            var RequestUrl = "https://twitter.com/oauth/access_token";

            //   string auth = client.GetAuthorizationQuery();
            var url2 = RequestUrl + "?oauth_token=" + oauth_token + "&oauth_verifier=" + oauth_verifier;

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(url2, null);
            var content = await response.Content.ReadAsStringAsync();

            // Parse the access token from the response
            var responseString = content;
            var queryString = HttpUtility.ParseQueryString(responseString);
            var accessToken = queryString["oauth_token"];
            var accessTokenSecret = queryString["oauth_token_secret"];

            //   var token = "your_token_string";
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
            };
            Response.Cookies.Append("access_token", accessToken, cookieOptions);
            Response.Cookies.Append("access_Token_Secret", accessTokenSecret, cookieOptions);


            Console.WriteLine("accessToken= " + accessToken);
            Console.WriteLine("accessTokenSecret= " + accessTokenSecret);

            return Ok("Authorization successful!");

        }

        [HttpPost("SendTweet")]
        public async Task<IActionResult> TweetAt(string message)
        {

            var accessToken = Request.Cookies["access_token"];
            var accessTokenSecret = Request.Cookies["access_Token_Secret"];

            if (accessToken == null && accessTokenSecret == null)
            {
                // token yoksa, uygun bir hata yanıtı döndürün.
                return Unauthorized();
            }
            else
            {
                string twitterURL = "https://api.twitter.com/1.1/statuses/update.json";

                // create unique request details
                System.TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
                string oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
                string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
                string oauth_version = "1.0";
                string oauth_signature_method = "HMAC-SHA1";

                // create oauth signature
                string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" + "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

                string baseString = string.Format(
                    baseFormat,
                    oauth_consumer_key,
                    oauth_nonce,
                oauth_signature_method,
                    oauth_timestamp, accessToken,
                    oauth_version,
                    Uri.EscapeDataString(message)
                );

                string oauth_signature = null;
                using (HMACSHA1 hasher2 = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(Uri.EscapeDataString(oauth_consumer_secret) + "&" + Uri.EscapeDataString(accessTokenSecret))))
                {
                    oauth_signature = Convert.ToBase64String(hasher2.ComputeHash(ASCIIEncoding.ASCII.GetBytes("POST&" + Uri.EscapeDataString(twitterURL) + "&" + Uri.EscapeDataString(baseString))));
                }

                // create the request header
                string authorizationFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " + "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " + "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " + "oauth_version=\"{6}\"";

                string authorizationHeader = string.Format(
                    authorizationFormat,
                    Uri.EscapeDataString(oauth_consumer_key),
                    Uri.EscapeDataString(oauth_nonce),
                    Uri.EscapeDataString(oauth_signature),
                    Uri.EscapeDataString(oauth_signature_method),
                    Uri.EscapeDataString(oauth_timestamp),
                    Uri.EscapeDataString(accessToken),
                    Uri.EscapeDataString(oauth_version)
                );

                HttpWebRequest objHttpWebRequest = (HttpWebRequest)WebRequest.Create(twitterURL);
                objHttpWebRequest.Headers.Add("Authorization", authorizationHeader);
                objHttpWebRequest.Method = "POST";
                objHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                using (Stream objStream = objHttpWebRequest.GetRequestStream())
                {
                    byte[] content3 = ASCIIEncoding.ASCII.GetBytes("status=" + Uri.EscapeDataString(message));
                    objStream.Write(content3, 0, content3.Length);
                }

                var responseResult = "";

                try
                {
                    WebResponse objWebResponse = objHttpWebRequest.GetResponse();
                    StreamReader objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                    responseResult = objStreamReader.ReadToEnd().ToString();
                }
                catch (Exception ex)
                {
                    responseResult = "Twitter Post Error: " + ex.Message.ToString() + ", authHeader: " + authorizationHeader;
                }

                Console.WriteLine(responseResult);
                return Ok("Authorization successful!");

            }


        }


    }
}


