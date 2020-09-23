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
                    Lookup = "iot-edge-beyond",
                    ContentURL = "https://player.vimeo.com/video/403508452",
                    ContentType = JourneyContentTypes.Video,
                    Uses = new List<string>() { "Devices", "Data Flow", "Data Science" },
                    Description = "Build and connect edge devices, securely manage, visualize and analyze your data, and take action on your intelligence.",
                    Roles = new List<string>(){ "IoT", "Data Science" },
                    ActionURL = "https://fathym.com",
                    Active = true,
                    Details = new JourneyOptionDetails()
                    {
                        Abstract = new List<string>()
                        {
                            "Multiple lines of descriptive...",
                            "Paragraphs of text"
                        },
                        Documentation = new Dictionary<string, string>()
                        {
                            { "Links to", "https://fathym.com" },
                            { "Different types", "https://fathym.com" },
                            { "of Documentation", "https://fathym.com" }
                        },
                        RelatedJourneys = new Dictionary<string, string>() {
                            { "Titles for journeys", "the-journey-lookup" },
                            { "Titles for journeys2", "the-journey-lookup" },
                            { "Titles for journeys3", "the-journey-lookup" }
                        },
                        Support = new Dictionary<string, string>()
                        {
                            { "Links to", "https://fathym.com" },
                            { "Different types", "https://fathym.com" },
                            { "Of Support Articles", "https://fathym.com" },
                            { "Tutorials and More", "https://fathym.com" }
                        },
                        SupportConfig = new LazyElementConfig()
                        {
                            Assets = new List<string>() { "/_lcu/lcu-guided-tour-lcu/wc/lcu-guided-tour.lcu.js" },
                            ElementName = "lcu-guided-tour-journey-details-iot-edge-beyond-element"
                        }
                    }
                },
                new JourneyOption()
                {
                    Name = "Start Your Micro-Frontend Journey",
                    Lookup = "micro-frontends",
                    ContentURL = "https://player.vimeo.com/video/403508452",
                    ContentType = JourneyContentTypes.Video,
                    Uses = new List<string>() { "Freeboard", "Data Applications", "Data Flow" },
                    Description = @"<p>An application orchestration environment has been setup that will support you in developing your cloud-native solutions. In this part of the guide, we walk you through deploying and leveraging existing Low Code Units and how to use the Fathym LCU CLI to create and deliver your first micro-frontends.</p>
                    <p>Read the <a href=""https://fathym-it.com/framework/docs/getting-started/try-it/micro-frontends"">full docs</a> on Fathym Framework micro-frontends now.</p>",
                    Roles = new List<string>(){ "IoT", "Insights" },
                    Active = true,
                    HighlightedOrder = 1,
                    ActionURL = "/freeboard",
                    Details = new JourneyOptionDetails()
                    {
                        Abstract = new List<string>()
                        {
                            @"Some more descriptive text",
                            @"Even more descriptive text..."
                        },
                        Documentation = new Dictionary<string, string>()
                        {
                            { "Using Freeboard with Fathym", "https://fathym.com" },
                            { "Configuring Freeboard with Custom Dashboard", "https://fathym.com" },
                            { "Deliver Open Source Tools on Fathym", "https://fathym.com" }
                        },
                        RelatedJourneys = new Dictionary<string, string>() {
                            { "IoT - To the Edge and Beyond", "iot-edge-beyond" }
                        },
                        Support = new Dictionary<string, string>()
                        {
                            { "Real-Time Insights & Monitoring", "/freeboard" },
                            { "A blog about freeboard on fathym", "https://fathym.com" },
                            { "Another blog about freeboard on fathym", "https://fathym.com" },
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
                    Description = @"<p>
                    We have setup a few things automatically for you. This
                    will allow you to start working with your devices and data
                    immediately. Start exploring to see your data visualized
                    in minutes on a best practice cloud setup for Azure.
                </p>

                <p>
                    Your data is ready to begin using. Connect it with your
                    favorite tools or let us help you walk through learning
                    some new ones.
                </p>",
                    Roles = new List<string>(){ "IoT", "Insights" },
                    Active = true,
                    HighlightedOrder = 1,
                    ActionURL = "/freeboard",
                    Details = new JourneyOptionDetails()
                    {
                        Abstract = new List<string>()
                        {
                            @"We have setup a few things automatically for you. This
                    will allow you to start working with your devices and data
                    immediately. Start exploring to see your data visualized
                    in minutes on a best practice cloud setup for Azure.",
                            @"Your data is ready to begin using. Connect it with your
                    favorite tools or let us help you walk through learning
                    some new ones."
                        },
                        Documentation = new Dictionary<string, string>()
                        {
                            { "Using Freeboard with Fathym", "https://fathym.com" },
                            { "Configuring Freeboard with Custom Dashboard", "https://fathym.com" },
                            { "Deliver Open Source Tools on Fathym", "https://fathym.com" }
                        },
                        RelatedJourneys = new Dictionary<string, string>() {
                            { "Titles for journeys", "the-journey-lookup" },
                            { "Titles for journeys2", "the-journey-lookup" },
                            { "Titles for journeys3", "the-journey-lookup" }
                        },
                        Support = new Dictionary<string, string>()
                        {
                            { "Real-Time Insights & Monitoring", "/freeboard" },
                            { "A blog about freeboard on fathym", "https://fathym.com" },
                            { "Another blog about freeboard on fathym", "https://fathym.com" },
                            { "Freeboard Documentation", "https://github.com/Freeboard/freeboard" },
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
