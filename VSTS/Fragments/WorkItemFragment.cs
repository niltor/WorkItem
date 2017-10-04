using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace VSTS.Fragments
{
    [Android.App.Activity(Label = "VSTS-WorkItems")]
    public class WorkItemFragment : Fragment
    {

        public static string _accessToken;
        public static string[] titles = { "全部", "任务", "Bug" };

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Log.Debug("vsts", "WorkItemFragment OnActivityCreated");
            base.OnActivityCreated(savedInstanceState);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug("vsts", "WorkItemFragment OnCreate");

            base.OnCreate(savedInstanceState);
            _accessToken = Arguments.GetString("access_token");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug("vsts", "WorkItemFragment OnCreatedView");

            var rootView = inflater.Inflate(Resource.Layout.WorkItemFragment, container, false);
            var tabLayout = rootView.FindViewById<TabLayout>(Resource.Id.workitem_tablayout);
            var viewPager = rootView.FindViewById<ViewPager>(Resource.Id.workitem_viewpager);

            viewPager.Adapter = new MyAdapter(ChildFragmentManager);
            viewPager.OffscreenPageLimit = 3;
            tabLayout.SetupWithViewPager(viewPager);

            return rootView;
        }



        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.workitem_menu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        internal static WorkItemFragment NewInstance()
        {
            return new WorkItemFragment
            {
                Arguments = new Bundle()
            };
        }
        class MyAdapter : FragmentPagerAdapter
        {
            public MyAdapter(FragmentManager fm) : base(fm)
            {
            }
            public override int Count => 3;

            public override ICharSequence GetPageTitleFormatted(int position)
            {
                return CharSequence.ArrayFromStringArray(titles)[position];
            }
            public override Fragment GetItem(int position)
            {
                Log.Debug("vsts", "当前标签:" + titles[position]);
                return WorkItemListFragment.NewInstance(_accessToken, titles[position]);
            }
        }
    }
}