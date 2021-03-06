using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Fathym;
using LCU.Presentation.State.ReqRes;
using LCU.StateAPI.Utilities;
using LCU.StateAPI;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Fathym.Business.Models;

namespace LCU.State.API.NapkinIDE.NapkinIDE.ToursManagement.State
{
    [Serializable]
    [DataContract]
    public class ToursManagementState
    {
        [DataMember]
        public virtual Dictionary<string, string> CompletedTourLookups { get; set; }

        [DataMember]
        public virtual GuidedTour CurrentTour { get; set; }

        [DataMember]
        public virtual string EnvironmentLookup { get; set; }

        [DataMember]
        public virtual bool Loading { get; set; }

        [DataMember]
        public virtual Dictionary<string, List<GuidedTourStepRecord>> StepRecordHistory { get; set; }

        [DataMember]
        public virtual Dictionary<string, GuidedTourStepRecord> StepRecords { get; set; }

        [DataMember]
        public virtual List<GuidedTour> Tours { get; set; }

        [DataMember]
        public virtual bool ToursEnabled { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GuidedTour : BusinessModel<Guid>
    {
        [DataMember]
        public virtual bool IsFirstTimeViewing { get; set; }

        [DataMember]
        public virtual string Lookup { get; set; }

        [DataMember]
        public virtual int MinimumScreenSize { get; set; }

        [DataMember]
        public virtual bool PreventBackdropFromAdvancing { get; set; }

        [DataMember]
        public virtual ResizeDialog ResizeDialog { get; set; }

        [DataMember]
        public virtual List<GuidedTourStep> Steps { get; set; }

        [DataMember]
        public virtual bool UseOrb { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GuidedTourStep : BusinessModel<Guid>
    {
        [DataMember]
        public virtual int ActionDelay { get; set; }

        [DataMember]
        public virtual string Content { get; set; }

        [DataMember]
        public virtual int HighlightPadding { get; set; }

        [DataMember]
        public virtual string IframeSelector { get; set; }

        [DataMember]
        public virtual string Lookup { get; set; }

        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual OrientationTypes Orientation { get; set; }

        [DataMember]
        public virtual bool SkipStep { get; set; }

        [DataMember]
        public virtual int ScrollAdjustment { get; set; }

        [DataMember]
        public virtual string Selector { get; set; }

        [DataMember]
        public virtual string Subtitle { get; set; }

        [DataMember]
        public virtual string Title { get; set; }

        [DataMember]
        public virtual string useHighlightPadding { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GuidedTourStepRecord
    {
        [DataMember]
        public virtual string CurrentStep { get; set; }

        [DataMember]
        public virtual List<string> StepHistory { get; set; }
    }

    [Serializable]
    [DataContract]
    public class OrientationConfiguration
    {
        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual OrientationTypes Orientation { get; set; }

        [DataMember]
        public virtual int MaximumSize { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ResizeDialog
    {
        [DataMember]
        public virtual string Content { get; set; }

        [DataMember]
        public virtual string Title { get; set; }
    }

    [Serializable]
    [DataContract]
    public enum OrientationTypes
    {
        [EnumMember]
        Bottom,

        [EnumMember]
        BottomLeft,

        [EnumMember]
        BottomRight,

        [EnumMember]
        Center,

        [EnumMember]
        Left,

        [EnumMember]
        Right,

        [EnumMember]
        Top,

        [EnumMember]
        TopLeft,

        [EnumMember]
        TopRight
    }
}
