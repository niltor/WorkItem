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

namespace VSTS.Models.WorkItemIdResult
{
    public class WorkiItemIdResult
    {
        public Column[] Columns { get; set; }
        public string QueryType { get; set; }
        public string AsOf { get; set; }
        public string QueryResultType { get; set; }
        public WorkItem[] WorkItems { get; set; }
    }

    public class Column
    {
        public string ReferenceName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class WorkItem
    {
        public long Id { get; set; }
        public string Url { get; set; }
    }
}