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

namespace VSTS.Models.ViewModels
{
    class WorkitemList
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string AssignedTo { get; set; }
        public string Url { get; set; }
    }
}