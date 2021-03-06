using Android.Content;
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
            base.OnActivityCreated(savedInstanceState);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            _accessToken = Arguments.GetString("access_token");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var rootView = inflater.Inflate(Resource.Layout.WorkItemFragment, container,false);
            var tabLayout = rootView.FindViewById<TabLayout>(Resource.Id.workitem_tablayout);
            var viewPager = rootView.FindViewById<ViewPager>(Resource.Id.workitem_viewpager);
            var floatButton = rootView.FindViewById<FloatingActionButton>(Resource.Id.workitem_float_action_button);

            floatButton.Click += FloatButton_Click;
            viewPager.Adapter = new MyAdapter(ChildFragmentManager);
            viewPager.OffscreenPageLimit = 3;
            tabLayout.SetupWithViewPager(viewPager);
            return rootView;
        }

        private void FloatButton_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(Activity, typeof(AddWorkitemActivity));
            Activity.StartActivity(intent);
        }

        public void OnCheckedChanged(FloatingActionButton fabView, bool isChecked)
        {
            switch (fabView.Id)
            {
                case Resource.Id.workitem_float_action_button:
                    Log.Debug("vsts", string.Format("FAB 1 was {0}.", isChecked ? "checked" : "unchecked"));
                    break;
                default:
                    break;
            }
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
                return WorkItemListFragment.NewInstance(_accessToken, titles[position]);
            }
        }
    }
}