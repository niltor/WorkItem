using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
namespace VSTS.Fragments
{
    public class HomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug("vsts", "home oncreate view");
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.HomeFragment, null);
        }

        internal static HomeFragment NewInstance()
        {
            return new  HomeFragment{
                Arguments = new Bundle()
            };
        }
    }
}