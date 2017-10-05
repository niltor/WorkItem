using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using VSTS.Models.ViewModels;
using VSTS.Models.WorkItemResult;
using VSTS.Services;
using static Android.Widget.AdapterView;

namespace VSTS.Fragments
{
    public class WorkItemListFragment : ListFragment
    {
        public List<IDictionary<string, object>> listData = new List<IDictionary<string, object>>();
        private string token;
        private string type;//内容类型
        public override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            token = Arguments.GetString("access_token");
            type = Arguments.GetString("type");

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.WorkItemListFragment, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {

            base.OnViewCreated(view, savedInstanceState);
            var sp = Activity.GetSharedPreferences("data", FileCreationMode.Private);
            var workItemsListData = new List<WorkitemList>();//要显示的数据列表
            //从本地取出数据 ，不再进行网络请求
            var workitemsData = sp.GetString("workitems", null);
            if (!string.IsNullOrEmpty(workitemsData))
            {
                var workItems = JsonConvert.DeserializeObject<WorkItemResult>(workitemsData);
                workItemsListData = workItems.Value
                    .Where(v => !v.Fields.SystemState.Equals("Closed"))
                    .OrderBy(v => v.Fields.SystemState)
                    .Select(v =>
                new WorkitemList
                {
                    Title = v.Fields.SystemTitle,
                    Type = v.Fields.SystemWorkItemType,
                    State = v.Fields.SystemState,
                    Id = v.Id,
                    Url = v.Url,
                    AssignedTo = v.Fields.SystemAssignedTo
                }).ToList();
                //过滤数据，只取当前分类
                switch (type)
                {
                    case "全部":
                        break;
                    case "任务":
                        workItemsListData = workItemsListData
                            .Where(item => item.Type.Equals("Task"))
                            .ToList();
                        break;
                    case "Bug":
                        workItemsListData = workItemsListData
                            .Where(item => item.Type.Equals("Bug"))
                            .ToList();
                        break;
                    default:
                        break;
                }
            }

            ListAdapter = new MyWorkitemAdapter(Activity, workItemsListData);
            ListView.ItemClick += delegate (object sender, ItemClickEventArgs args)
            {
                var intent = new Intent(Activity,typeof(WorkitemDeatilActivity));
                Activity.StartActivity(intent);
            };

        }
        internal static WorkItemListFragment NewInstance(string access_token, string type = "")
        {
            var bundle = new Bundle();
            bundle.PutString("access_token", access_token);
            bundle.PutString("type", type);
            return new WorkItemListFragment
            {
                Arguments = bundle
            };
        }

    }
    class MyWorkitemAdapter : BaseAdapter
    {
        protected Android.App.Activity _activity;
        protected List<WorkitemList> _listData;
        public MyWorkitemAdapter(Android.App.Activity activity, List<WorkitemList> listData)
        {
            _activity = activity;
            _listData = listData;
        }

        public override int Count => _listData.Count;

        public override Java.Lang.Object GetItem(int position)
        {

            return null;
        }

        public override long GetItemId(int position)
        {
            return _listData[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //获取控件
            var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.workitem_list_item, parent, false);
            var stateView = view.FindViewById<TextView>(Resource.Id.workitem_state);
            var idView = view.FindViewById<TextView>(Resource.Id.workitem_id);
            var titleView = view.FindViewById<TextView>(Resource.Id.workitem_title);
            var typeView = view.FindViewById<ImageView>(Resource.Id.workitem_type);
            var assignView = view.FindViewById<TextView>(Resource.Id.workitem_assigned_to);

            //设置基本内容
            idView.Text = Convert.ToString(_listData.ElementAt(position).Id);
            stateView.Text = _listData.ElementAt(position).State;
            titleView.Text = _listData.ElementAt(position).Title;
            assignView.Text = _listData.ElementAt(position).AssignedTo;

            //设置类型图标
            switch (_listData[position].Type)
            {
                case "Bug":
                    typeView.SetImageResource(Resource.Drawable.bug);
                    break;
                case "Feature":
                    typeView.SetImageResource(Resource.Drawable.feature);

                    break;
                case "Issue":
                    typeView.SetImageResource(Resource.Drawable.issue);

                    break;
                case "Task":
                    typeView.SetImageResource(Resource.Drawable.task);
                    break;
                case "User Story":
                    typeView.SetImageResource(Resource.Drawable.book);
                    break;
                default:
                    break;
            }

            //设置状态颜色
            switch (stateView.Text)
            {
                case "Closed":
                    stateView.SetTextColor(new Color(51, 153, 51));
                    break;
                case "Active":
                    stateView.SetTextColor(new Color(0, 122, 204));
                    break;
                case "New":
                    stateView.SetTextColor(new Color(178, 178, 178));
                    break;
                case "Resolved":
                    stateView.SetTextColor(Color.DarkOrange);
                    break;
            }
            return view;
        }
    }
}