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

namespace LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour
{    
    [Serializable]
    [DataContract]
    public class SetActiveTourRequest
    {
        [DataMember]
        public virtual string Lookup { get; set; }
    }

    public class SetActiveTour
    {
        protected EnterpriseManagerClient entMgr;

        public SetActiveTour(EnterpriseManagerClient entMgr)
        {
            this.entMgr = entMgr;
        }

        [FunctionName("SetActiveTour")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = GuidedTourState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-api-key}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            return await stateBlob.WithStateHarness<GuidedTourState, SetActiveTourRequest, GuidedTourStateHarness>(req, signalRMessages, log,
                async (harness, reqData, actReq) =>
            {
                log.LogInformation($"Setting Active Tour to: {reqData.Lookup}");

                var stateDetails = StateUtils.LoadStateDetails(req);

                await harness.SetActiveTour(stateDetails.EnterpriseAPIKey, reqData.Lookup);

                return Status.Success;
            });
        }
    }
}
