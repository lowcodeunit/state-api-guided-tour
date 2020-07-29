using System;
using System.IO;
using System.Linq;
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
using LCU.Personas.Client.Enterprises;
using LCU.Personas.Client.DevOps;
using LCU.Personas.Enterprises;
using LCU.Personas.Client.Applications;
using Fathym.API;
using LCU.Graphs.Registry.Enterprises.Apps;

namespace LCU.State.API.NapkinIDE.NapkinIDE.JourneysManagement.State
{
    public class JourneysManagementStateHarness : LCUStateHarness<JourneysManagementState>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public JourneysManagementStateHarness(JourneysManagementState state)
            : base(state ?? new JourneysManagementState())
        { }
        #endregion

        #region API Methods
        public virtual async Task EstablishIoTStarter()
        {
            State.IsIoTStarter = true;
        }

        public virtual async Task LoadIoTData()
        {
            State.IoTData = new List<JourneysIoTDetails>();

            State.IoTData.Add(new JourneysIoTDetails()
            {
                Name = "Device 1",
                Color = "#5AA454",
                Data = new Dictionary<string, double>()
                {
                    { "July 16th", 74 },
                    { "July 17th", 78 },
                    { "July 18th", 82 }
                }
            });

            State.IoTData.Add(new JourneysIoTDetails()
            {
                Name = "Device 2",
                Color = "#E44D25",
                Data = new Dictionary<string, double>()
                {
                    { "July 16th", 82 },
                    { "July 17th", 86 },
                    { "July 18th", 84 }
                }
            });

            State.IoTData.Add(new JourneysIoTDetails()
            {
                Name = "Device 3",
                Color = "#CFC0BB",
                Data = new Dictionary<string, double>()
                {
                    { "July 16th", 68 },
                    { "July 17th", 78 },
                    { "July 18th", 85 }
                }
            });
        }

        public virtual async Task LoadJourneyOptions()
        {
            State.Journeys = new List<JourneyOption>()
            {
                new JourneyOption()
                {
                    Name = "IoT - To the Edge and Beyond!",
                    ContentURL = "https://player.vimeo.com/video/403508452",
                    ContentType = JourneyContentTypes.Video,
                    Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                    Description = "Build and connect edge devices, securely manage, visualize and analyze your data, and take action on your intelligence.",
                    Roles = new List<string>(){ "IoT", "Data Science" },
                    Active = true
                },
                // new JourneyOption()
                // {
                //     Name = "Application Development",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "JS Apps", "Security", "Dev Tools" },
                //     Description = "Develop JavaScript applications in the framework of your choosing and easily deploy, secure, and manage at scale.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                //     ComingSoon = true,
                //     Active = true
                // },
                // new JourneyOption()
                // {
                //     Name = "Cloud Development",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "DevOps", "IaC", "Data Flow" },
                //     Description =  "Rapidly set up and manage enterprise grade, best practice cloud infrastructures and leverage them to build apps and APIs.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                //     ComingSoon = true,
                //     Active = true
                // },
                // new JourneyOption()
                // {
                //     Name = "Data Development",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "AI/ML", "Analytics", "Reporting" },
                //     Description =  "Develop data applications from existing and new enterprise data. Leverage existing tools with new at a rapid pace.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                //     ComingSoon = true,
                //     Active = true
                // },
                // new JourneyOption()
                // {
                //     Name = "Cloud Orchestration",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "DevOps", "IaC", "Data Flow" },
                //     Description =  "Rapidly set up and manage enterprise grade, best practice cloud infrastructures and leverage them to build apps and APIs.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                //     ComingSoon = true,
                //     Active = true
                // },
                // new JourneyOption()
                // {
                //     Name = "Enterprise Intranets",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "Dashboards", "Reporting", "Identity" },
                //     Description =  "Leverage our Enterprise IDE to rapidly pull together open source and custom LCUs to drive value in your organization.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Developer },
                //     ComingSoon = true,
                //     Active = true
                // },
                // new JourneyOption()
                // {
                //     Name = "Designer Tools",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "AI/ML", "Analytics", "Reporting" },
                //     Description =  "Develop data applications from existing and new enterprise data. Leverage existing tools with new at a rapid pace.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Designer },
                //     ComingSoon = true,
                //     Active = true
                // },
                // new JourneyOption()
                // {
                //     Name = "Admin Tools",
                //     ContentURL = "https://www.google.com/logos/doodles/2020/thank-you-grocery-workers-6753651837108758.2-law.gif",
                //     ContentType = JourneyContentTypes.Image,
                //     Uses = new List<string>() { "AI/ML", "Analytics", "Reporting" },
                //     Description =  "Develop data applications from existing and new enterprise data. Leverage existing tools with new at a rapid pace.",
                //     Roles = new List<JourneyRoleTypes>(){ JourneyRoleTypes.Administrator },
                //     ComingSoon = true,
                //     Active = true
                // }
            };

            if (State.IsIoTStarter)
            {
                State.Journeys.AddRange(new List<JourneyOption>()
                {
                    new JourneyOption()
                    {
                        Name = "Device Insights & Monitoring",
                        ContentURL = "https://player.vimeo.com/video/403508452",
                        ContentType = JourneyContentTypes.Video,
                        Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                        Description = "Monitor and gain insights about your devices from R&D through deployment.",
                        Roles = new List<string>(){ "IoT", "Insights" },
                        Active = true,
                        HighlightedOrder = 1
                    },
                    new JourneyOption()
                    {
                        Name = "Mean, Median and Standard Deviation with Machine Learning",
                        ContentURL = "https://player.vimeo.com/video/403508452",
                        ContentType = JourneyContentTypes.Video,
                        Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                        Description = "Calculate Elementary Statistics on your stream of IoT data.",
                        Roles = new List<string>(){ "IoT", "Data Science", "Insights" },
                        Active = true,
                        HighlightedOrder = 2
                    },
                    new JourneyOption()
                    {
                        Name = "ABB Device Notifications and Resolution",
                        ContentURL = "https://player.vimeo.com/video/403508452",
                        ContentType = JourneyContentTypes.Video,
                        Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                        Description = "Receive notifications around configured threshholds and resolve them once addressed.",
                        Roles = new List<string>(){ "IoT", "Insights" },
                        Active = true,
                        HighlightedOrder = 3
                    },
                    new JourneyOption()
                    {
                        Name = "Augment your Data with Real-Time Surface Forecast",
                        ContentURL = "https://player.vimeo.com/video/403508452",
                        ContentType = JourneyContentTypes.Video,
                        Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                        Description = "Receive notifications around configured threshholds and resolve them once addressed.",
                        Roles = new List<string>(){ "IoT", "Insights" },
                        Active = true,
                        HighlightedOrder = 3
                    }
                });
            }
        }

        public virtual async Task RefreshJourneys()
        {
            await EstablishIoTStarter();

            // if (State.IsIoTStarter)
            //     await LoadIoTData();

            await LoadJourneyOptions();
        }
        #endregion
    }
}