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
using Microsoft.Azure.Storage.Blob;
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
using LCU.Personas.Client.Identity;

namespace LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour.Tours
{
    [Serializable]
    [DataContract]
    public class ResetRequest
    { }

    public class Reset
    {
        protected IdentityManagerClient idMgr;

        public Reset(IdentityManagerClient idMgr)
        { 
            this.idMgr = idMgr;}

        [FunctionName("Reset")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = GuidedTourState.HUB_NAME)] IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-lookup}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            return await stateBlob.WithStateHarness<ToursManagementState, RecordStepRequest, ToursManagementStateHarness>(req, signalRMessages, log,
                async (harness, reqData, actReq) =>
            {
                log.LogInformation($"Recording step for {reqData.TourLookup}: {reqData.CurrentStep}");

                var stateDetails = StateUtils.LoadStateDetails(req);

                await harness.Reset(idMgr, stateDetails.EnterpriseLookup, stateDetails.Username);

                return Status.Success;
            });
        }
    }
}
