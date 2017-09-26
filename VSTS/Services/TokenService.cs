using System;
using System.Net;
using Java.Net;
using Android.Content;
using System.IO;
using Newtonsoft.Json;
using VSTS.Models;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace VSTS.Services
{
    public class TokenService
    {
        private Context _context;

        public TokenService(Context context)
        {
            _context = context;
        }


        public async Task<bool> RefreshToken(string refreshToken)
        {
            string url = "oauth2/token";
            var stream = _context.Assets.Open("config.prod.json");
            string configString = new StreamReader(stream).ReadToEnd();
            var config = JsonConvert.DeserializeObject<Config>(configString);
            using (var wc = new WebClient())
            {
                wc.BaseAddress = "https://app.vssps.visualstudio.com/";
                var postData = GenerateRequestPostData(config.ClientSecret, refreshToken, config.CallbackUrl);
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //wc.Headers.Add("Content-Length", Encoding.ASCII.GetBytes(postData).Length.ToString());
                var result = await wc.UploadDataTaskAsync(url, "POST", Encoding.UTF8.GetBytes(postData));
                var token = JsonConvert.DeserializeObject<Token>(Encoding.UTF8.GetString(result));

                //存储内容
                var sp = _context.GetSharedPreferences("config", FileCreationMode.Private);
                var editor = sp.Edit();
                editor.PutString("token", token.AccessToken);
                editor.PutString("refresh_token", token.RefreshToken);
                editor.PutString("expiration", token.ExpiresIn);
                editor.PutString("token_type", token.TokenType);
                editor.Commit();
            }
            return true;
        }

        public string GenerateRequestPostData(string clientSecret, string authCode, string callbackUrl)
        {
            return String.Format("client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}",
                        URLEncoder.Encode(clientSecret, "utf-8"),
                        URLEncoder.Encode(authCode, "utf-8"),
                        callbackUrl
                 );
        }


        public async Task TestAsync()
        {
            //var client = new HttpClient();
            //var result = await client.GetAsync("https://msdev.cc");s            //if (result.IsSuccessStatusCode)
            //{
            //    var re= await result.Content.ReadAsStringAsync();

            //}
            //    Console.WriteLine(re);


            using (var wc = new WebClient())
            {
                var re = await wc.DownloadStringTaskAsync(new Uri("https://baidu.com"));


                Console.WriteLine(re);
            }
        }
    }
}