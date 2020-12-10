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
        public JourneysManagementStateHarness(JourneysManagementState state, ILogger log)
            : base(state ?? new JourneysManagementState(), log)
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
                    Name = "Start Your Micro-Frontend Journey",
                    Lookup = "micro-frontends",
                    ContentURL = "https://player.vimeo.com/video/403508452",
                    ContentType = JourneyContentTypes.Video,
                    Uses = new List<string>() { "Freeboard", "Data Applications", "Data Flow" },
                    Description = @"<p>Fathym enables you to break down your frontend monolith into decoupled micro frontends. Fathym seamlessly orchestrates web components that can be used to build modular and repeatable solutions. </p>
                            <p>Discover how you can adopt a micro frontends architecture for faster, repeatable and independently deliverable frontend development. </p>",
                    Roles = new List<string>(){ "IoT", "Insights" },
                    Active = true,
                    HighlightedOrder = 1,
                    ActionURL = "/freeboard",
                    Details = new JourneyOptionDetails()
                    {
                        Abstract = new List<string>()
                        {
                            @"<p>Micro-frontends is an architectural approach that facilitates breaking down and modularizing frontend monoliths into smaller, decoupled units. These micro frontends can be developed, tested, upgraded and deployed independently, while still contributing to a single cohesive application or product. </p>
                            <p>An application orchestration environment has been set up that will support you in developing your cloud-native solutions. We will walk you through deploying and leveraging existing Low Code Units and how to use the Fathym LCU CLI to create and deliver your first micro-frontends.</p>",
                        },
                        Documentation = new Dictionary<string, string>()
                        {
                            { "Micro Frontends Overview", "https://www.fathym-it.com/framework/docs/getting-started/try-it/micro-frontends" },
                            { "Deploy App", "https://www.fathym-it.com/framework/docs/getting-started/try-it/micro-frontends/deploy-app" },
                            { "Deploy IDE Blade", "https://www.fathym-it.com/framework/docs/getting-started/try-it/micro-frontends/deploy-ide-blade" },
                            { "Customize App", "https://www.fathym-it.com/framework/docs/getting-started/try-it/micro-frontends/customize-app" },
                            { "Create & Deploy LCU", "https://www.fathym-it.com/framework/docs/getting-started/try-it/micro-frontends/create-deploy-custom-lcu" }
                        },
                        RelatedJourneys = new Dictionary<string, string>() {
                            // { "Start Your IoT Journey", "/freeboard" }
                        },
                        Support = new Dictionary<string, string>()
                        {
                            // { "Host an Application", "/freeboard" },
                            // { "View Low-Code Units", "/_lcu" },
                            { "Design Principles", "https://www.fathym-it.com/framework/docs/getting-started/for-devs/design-principals" },
                            { "Real-Time Insights & Monitoring", "/freeboard" },
                            { "Freeboard Documentation", "https://github.com/Freeboard/freeboard" },
                        },
                        SupportConfig = new LazyElementConfig()
                        {
                            Assets = new List<string>() { "/_lcu/lcu-guided-tour-lcu/wc/lcu-guided-tour.lcu.js" },
                            ElementName = "lcu-guided-tour-journey-details-micro-frontend-starter-element"
                        }
                    }
                },
            };

            State.Journeys.AddRange(new List<JourneyOption>()
            {
                new JourneyOption()
                {
                    Name = "Start Your IoT Journey",
                    Lookup = "iot-starter",
                    ContentURL = "https://player.vimeo.com/video/403508452",
                    ContentType = JourneyContentTypes.Video,
                    Uses = new List<string>() { "Freeboard", "Data Applications", "Data Flow" },
                    Description =   @"<p>We have set up a template that enables you to start working with your devices and data immediately.</p>  
                                    <p>Start exploring to see your data visualized in minutes on a best practice cloud infrastructure setup for Azure. Connect with your favorite tools or let us help you explore some new ones. </p>",
                    Roles = new List<string>(){ "IoT", "Insights" },
                    Active = true,
                    HighlightedOrder = 1,
                    ActionURL = "/freeboard",
                    Details = new JourneyOptionDetails()
                    {
                        Abstract = new List<string>()
                        {                                
                            @"<p>Fathym enables users to easily configure an end-to-end best practice cloud infrastructure for streaming emulated or device data in your Azure environment. You can quickly and easily visualize and gain insight from your data. </p>
                                <p>In this template, we have provisioned a best practice emulated data flow for you to get started. This consists of Azure resources, such as IoT Hub, Stream Analytics and Cosmos DB, and connects to Freeboard, an open-source data visualization tool. </p> 
                                <p>Get started with this template and discover how easy it is to configure and provision your own through the resources below.</p>",
                        },
                        Documentation = new Dictionary<string, string>()
                        {
                            { "An Introduction to Data Flows", "https://support.fathym.com/docs/en/an-introduction-to-data-flows" },
                            { "How to Create and Provision Data Flows", "https://support.fathym.com/docs/en/create-and-provision-data-flows" },
                            { "How to Create a Real-Time Data Dashboard using Freeboard", "https://support.fathym.com/docs/en/freeboard" },
                            { "Freeboard Documentation", "https://github.com/Freeboard/freeboard" }
                        },
                        RelatedJourneys = new Dictionary<string, string>() {
                            // { "Micro-Frontends Journey", "/micro-frontends" }
                        },
                        Support = new Dictionary<string, string>()
                        {
                            // { "View Emulated Data in Freeboard", "/freeboard" },
                            // { "View Template Data Flow", "/freeboard" }
                        },
                        SupportConfig = new LazyElementConfig()
                        {
                            Assets = new List<string>() { "/_lcu/lcu-guided-tour-lcu/wc/lcu-guided-tour.lcu.js" },
                            ElementName = "lcu-guided-tour-journey-details-iot-starter-element"
                        }
                    }
                },
                // new JourneyOption()
                // {
                //     Name = "Mean, Median and Standard Deviation with Machine Learning",
                //     Lookup = "iot-elementary-statistics",
                //     ContentURL = "https://player.vimeo.com/video/403508452",
                //     ContentType = JourneyContentTypes.Video,
                //     Uses = new List<string>() { "Machine Learning", "Data Flow", "Data Applications" },
                //     Description = "Calculate Elementary Statistics on your stream of IoT data.",
                //     Roles = new List<string>(){ "IoT", "Data Science", "Insights" },
                //     Active = true,
                //     HighlightedOrder = 2,
                //     ActionURL = "https://fathym.com",
                //     Details = new JourneyOptionDetails()
                //     {
                //         Abstract = new List<string>()
                //         {
                //             "Multiple lines of descriptive...",
                //             "Paragraphs of text"
                //         },
                //         Documentation = new Dictionary<string, string>()
                //         {
                //             { "Links to", "https://fathym.com" },
                //             { "Different types", "https://fathym.com" },
                //             { "of Documentation", "https://fathym.com" }
                //         },
                //         RelatedJourneys = new Dictionary<string, string>() {
                //             { "Titles for journeys", "the-journey-lookup" },
                //             { "Titles for journeys2", "the-journey-lookup" },
                //             { "Titles for journeys3", "the-journey-lookup" }
                //         },
                //         Support = new Dictionary<string, string>()
                //         {
                //             { "Links to", "https://fathym.com" },
                //             { "Different types", "https://fathym.com" },
                //             { "Of Support Articles", "https://fathym.com" },
                //             { "Tutorials and More", "https://fathym.com" }
                //         }
                //     }
                // },
                // new JourneyOption()
                // {
                //     Name = "ABB Device Notifications and Resolution",
                //     Lookup = "iot-notifications-resolution",
                //     ContentURL = "https://player.vimeo.com/video/403508452",
                //     ContentType = JourneyContentTypes.Video,
                //     Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                //     Description = "Receive notifications around configured threshholds and resolve them once addressed.",
                //     Roles = new List<string>(){ "IoT", "Insights" },
                //     Active = true,
                //     HighlightedOrder = 3,
                //     ActionURL = "https://fathym.com",
                //     Details = new JourneyOptionDetails()
                //     {
                //         Abstract = new List<string>()
                //         {
                //             "Multiple lines of descriptive...",
                //             "Paragraphs of text"
                //         },
                //         Documentation = new Dictionary<string, string>()
                //         {
                //             { "Links to", "https://fathym.com" },
                //             { "Different types", "https://fathym.com" },
                //             { "of Documentation", "https://fathym.com" }
                //         },
                //         RelatedJourneys = new Dictionary<string, string>() {
                //             { "Titles for journeys", "the-journey-lookup" },
                //             { "Titles for journeys2", "the-journey-lookup" },
                //             { "Titles for journeys3", "the-journey-lookup" }
                //         },
                //         Support = new Dictionary<string, string>()
                //         {
                //             { "Links to", "https://fathym.com" },
                //             { "Different types", "https://fathym.com" },
                //             { "Of Support Articles", "https://fathym.com" },
                //             { "Tutorials and More", "https://fathym.com" }
                //         }
                //     }
                // },
                // new JourneyOption()
                // {
                //     Name = "Augment your Data with Real-Time Surface Forecast",
                //     Lookup = "iot-real-time-surface-forecast",
                //     ContentURL = "https://player.vimeo.com/video/403508452",
                //     ContentType = JourneyContentTypes.Video,
                //     Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                //     Description = "Receive notifications around configured threshholds and resolve them once addressed.",
                //     Roles = new List<string>(){ "IoT", "Insights" },
                //     Active = true,
                //     HighlightedOrder = 3,
                //     ActionURL = "https://fathym.com",
                //     Details = new JourneyOptionDetails()
                //     {
                //         Abstract = new List<string>()
                //         {
                //             "Multiple lines of descriptive...",
                //             "Paragraphs of text"
                //         },
                //         Documentation = new Dictionary<string, string>()
                //         {
                //             { "Links to", "https://fathym.com" },
                //             { "Different types", "https://fathym.com" },
                //             { "of Documentation", "https://fathym.com" }
                //         },
                //         RelatedJourneys = new Dictionary<string, string>() {
                //             { "Titles for journeys", "the-journey-lookup" },
                //             { "Titles for journeys2", "the-journey-lookup" },
                //             { "Titles for journeys3", "the-journey-lookup" }
                //         },
                //         Support = new Dictionary<string, string>()
                //         {
                //             { "Links to", "https://fathym.com" },
                //             { "Different types", "https://fathym.com" },
                //             { "Of Support Articles", "https://fathym.com" },
                //             { "Tutorials and More", "https://fathym.com" }
                //         }
                //     }
                // },
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
            });
        }

        public virtual async Task MoreDetails(string journeyLookup)
        {
            State.CurrentJourneyLookup = journeyLookup;
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
