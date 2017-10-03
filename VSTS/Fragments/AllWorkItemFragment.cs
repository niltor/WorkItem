using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using VSTS.Services;
using static Android.Widget.AdapterView;

namespace VSTS.Fragments
{
    public class AllWorkItemFragment : ListFragment
    {
        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var token = Arguments.GetString("access_token");
            var service = new VSTSService(token);
            var projects = await service.GetMyProjectAsync();

            var workItems = await service.GetWorkItemsAsync("MSDeveloper", "Task");

            var workItemsListData = workItems.Value.Select(v => new
            {
                Title = v.Fields.SystemTitle,
                Type = v.Fields.SystemWorkItemType,
                State = v.Fields.SystemState
            }).ToList();

            var listData = new List<IDictionary<string, object>>();
            foreach (var item in workItemsListData)
            {
                var dictionary = new JavaDictionary<string, object>
                {
                    { "Title", item.Title },
                    { "Type", item.Type },
                    { "State", item.State }
                };
                listData.Add(dictionary);
            }

            ListAdapter = new SimpleAdapter(Activity,
                listData,
                Resource.Layout.projetct_list_item,
                new string[] { "Title", "Type", "State" },
                new int[] { Resource.Id.workitem_content, Resource.Id.workitem_type, Resource.Id.workitem_state }
                );

            ListView.ItemClick += delegate (object sender, ItemClickEventArgs args)
            {
                Toast.MakeText(Activity, ((TextView)args.View).Text, ToastLength.Long).Show();
            };
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.AllWorkItemFragment, null);
        }

        internal static AllWorkItemFragment NewInstance(string access_token)
        {
            var bundle = new Bundle();
            bundle.PutString("access_token", access_token);
            return new AllWorkItemFragment
            {
                Arguments = bundle
            };
        }
    }
}