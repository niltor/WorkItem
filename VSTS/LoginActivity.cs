using Android.App;
using Android.OS;
using Android.Webkit;
using Android.Graphics;
using Android.Content;
using Android.Net;
using System.Net;
using VSTS.Services;
using System.Threading.Tasks;
using Android.Util;
using Newtonsoft.Json;

namespace VSTS
{
    [Activity(Label = "VSTS", MainLauncher = true)]
    public class LogActivity : Activity
    {
        private WebView webView;
        private static LogActivity activity;

        static ISharedPreferences spConfig;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            spConfig = GetSharedPreferences("config", FileCreationMode.Private);
            activity = this;

            webView = (WebView)FindViewById(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DefaultTextEncodingName = "utf-8";
            webView.Settings.UseWideViewPort = true;

            //判断是否已经登录
            var refreshToken = spConfig.GetString("refresh_token", string.Empty);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                webView.LoadUrl("file:///android_asset/loading.html");
                //获取新的token，存储并跳转
                var service = new TokenService(this);
                var accessToken = await service.RefreshToken(refreshToken);

                //获取项目及workitem信息并保存
                var VSTSService = new VSTSService(accessToken);
                var projects = await VSTSService.GetMyProjectAsync();
                var workitems = await VSTSService.GetWorkItemsAsync();

                var spData = GetSharedPreferences("data", FileCreationMode.Private);
                var editor = spData.Edit();
                editor.PutString("projects", JsonConvert.SerializeObject(projects));
                editor.PutString("workitems", JsonConvert.SerializeObject(workitems));
                editor.Commit();

                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                webView.SetWebViewClient(new MyWebViewClient());
                webView.LoadUrl("https://workitem.msdev.cc/");
            }
        }

        private class MyWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                view.LoadUrl(request.Url.ToString());
                return true;
            }

            public override void OnPageStarted(WebView view, string url, Bitmap favicon)
            {
                Uri uri = Uri.Parse(url);
                if (url.ToLower().Contains("loginresult"))
                {
                    var token = uri.GetQueryParameter("token");
                    var refresh_token = uri.GetQueryParameter("refresh_token");
                    var expiration = uri.GetQueryParameter("expiration");

                    var editor = spConfig.Edit();
                    editor.PutString("token", token);
                    editor.PutString("refresh_token", refresh_token);
                    editor.PutString("expiration", expiration);
                    editor.Commit();

                    var intent = new Intent(activity, typeof(MainActivity));
                    activity.StartActivity(intent);
                    activity.Finish();
                }
            }
        }
    }
}

