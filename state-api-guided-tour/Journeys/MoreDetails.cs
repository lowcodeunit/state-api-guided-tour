using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Fathym;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Runtime.Serialization;
using Fathym.API;
using System.Collections.Generic;
using System.Linq;
using LCU.Personas.Client.Applications;
using LCU.StateAPI.Utilities;
using System.Security.Claims;
using LCU.Personas.Client.Enterprises;
using LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour.State;
using LCU.State.API.NapkinIDE.NapkinIDE.ToursManagement.State;
using LCU.State.API.NapkinIDE.NapkinIDE.JourneysManagement.State;

namespace LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour.Journeys
{    
    [Serializable]
    [DataContract]
    public class MoreDetailsRequest
    {
        [DataMember]
        public virtual string JourneyLookup { get; set; }
    }

    public class MoreDetails
    {
        public MoreDetails()
        { }

        [FunctionName("MoreDetails")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = GuidedTourState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-api-key}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            return await stateBlob.WithStateHarness<JourneysManagementState, MoreDetailsRequest, JourneysManagementStateHarness>(req, signalRMessages, log,
                async (harness, reqData, actReq) =>
            {
                log.LogInformation($"Setting Active Tour to: {reqData.JourneyLookup}");

                var stateDetails = StateUtils.LoadStateDetails(req);

                await harness.MoreDetails(reqData.JourneyLookup);

                return Status.Success;
            });
        }
    }
}
