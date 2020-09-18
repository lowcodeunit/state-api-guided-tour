using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using LCU.StateAPI;
using Microsoft.Azure.Storage.Blob;
using LCU.StateAPI.Utilities;
using LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour.State;
using LCU.State.API.NapkinIDE.NapkinIDE.ToursManagement.State;
using LCU.State.API.NapkinIDE.NapkinIDE.JourneysManagement.State;

namespace LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour.Host
{
    public static class ConnectToState
    {
        [FunctionName("ConnectToState")]
        public static async Task<ConnectToStateResponse> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequest req, ILogger log,
            ClaimsPrincipal claimsPrincipal, //[LCUStateDetails]StateDetails stateDetails,
            [SignalR(HubName = GuidedTourState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [SignalR(HubName = GuidedTourState.HUB_NAME)]IAsyncCollector<SignalRGroupAction> signalRGroupActions,
            [Blob("state-api/{headers.lcu-ent-lookup}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            var stateDetails = StateUtils.LoadStateDetails(req);

            if (stateDetails.StateKey == "tours")
                return await signalRMessages.ConnectToState<ToursManagementState>(req, log, claimsPrincipal, stateBlob, signalRGroupActions);
            else if (stateDetails.StateKey == "journeys")
                return await signalRMessages.ConnectToState<JourneysManagementState>(req, log, claimsPrincipal, stateBlob, signalRGroupActions);
            else
                throw new Exception("A valid State Key must be provided (tours, journeys).");
        }
    }
}
