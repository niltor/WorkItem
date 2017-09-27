using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using VSTS.Services;
using System.IO;
using Android.Content.Res;

namespace VSTS
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity
    {
        private TextView messageTV;
        private TextView tokenTV;
        private Button button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            messageTV = (TextView)FindViewById(Resource.Id.message);
            tokenTV = (TextView)FindViewById(Resource.Id.token);
            button = (Button)FindViewById(Resource.Id.button);

            var sp = GetSharedPreferences("config", FileCreationMode.Private);
            var token = sp.GetString("token", string.Empty);

            tokenTV.Text = token;

            var service = new TokenService(this);
            button.Click += async delegate
             {
                 await service.GetMyProjectAsync(token);
             };

            // Create your application here
        }
    }
}