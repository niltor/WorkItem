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
using Android.Util;
using System.Net.Http.Headers;

namespace VSTS.Services
{
    public class TokenService
    {
        private Context _context;


        public TokenService(Context context)
        {
            _context = context;
        }

        public string GenerateRefreshPostData(string clientSecret, string authCode, string callbackUrl)
        {
            return String.Format("client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=refresh_token&assertion={1}&redirect_uri={2}",
                        URLEncoder.Encode(clientSecret, "utf-8"),
                        URLEncoder.Encode(authCode, "utf-8"),
                        callbackUrl
                 );
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            string url = "https://app.vssps.visualstudio.com/oauth2/token";
            var stream = _context.Assets.Open("config.prod.json");
            string configString = new StreamReader(stream).ReadToEnd();

            var config = JsonConvert.DeserializeObject<Models.Config>(configString);
            using (var hc = new HttpClient())
            {
                var postData = GenerateRefreshPostData(config.ClientSecret, refreshToken, config.CallbackUrl);
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Content-Length", postData.Length.ToString());
                var response = await hc.PostAsync(url, new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"));
                var result = response.Content.ReadAsStringAsync().Result;

                var token = JsonConvert.DeserializeObject<Token>(response.Content.ReadAsStringAsync().Result);
                //存储内容
                var sp = _context.GetSharedPreferences("config", FileCreationMode.Private);
                var editor = sp.Edit();
                editor.PutString("token", token.AccessToken);
                editor.PutString("refresh_token", token.RefreshToken);
                editor.PutString("expiration", token.ExpiresIn);
                editor.PutString("token_type", token.TokenType);
                editor.Commit();

                return token.AccessToken;
            }
        }

    }
}