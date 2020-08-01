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
using LCU.Graphs.Registry.Enterprises.Apps;
using LCU.Presentation.State.ReqRes;
using LCU.StateAPI.Utilities;
using LCU.StateAPI;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace LCU.State.API.NapkinIDE.NapkinIDE.JourneysManagement.State
{
    [Serializable]
    [DataContract]
    public class JourneysManagementState
    {
        [DataMember]
        public virtual List<JourneysIoTDetails> IoTData { get; set; }

        [DataMember]
        public virtual bool IsIoTStarter { get; set; }

        [DataMember]
        public virtual List<JourneyOption> Journeys { get; set; }

        [DataMember]
        public virtual bool Loading { get; set; }
    }

    [Serializable]
    [DataContract]
    public class JourneysIoTDetails
    {
        [DataMember]
        public virtual string Color { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual Dictionary<string, double> Data { get; set; }
    }

    [DataContract]
    public class JourneyOption
    {
        [DataMember]
        public virtual bool Active { get; set; }

        [DataMember]
        public virtual string ActionURL { get; set; }

        [DataMember]
        public virtual bool ComingSoon { get; set; }

        [DataMember]
        public virtual string ContentURL { get; set; }

        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual JourneyContentTypes ContentType { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        public virtual JourneyOptionDetails Details { get; set; }

        [DataMember]
        public virtual int? HighlightedOrder { get; set; }

        [DataMember]
        public virtual string Lookup { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual List<string> Roles { get; set; }

        [DataMember]
        public virtual List<string> Uses { get; set; }
    }

    [DataContract]
    public class JourneyOptionDetails
    {
        [DataMember]
        public virtual string Abstract { get; set; }

        [DataMember]
        public virtual string Blog { get; set; }

        [DataMember]
        public virtual string Documentation { get; set; }

        [DataMember]
        public virtual List<string> RelatedJourneys { get; set; }
    }

    [DataContract]
    public enum JourneyContentTypes
    {
        [EnumMember]
        Image,

        [EnumMember]
        Video
    }
}
