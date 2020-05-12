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
using LCU.State.API.NapkinIDE.NapkinIDE.JourneysManagement.State;
using LCU.State.API.NapkinIDE.NapkinIDE.ToursManagement.State;

namespace LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour
{
    [Serializable]
    [DataContract]
    public class RefreshRequest : BaseRequest
    { }

    public class Refresh
    {
        protected EnterpriseManagerClient entMgr;

        public Refresh(EnterpriseManagerClient entMgr)
        {
            this.entMgr = entMgr;
        }

        #region API Methods
        [FunctionName("Refresh")]
        public virtual async Task<Status> Run([HttpTrigger] HttpRequest req, ILogger log,
            [SignalR(HubName = GuidedTourState.HUB_NAME)]IAsyncCollector<SignalRMessage> signalRMessages,
            [Blob("state-api/{headers.lcu-ent-api-key}/{headers.lcu-hub-name}/{headers.x-ms-client-principal-id}/{headers.lcu-state-key}", FileAccess.ReadWrite)] CloudBlockBlob stateBlob)
        {
            var stateDetails = StateUtils.LoadStateDetails(req);

            if (stateDetails.StateKey == "journeys")
                return await stateBlob.WithStateHarness<JourneysManagementState, RefreshRequest, JourneysManagementStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing Journeys state");

                    return await refreshJourneys(harness, log, stateDetails);
                });
            else if (stateDetails.StateKey == "tours")
                return await stateBlob.WithStateHarness<ToursManagementState, RefreshRequest, ToursManagementStateHarness>(req, signalRMessages, log,
                    async (harness, refreshReq, actReq) =>
                {
                    log.LogInformation($"Refreshing Tours state");

                    return await refreshTours(harness, log, stateDetails);
                });
            else
                throw new Exception("A valid State Key must be provided (journeys, tours).");
        }
        #endregion

        #region Helpers
        protected virtual async Task<Status> refreshTours(ToursManagementStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            harness.RefreshTours();

            return Status.Success;
        }

        protected virtual async Task<Status> refreshJourneys(JourneysManagementStateHarness harness, ILogger log, StateDetails stateDetails)
        {
            await harness.RefreshJourneys();

            return Status.Success;
        }
        #endregion

    }

    
}
