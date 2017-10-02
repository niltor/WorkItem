using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using VSTS.Services;
using static Android.Widget.AdapterView;

namespace VSTS.Fragments
{

    public class WorkItemFragment : ListFragment
    {
        private static string[] data = new string[] { "first", "second", "third" };

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var token = Arguments.GetString("access_token");

            Log.Debug("vsts", token);
            var service = new VSTSService(token);
            var projects = await service.GetMyProjectAsync();

            var projectTitles = projects.Value.Select(s => s.Name).ToArray();

            ListAdapter = new ArrayAdapter<string>(Activity, Resource.Layout.projetct_list_item, Resource.Id.project_content, projectTitles);

            ListView.ItemClick += delegate (object sender, ItemClickEventArgs args)
            {
                Toast.MakeText(Activity, ((TextView)args.View).Text, ToastLength.Long).Show();
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.WorkItemFragment, null);

        }
        internal static WorkItemFragment NewInstance()
        {
            return new WorkItemFragment
            {
                Arguments = new Bundle()
            };
        }
    }
}