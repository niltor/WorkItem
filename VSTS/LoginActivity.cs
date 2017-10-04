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

namespace VSTS
{
    [Activity(Label = "VSTS登录", MainLauncher = true)]
    public class LogActivity : Activity
    {
        private WebView webView;
        private static LogActivity activity;

        static ISharedPreferences sp;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            sp = GetSharedPreferences("config", FileCreationMode.Private);
            activity = this;

            //判断是否已经登录
            var refreshToken = sp.GetString("refresh_token", string.Empty);
            Log.Debug("vsts", "当前的 refreshToken:{0}", refreshToken);
            if (!string.IsNullOrEmpty(refreshToken))
            {
               
                //获取新的token，存储并跳转
                var service = new TokenService(this);
                await service.RefreshToken(refreshToken);
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }

            webView = (WebView)FindViewById(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.DefaultTextEncodingName = "utf-8";
            webView.Settings.UseWideViewPort = true;
            webView.SetWebViewClient(new MyWebViewClient());
            webView.LoadUrl("https://workitem.msdev.cc/");

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

                    var editor = sp.Edit();
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

