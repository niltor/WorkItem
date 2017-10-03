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
using Newtonsoft.Json;

namespace VSTS.Models.WorkItemResult
{
    public class WorkItemResult
    {
        public long Count { get; set; }
        public Value[] Value { get; set; }
    }

    public class Value
    {
        public long Id { get; set; }
        public Fields Fields { get; set; }
        public long Rev { get; set; }
        public string Url { get; set; }
    }

    public class Fields
    {
        [JsonProperty("Microsoft.VSTS.Common.ResolvedDate")]
        public string MicrosoftVSTSCommonResolvedDate { get; set; }

        [JsonProperty("System.ChangedBy")]
        public string SystemChangedBy { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ClosedBy")]
        public string MicrosoftVSTSCommonClosedBy { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ActivatedDate")]
        public string MicrosoftVSTSCommonActivatedDate { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ActivatedBy")]
        public string MicrosoftVSTSCommonActivatedBy { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.Activity")]
        public string MicrosoftVSTSCommonActivity { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.Priority")]
        public long MicrosoftVSTSCommonPriority { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ClosedDate")]
        public string MicrosoftVSTSCommonClosedDate { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ResolvedBy")]
        public string MicrosoftVSTSCommonResolvedBy { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ValueArea")]
        public string MicrosoftVSTSCommonValueArea { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.Severity")]
        public string MicrosoftVSTSCommonSeverity { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ResolvedReason")]
        public string MicrosoftVSTSCommonResolvedReason { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
        public string MicrosoftVSTSCommonStateChangeDate { get; set; }

        [JsonProperty("System.AreaPath")]
        public string SystemAreaPath { get; set; }

        [JsonProperty("Microsoft.VSTS.Scheduling.OriginalEstimate")]
        public long? MicrosoftVSTSSchedulingOriginalEstimate { get; set; }

        [JsonProperty("System.AssignedTo")]
        public string SystemAssignedTo { get; set; }

        [JsonProperty("System.Description")]
        public string SystemDescription { get; set; }

        [JsonProperty("System.CreatedBy")]
        public string SystemCreatedBy { get; set; }

        [JsonProperty("System.ChangedDate")]
        public string SystemChangedDate { get; set; }

        [JsonProperty("System.CreatedDate")]
        public string SystemCreatedDate { get; set; }

        [JsonProperty("System.Reason")]
        public string SystemReason { get; set; }

        [JsonProperty("System.TeamProject")]
        public string SystemTeamProject { get; set; }

        [JsonProperty("System.IterationPath")]
        public string SystemIterationPath { get; set; }

        [JsonProperty("System.State")]
        public string SystemState { get; set; }

        [JsonProperty("System.Title")]
        public string SystemTitle { get; set; }

        [JsonProperty("System.WorkItemType")]
        public string SystemWorkItemType { get; set; }
    }
}