
using Android.Content;
using Android.OS;
using VSTS.Services;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using VSTS.Fragments;
using Android.App;

namespace VSTS
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : AppCompatActivity
    {
        private Fragment fragment;
        private BottomNavigationView bottomNavigation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            bottomNavigation = (BottomNavigationView)FindViewById(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            var sp = GetSharedPreferences("config", FileCreationMode.Private);
            var token = sp.GetString("token", string.Empty);


            var service = new TokenService(this);

        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.menu_home:
                    fragment = HomeFragment.NewInstance();
                    break;
                //case Resource.Id.menu_audio:
                //    fragment = Fragment2.NewInstance();
                //    break;
                //case Resource.Id.menu_video:
                //    fragment = Fragment3.NewInstance();
                //    break;
            }

            if (fragment == null)
                return;

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }
    }
}