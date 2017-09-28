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

namespace VSTS.Models
{
    class ProjectModels
    {
    }
    public class Value
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Url { get; set; }
        public string State { get; set; }
    }

    public class Projects
    {
        public int Count { get; set; }
        public List<Value> Value { get; set; }
    }

}