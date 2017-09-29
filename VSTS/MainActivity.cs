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
using Android.Support.V7.App;

namespace VSTS
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);



            var sp = GetSharedPreferences("config", FileCreationMode.Private);
            var token = sp.GetString("token", string.Empty);


            var service = new TokenService(this);
 

        }
    }
}