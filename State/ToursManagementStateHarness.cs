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
using LCU.Personas.Client.Identity;

namespace LCU.State.API.NapkinIDE.NapkinIDE.ToursManagement.State
{
    public class ToursManagementStateHarness : LCUStateHarness<ToursManagementState>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ToursManagementStateHarness(ToursManagementState state)
            : base(state ?? new ToursManagementState())
        { }
        #endregion

        #region API Methods
        public virtual void LoadGuidedTours()
        {
            State.Tours = new List<GuidedTour>();

            State.Tours.Add(createDemoTour("demo-tour"));

            State.Tours.Add(createLimitedTrialTour("limited-trial-tour"));

            State.Tours.Add(createDataApplicationsTour("data-applications-tour"));

            State.Tours.Add(createDataFlowManagementTour("data-flow-management-tour"));

            State.Tours.Add(createDataFlowToolTour("data-flow-tool-tour"));
            
            State.Tours.Add(createIoTDeveloperJourneyTour("iot-developer-journey-tour"));

            State.Tours.Add(createProWelcomeTour("pro-welcome-tour"));
            
            State.Tours.Add(createProDataApplicationsTour("pro-data-applications-tour"));
            
            State.Tours.Add(createProDataFlowTour("pro-data-flow-tour"));
            
            State.Tours.Add(createProSettingsTour("pro-settings-tour"));
        }

        public virtual void RecordStep(string tourLookup, string currentStep, bool isComplete)
        {
            if (State.StepRecords.IsNullOrEmpty())
                State.StepRecords = new Dictionary<string, GuidedTourStepRecord>();

            if (!State.StepRecords.ContainsKey(tourLookup))
                State.StepRecords[tourLookup] = new GuidedTourStepRecord() { StepHistory = new List<string>() };

            State.StepRecords[tourLookup].CurrentStep = currentStep;

            State.StepRecords[tourLookup].StepHistory.Add(State.StepRecords[tourLookup].CurrentStep);

            if (isComplete)
            {
                if (State.CompletedTourLookups.IsNullOrEmpty())
                    State.CompletedTourLookups = new Dictionary<string, string>();

                if (State.StepRecordHistory.IsNullOrEmpty())
                    State.StepRecordHistory = new Dictionary<string, List<GuidedTourStepRecord>>();

                if (!State.StepRecordHistory.ContainsKey(tourLookup))
                    State.StepRecordHistory[tourLookup] = new List<GuidedTourStepRecord>();

                State.CompletedTourLookups[tourLookup] = currentStep;

                State.StepRecordHistory[tourLookup].Add(State.StepRecords[tourLookup]);

                State.StepRecords.Remove(tourLookup);
            }
        }

        public virtual async Task Reset(IdentityManagerClient idMgr, string entApiKey, string username)
        {
            State = new ToursManagementState();

            await RefreshTours(idMgr, entApiKey, username);
        }

        public virtual async Task RefreshTours(IdentityManagerClient idMgr, string entApiKey, string username)
        {
            LoadGuidedTours();

            await SetToursEnabled(idMgr, entApiKey, username);

            if (State.CurrentTour == null && !State.ToursEnabled)
                State.CurrentTour = State.Tours.FirstOrDefault(tour => tour.Lookup == "limited-trial-tour");
        }

        public virtual async Task SetActiveTour(string entApiKey, string lookup)
        {
            State.CurrentTour = State.Tours.FirstOrDefault(tour => tour.Lookup == lookup);
        }

        public virtual async Task SetToursEnabled(IdentityManagerClient idMgr, string entApiKey, string username)
        {
            var authResp = await idMgr.HasAccess(entApiKey, username, new List<string>() { "LCU.NapkinIDE.AllAccess" });

            State.ToursEnabled = !authResp.Status;
        }
        #endregion

        #region Helpers
        protected virtual GuidedTour createCopyForNewTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-0000000000**"), // Change and ensure Guid is unique to all other tours, by replacing ** with next value (01, 02, 03...11, 12...)
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        Title = "LCU-Guided-Tour",
                        Subtitle = "Guided Tour",
                        Lookup = "welcome",
                        Content = "Welcome to the LCU-Guided-Tour library! This library provides the functionality to do your own guided tour of an application. <br/><br/> Click the <b>Next</b> button to get started with an example Tour!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Title",
                        Subtitle = "Guided Tour",
                        Selector = "#guidedTourHeader",
                        Orientation = OrientationTypes.Bottom,
                        Lookup = "some-content",
                        Content = "This be some content"
                    },
                }
            };
        }

        protected virtual GuidedTour createDemoTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        Title = "LCU-Guided-Tour",
                        Subtitle = "Guided Tour",
                        Lookup = "welcome",
                        Content = "Welcome to the LCU-Guided-Tour library! This library provides the functionality to do your own guided tour of an application. <br/><br/> Click the <b>Next</b> button to get started with an example Tour!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Title",
                        Subtitle = "Guided Tour",
                        Selector = "#guidedTourHeader",
                        Orientation = OrientationTypes.Bottom,
                        Lookup = "header",
                        Content = "With the LCU-Guided-Tour, you can select anything that is on the screen that has a valid CSS selector.  For example, you can select this title, which as an id of <b>#guidedTourHeader</b>. <br/><br/> Valid selectors are as follows: <ul><li>.class</li><li>#id</li><li>element</li></ul>"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Complex Selectors",
                        Subtitle = "Guided Tour",
                        Selector = ".section:nth-of-type(2) .mat-radio-button:nth-child(3)",
                        Orientation = OrientationTypes.Right,
                        Lookup = "specific",
                        Content = "You can even target more specific, complex elements, by using various built-in CSS selectors. In this case, we are targeting the third radio item in the second section with the selector of: <br/> <b>.section:nth-of-type(2) .mat-radio-button:nth-child(3)</b>"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Modifiers",
                        Subtitle = "Guided Tour",
                        Selector = "#formBox",
                        Orientation = OrientationTypes.Right,
                        Lookup = "customize",
                        Content = "As for the bot, you can modify certain properties of it in order to customize it to your needs. Here we can change the position it lives on the screen, the container it should position itself in, as well as the amount of padding we would like to have between the bot and the container."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Bounding Container",
                        Subtitle = "Guided Tour",
                        Selector = "#boundingBox",
                        Orientation = OrientationTypes.Left,
                        Lookup = "inside",
                        Content = "As an example, you can set the Bot to be positioned inside this box by setting the container to the <b>#boundingBox</b> selector."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Assigning Actions",
                        Subtitle = "Guided Tour",
                        Selector = ".mat-tab-label:nth-of-type(2)",
                        Orientation = OrientationTypes.BottomLeft,
                        Lookup = "assign",
                        Content = "You can assign each step an action as well, in case you want to run logic before or after a step is displayed.  Click <b>Next</b> to see this in action!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Tab Movement",
                        Subtitle = "Guided Tour",
                        Selector = "#boxLogoForm",
                        Orientation = OrientationTypes.BottomLeft,
                        Lookup = "see",
                        Content = "As you can see, this tab was selected so that the Tour could continue after the DOM has rendered a different view.  You can also use the <b>actionDelay</b> property to specify a time delay before showing the next step, in order to properly render the next view."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Tour Complete",
                        Subtitle = "Guided Tour",
                        Selector = "#boxMiscForm",
                        Orientation = OrientationTypes.Right,
                        Lookup = "complete",
                        Content = "Congratulations! You have successfully completed a Tour. You can click the 'Finish' button to complete the tour, or you can click the 'Back' button to go back and view any previous steps you want to review again."
                    }
                }
            };
        }

        protected virtual GuidedTour createLimitedTrialTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000002"),
                IsFirstTimeViewing = true,
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000020"),
                        Title = "Welcome",
                        Subtitle = "Limited Trial Tour",
                        Lookup = "welcome",
                        Content = "Welcome to the live demo of the Fathym Low-Code Framework. I’m Thinky! I’ll guide you through a few tours to show you some of Fathym’s low-code tools, such as the Data Flow Manager and Data Applications, and I’ll explain how to interact and dig deeper with the tools so you can customize them for your needs."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000021"),
                        Title = "Welcome Page Resources",
                        Subtitle = "Limited Trial Tour",
                        Selector = "lcu-limited-trial-welcome-element > .welcome-container",
                        Orientation = OrientationTypes.Left,
                        Lookup = "journeys",
                        Content = "Here are a number of resources and guided tours you can select that will educate you of the different developer journeys you can undertake through Fathym."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000022"),
                        Title = "Data Flows",
                        Subtitle = "Limited Trial Tour",
                        Selector = "nide-ide-side-bar .ide-side-bar-action:nth-of-type(2)",
                        Orientation = OrientationTypes.Right,
                        Lookup = "data-flow",
                        Content = "The Data Flow Manager is a powerful drag and drop interface for easily configuring and provisioning end-to-end cloud infrastructure. Navigate here to explore further."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000023"),
                        Title = "Data Applications",
                        Subtitle = "Limited Trial Tour",
                        Selector = "nide-ide-side-bar .ide-side-bar-action:nth-of-type(3)",
                        Orientation = OrientationTypes.Right,
                        Lookup = "data-apps",
                        Content = "In Data Applications you can host, manage and deploy data apps that integrate automatically with NPM packages and GitHub repositories. Navigate here to explore further."
                    }
                }
            };
        }

        protected virtual GuidedTour createDataApplicationsTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000003"),
                IsFirstTimeViewing = true,
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000030"),
                        Title = "Data Applications",
                        Subtitle = "Data Applications Tour",
                        Selector = "lcu-limited-trial-data-apps-element .lcu-data-apps-config-manager",
                        Orientation = OrientationTypes.Left,
                        Lookup = "welcome",
                        Content = "Data applications are quick and easy ways to host, manage and deploy scalable web apps and sites, easily managed for multiple end users."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000031"),
                        Title = "Public/Private Apps Sidebar",
                        Subtitle = "Data Applications Tour",
                        Selector = "lcu-limited-trial-data-apps-element .mat-drawer-inner-container",
                        Orientation = OrientationTypes.Left,
                        Lookup = "apps",
                        Content = "Access your applications here. The examples provided are Hello World, Fathym Forecast and Trial Dashboard. Applications can be hosted as either public or private. Set your application to private if you want to control access for internal use or set to public if you want to share it with external users without a login. You can update visibility settings at any point."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000032"),
                        Title = "View Configuration",
                        Subtitle = "Data Applications Tour",
                        Selector = "lcu-limited-trial-data-apps-element lcu-data-apps-config .mat-tab-group .mat-tab-label:nth-of-type(1)",
                        Orientation = OrientationTypes.Left,
                        Lookup = "view-config",
                        Content = "Here you can view the configuration and version history of your application. Fathym leverages NPM node packages to organize and deploy any previous or current version of your application to your custom domain."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000033"),
                        Title = "Application Details",
                        Subtitle = "Data Applications Tour",
                        Selector = "lcu-limited-trial-data-apps-element lcu-data-apps-config .mat-tab-group .mat-tab-label:nth-of-type(2)",
                        Orientation = OrientationTypes.Left,
                        Lookup = "app-config",
                        Content = "Here you can view the name, description and path of the application. The path is the URL where your application is hosted."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000034"),
                        Title = "Add New Data App",
                        Subtitle = "Data Applications Tour",
                        Selector = "lcu-limited-trial-data-apps-element #createNewDataAppBtn",
                        Orientation = OrientationTypes.Left,
                        Lookup = "create",
                        Content = "Create and configure your own data app or use pre-existing applications. Fathym has several open source data apps to get you started."
                    }
                }
            };
        }

        protected virtual GuidedTour createDataFlowManagementTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000004"),
                IsFirstTimeViewing = true,
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000040"),
                        Title = "Data Flow Management",
                        Subtitle = "Data Flow Management Tour",
                        Selector = "lcu-limited-trial-data-flow-element .data-flow-manager-container",
                        Orientation = OrientationTypes.Left,
                        Lookup = "welcome",
                        Content = "Developers can easily manage and create data flows by rapidly configuring and provisioning Azure resources through a visual drag-and-drop interface."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000041"),
                        Title = "Emulated Data Flows",
                        Subtitle = "Data Flow Management Tour",
                        Selector = "lcu-limited-trial-data-flow-element lcu-data-flow-list-element .mat-tab-label:nth-of-type(1)",
                        Orientation = OrientationTypes.Bottom,
                        Lookup = "best-practice",
                        Content = "We have created a sample best practice IoT environment for you to explore using an emulated data flow."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000042"),
                        Title = "Trial Data Flows",
                        Subtitle = "Data Flow Management Tour",
                        Selector = "lcu-limited-trial-data-flow-element lcu-data-flow-list-element .mat-tab-label:nth-of-type(2)",
                        Orientation = OrientationTypes.Bottom,
                        Lookup = "explore",
                        Content = "Use our drag-and-drop interface to explore the tool and connect dummy Azure resources."
                    }
                }
            };
        }

        protected virtual GuidedTour createDataFlowToolTour(string lookup)
        {
            State.EnvironmentLookup = Environment.GetEnvironmentVariable("EnvironmentLookup");

            if(State.EnvironmentLookup == "limited-lcu-int"){
                return new GuidedTour()
                {
                    ID = new Guid("00000000-0000-0000-0000-000000000005"),
                    IsFirstTimeViewing = true,
                    Lookup = lookup,
                    UseOrb = false,
                    Steps = new List<GuidedTourStep>()
                    {
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000050"),
                            Title = "Emulator",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='e7457c9c-c9b2-4955-b0a2-330b6244982d']", // selects by attribute selector
                            Orientation = OrientationTypes.Top,
                            Lookup = "emulator",
                            Content = "The emulator is where you can configure your test device data and the frequency that it posts to the ingest. This is ideal for getting data streaming through your IoT infrastructure and into business applications and dashboards, while your hardware team is working to get the actual devices online. Once the real devices are online you can turn off the emulator."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000051"),
                            Title = "Ingest",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='f0e0b225-5e51-44c2-8618-a48a0d7678de']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "ingest",
                            Content = "The ingest is a security-enhanced communication channel for sending and receiving data from your devices or an emulator."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000052"),
                            Title = "Data Map",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='2bb21cb0-37db-4e6e-a762-ab5b1ea3c974']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "data-map",
                            Content = "The data map is a real-time analytics service that allows you to manipulate and analyze your data before pushing to the configured outputs."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000053"),
                            Title = "Cold Storage",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='8a3fe2f2-d7a3-43f5-b8c7-cf87c6691422']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "cold-storage",
                            Content = "This storage option costs less than $1/month. You can send raw, untouched JSON messages from a device to cold storage. This allows you to refer to cold storage for debugging when you need to see the exact messages that were sent from a device."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000054"),
                            Title = "Warm Storage",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='038131c4-57a9-443c-90ff-d683816c2c40']",
                            Orientation = OrientationTypes.Left,
                            Lookup = "warm-storage",
                            Content = "This storage option is more expensive, starting at $25/month. Warm storage is a database, like CosmosDB or SQL Server. It's queryable storage that is ideal for connecting to reporting solutions like Power BI. Fathym helps you convert units (like Celsius to Fahrenheit) before storing in warm storage so the data is ready for downstream use."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000055"),
                            Title = "Hot Storage",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='0392f943-577e-4165-acb8-93c70074c04f']",
                            Orientation = OrientationTypes.Left,
                            Lookup = "hot-storage",
                            Content = "This storage option is also inexpensive, normally less than $1/month. Hot storage is PubSub – Publisher/Subscriber relationships. A publisher application creates and sends messages to a topic. Subscriber applications create a subscription to a topic to receive messages from it. Fathym uses hot storage for real-time sensor dashboards."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000056"),
                            Title = "Warm Query",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='3ccb861c-57f6-44a0-9430-13c68fb19055']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "warm-query",
                            Content = "Depending on the dashboard and reporting tools you’re using, you may not be able to connect directly to the warm storage. If that’s the case, the warm query provides an API endpoint that you can use to get access to the data in warm storage."
                        }
                    }
            };
            }

            else {
                return new GuidedTour()
                {
                    ID = new Guid("00000000-0000-0000-0000-000000000005"),
                    IsFirstTimeViewing = true,
                    Lookup = lookup,
                    UseOrb = false,
                    Steps = new List<GuidedTourStep>()
                    {
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000050"),
                            Title = "Emulator",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='111fb8c3-0e30-46de-a61c-b78fe1b9d5dd']", // selects by attribute selector
                            Orientation = OrientationTypes.Top,
                            Lookup = "emulator",
                            Content = "The emulator is where you can configure your test device data and the frequency that it posts to the ingest. This is ideal for getting data streaming through your IoT infrastructure and into business applications and dashboards, while your hardware team is working to get the actual devices online. Once the real devices are online you can turn off the emulator."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000051"),
                            Title = "Ingest",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='39240064-dca2-4a18-9377-777d0e4d29db']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "ingest",
                            Content = "The ingest is a security-enhanced communication channel for sending and receiving data from your devices or an emulator."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000052"),
                            Title = "Data Map",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='4709ff32-3bd8-4535-950d-02518fa61d7f']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "data-map",
                            Content = "The data map is a real-time analytics service that allows you to manipulate and analyze your data before pushing to the configured outputs."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000053"),
                            Title = "Cold Storage",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='8eb91baf-d4a4-4b9b-b941-3c05bc5cbbd0']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "cold-storage",
                            Content = "This storage option costs less than $1/month. You can send raw, untouched JSON messages from a device to cold storage. This allows you to refer to cold storage for debugging when you need to see the exact messages that were sent from a device."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000054"),
                            Title = "Warm Storage",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='b123dda9-788d-47eb-8698-7d9c80817492']",
                            Orientation = OrientationTypes.Left,
                            Lookup = "warm-storage",
                            Content = "This storage option is more expensive, starting at $25/month. Warm storage is a database, like CosmosDB or SQL Server. It's queryable storage that is ideal for connecting to reporting solutions like Power BI. Fathym helps you convert units (like Celsius to Fahrenheit) before storing in warm storage so the data is ready for downstream use."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000055"),
                            Title = "Hot Storage",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='42877965-0bed-4edf-9b99-e308a856c839']",
                            Orientation = OrientationTypes.Left,
                            Lookup = "hot-storage",
                            Content = "This storage option is also inexpensive, normally less than $1/month. Hot storage is PubSub – Publisher/Subscriber relationships. A publisher application creates and sends messages to a topic. Subscriber applications create a subscription to a topic to receive messages from it. Fathym uses hot storage for real-time sensor dashboards."
                        },
                        new GuidedTourStep()
                        {
                            ID = new Guid("00000000-0000-0000-0000-000000000056"),
                            Title = "Warm Query",
                            Subtitle = "Emulated Data Flow Tour",
                            Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='5708f3b6-ecc0-4aed-9bcd-f389d8720c73']",
                            Orientation = OrientationTypes.Top,
                            Lookup = "warm-query",
                            Content = "Depending on the dashboard and reporting tools you’re using, you may not be able to connect directly to the warm storage. If that’s the case, the warm query provides an API endpoint that you can use to get access to the data in warm storage."
                        }
                    }
            };
            }
        }

        protected virtual GuidedTour createDataFlowToolTourProd(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000005"),
                IsFirstTimeViewing = true,
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000050"),
                        Title = "Emulator",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='111fb8c3-0e30-46de-a61c-b78fe1b9d5dd']", // selects by attribute selector
                        Orientation = OrientationTypes.Top,
                        Lookup = "emulator",
                        Content = "The emulator is where you can configure your test device data and the frequency that it posts to the ingest. This is ideal for getting data streaming through your IoT infrastructure and into business applications and dashboards, while your hardware team is working to get the actual devices online. Once the real devices are online you can turn off the emulator."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000051"),
                        Title = "Ingest",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='39240064-dca2-4a18-9377-777d0e4d29db']",
                        Orientation = OrientationTypes.Top,
                        Lookup = "ingest",
                        Content = "The ingest is a security-enhanced communication channel for sending and receiving data from your devices or an emulator."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000052"),
                        Title = "Data Map",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='4709ff32-3bd8-4535-950d-02518fa61d7f']",
                        Orientation = OrientationTypes.Top,
                        Lookup = "data-map",
                        Content = "The data map is a real-time analytics service that allows you to manipulate and analyze your data before pushing to the configured outputs."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000053"),
                        Title = "Cold Storage",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='8eb91baf-d4a4-4b9b-b941-3c05bc5cbbd0']",
                        Orientation = OrientationTypes.Top,
                        Lookup = "cold-storage",
                        Content = "This storage option costs less than $1/month. You can send raw, untouched JSON messages from a device to cold storage. This allows you to refer to cold storage for debugging when you need to see the exact messages that were sent from a device."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000054"),
                        Title = "Warm Storage",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='b123dda9-788d-47eb-8698-7d9c80817492']",
                        Orientation = OrientationTypes.Left,
                        Lookup = "warm-storage",
                        Content = "This storage option is more expensive, starting at $25/month. Warm storage is a database, like CosmosDB or SQL Server. It's queryable storage that is ideal for connecting to reporting solutions like Power BI. Fathym helps you convert units (like Celsius to Fahrenheit) before storing in warm storage so the data is ready for downstream use."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000055"),
                        Title = "Hot Storage",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='42877965-0bed-4edf-9b99-e308a856c839']",
                        Orientation = OrientationTypes.Left,
                        Lookup = "hot-storage",
                        Content = "This storage option is also inexpensive, normally less than $1/month. Hot storage is PubSub – Publisher/Subscriber relationships. A publisher application creates and sends messages to a topic. Subscriber applications create a subscription to a topic to receive messages from it. Fathym uses hot storage for real-time sensor dashboards."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000056"),
                        Title = "Warm Query",
                        Subtitle = "Emulated Data Flow Tour",
                        Selector = "lcu-limited-trial-data-flow-element .flowchart-object[data-jtk-node-id='5708f3b6-ecc0-4aed-9bcd-f389d8720c73']",
                        Orientation = OrientationTypes.Top,
                        Lookup = "warm-query",
                        Content = "Depending on the dashboard and reporting tools you’re using, you may not be able to connect directly to the warm storage. If that’s the case, the warm query provides an API endpoint that you can use to get access to the data in warm storage."
                    }
                }
            };
        }

        protected virtual GuidedTour createIoTDeveloperJourneyTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000006"),
                IsFirstTimeViewing = true,
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000060"),
                        Title = "IoT Developer Journey",
                        Subtitle = "To the Edge and Beyond",
                        Lookup = "iot-journey",
                        Content = "In 6 steps, I’ll guide you how on IoT developers can use the Fathym Low-Code Framework to rapidly provision end-to-end IoT infrastructure and build enterprise scale IoT solutions."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000061"),
                        Title = "Data Flow Manager",
                        Subtitle = "To the Edge and Beyond",
                        Selector = "lcu-limited-trial-data-flow-element .data-flow-manager-container",
                        Orientation = OrientationTypes.Left,
                        Lookup = "data-flow",
                        Content = "The data flow manager is a powerful drag and drop interface for easily configuring and provisioning end-to-end IoT infrastructure."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000062"),
                        Title = "Emulated IoT Data Flow",
                        Subtitle = "To the Edge and Beyond",
                        Selector = "lcu-limited-trial-data-flow-element .data-flow-ide-container",
                        Orientation = OrientationTypes.Left,
                        Lookup = "emulated",
                        Content = "Here is an emulated data flow that demonstrates one of our best practice IoT environments. Using our drag and drop interface, you can create emulated data functions and connect them to data streams and data maps. The data map allows you to manipulate and analyze your data before pushing to various configured storage outputs."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000063"),
                        Title = "Data Applications",
                        Subtitle = "To the Edge and Beyond",
                        Selector = "lcu-limited-trial-data-apps-element .lcu-data-apps-config-manager",
                        Orientation = OrientationTypes.Left,
                        Lookup = "data-apps",
                        Content = "Data applications enable you to build and deliver powerful data-driven web applications and sites that are easily managed for multiple end users."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000064"),
                        Title = "Hosting your Application",
                        Subtitle = "To the Edge and Beyond",
                        Selector = "lcu-data-apps-config #dataAppsConfigCard",
                        Orientation = OrientationTypes.Left,
                        Lookup = "hosting",
                        Content = "You can host and build data apps that integrate automatically with NPM packages and GitHub repositories, enabling you to organize and deploy any previous or current version of your application to your custom domain."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000065"),
                        Title = "Access Control",
                        Subtitle = "To the Edge and Beyond",
                        Selector = "lcu-data-apps-config #accessControlToggle",
                        Orientation = OrientationTypes.Top,
                        Lookup = "access",
                        Content = "Set your application to private if you want to control access for internal use or set to public if you want to share it with external users without a login. You can update visibility settings at any point."
                    }
                }
            };
        }

        protected virtual GuidedTour createProWelcomeTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000007"),
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000070"),
                        Title = "Welcome",
                        Subtitle = "Welcome Tour",
                        Lookup = "welcome",
                        Content = "Welcome to the Fathym Low-Code Framework. I’m Thinky! I’ll guide you through a few tours to show you some of Fathym’s low-code tools and I'll explain how to interact with the tools and dig deeper so you can customize them for your needs."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000071"),
                        Title = "Low-Code Unit™",
                        Subtitle = "Welcome Tour",
                        Lookup = "low-code-unit",
                        Content = "Each element of the Fathym Low-Code Framework is made up of Low-Code Units – modular and reusable building blocks of code that can be as large as an application, or as small as a data visualization. You can modify our data flow and data application Low-Code Units or create and upload your own."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000072"),
                        Title = "Core",
                        Subtitle = "Welcome Tour",
                        Lookup = "core",
                        Selector = "#coreActivityLink",
                        Orientation = OrientationTypes.Right,
                        Content = "In Core you can host, manage and deploy data applications."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000073"),
                        Title = "Data Flows",
                        Subtitle = "Welcome Tour",
                        Lookup = "data-flows",
                        Selector = "#dataFlowActivityLink",
                        Orientation = OrientationTypes.Right,
                        Content = "The data flow manager is a powerful drag and drop interface for easily configuring and provisioning end-to-end cloud infrastructure."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000074"),
                        Title = "Settings",
                        Subtitle = "Welcome Tour",
                        Lookup = "settings",
                        Selector = "#settingsLink",
                        Orientation = OrientationTypes.TopLeft,
                        Content = "In settings you can customize the IDE to suit your needs. Modify or upload Low-Code Units and configure your IDE activity bar and side bar."
                    }
                }
            };
        }

        protected virtual GuidedTour createProDataApplicationsTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000008"),
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000080"),
                        Title = "Data Applications",
                        Subtitle = "Data Applications Tour",
                        Lookup = "data-apps",
                        Content = "Data applications are quick and easy ways to host, manage and deploy scalable web apps and sites, easily managed for multiple end users."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000081"),
                        Title = "NPM node packages",
                        Subtitle = "Data Applications Tour",
                        Lookup = "npm-packages",
                        Content = "Fathym leverages NPM node packages to organize and deploy any previous or current version of your application to your custom domain."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000082"),
                        Title = "Secure Application",
                        Subtitle = "Data Applications Tour",
                        Lookup = "secure",
                        Selector = "#isSecuredAppSlideToggle",
                        Orientation = OrientationTypes.Top,
                        Content = "The default setting of an application is public, but you can easily secure your application and control access rights."
                    }
                }
            };
        }

        protected virtual GuidedTour createProDataFlowTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000009"),
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000090"),
                        Title = "No-Code Data Flow",
                        Subtitle = "Data Flow Tour",
                        Lookup = "no-code-data-flow",
                        Content = "Use our drag and drop interface to easily configure the following Azure resources."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000091"),
                        Title = "Data Emulator",
                        Subtitle = "Data Flow Tour",
                        Lookup = "data-emulator",
                        Selector = "#dataEmulatorModuleOption",
                        Orientation = OrientationTypes.Left,
                        Content = "The emulator is where you can configure your test device data and the frequency that it posts to the ingest. This is ideal for getting data streaming through your IoT infrastructure and into business applications and dashboards, while your hardware team is working to get the actual devices online. Once the real devices are online you can turn off the emulator."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000092"),
                        Title = "Data Stream",
                        Subtitle = "Data Flow Tour",
                        Lookup = "data-stream",
                        Selector = "#dataStreamModuleOption",
                        Orientation = OrientationTypes.Left,
                        Content = "The data stream is designed for large scale data ingestion, from either an IoT enabled device or from an application. A Data Stream is only one-way communication; it can’t talk back to the device that is sending the information."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000093"),
                        Title = "Device Stream",
                        Subtitle = "Data Flow Tour",
                        Lookup = "device-stream",
                        Selector = "#deviceStreamModuleOption",
                        Orientation = OrientationTypes.Left,
                        Content = "The device stream is also designed for large scale data ingestion, from either an IoT enabled device or from an application. However, a Device Stream can facilitate 2-way communication with a device. It can receive data from a device, and it can also send firmware or software updates back down to the device."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000094"),
                        Title = "Data Map",
                        Subtitle = "Data Flow Tour",
                        Lookup = "data-map",
                        Selector = "#dataMapModuleOption",
                        Orientation = OrientationTypes.TopRight,
                        Content = "The data map is a real-time analytics service that allows you to manipulate and analyze your data before pushing to the configured outputs."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000095"),
                        Title = "Cold Storage",
                        Subtitle = "Data Flow Tour",
                        Lookup = "cold-storage",
                        Selector = "#coldStorageModuleOption",
                        Orientation = OrientationTypes.Left,
                        Content = "This storage option costs less than $1/month. We recommend sending raw, untouched JSON messages from a device to cold storage. This allows you to refer to cold storage for debugging when you need to see the exact messages that were sent from a device."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000096"),
                        Title = "Warm Storage",
                        Subtitle = "Data Flow Tour",
                        Lookup = "warm-storage",
                        Selector = "#warmStorageModuleOption",
                        Orientation = OrientationTypes.Left,
                        Content = "This storage option is more expensive, starting at $25/month. Warm storage is a database, like CosmosDB or SQL Server. It's queryable storage that is ideal for connecting to reporting solutions like Power BI. Use the data map to convert units (like Celsius to Fahrenheit) before storing in warm storage so the data is ready for downstream use."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000097"),
                        Title = "Hot Storage",
                        Subtitle = "Data Flow Tour",
                        Lookup = "hot-storage",
                        Selector = "#hotStorageModuleOption",
                        Orientation = OrientationTypes.Left,
                        Content = "This storage option is also inexpensive, normally less than $1/month. We recommend hot storage for real-time device dashboards."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000098"),
                        Title = "Warm Query",
                        Subtitle = "Data Flow Tour",
                        Lookup = "warm-query",
                        Selector = "#warmQueryModuleOption",
                        Orientation = OrientationTypes.TopRight,
                        Content = "Depending on the dashboard and reporting tools you’re using; you may not be able to connect directly to the warm storage. If that’s the case, the warm query provides an API endpoint that you can use to get access to your data."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000099"),
                        Title = "Save",
                        Subtitle = "Data Flow Tour",
                        Lookup = "save",
                        Selector = "#dataFlowIdeSaveBtn",
                        Orientation = OrientationTypes.Bottom,
                        Content = "As you create your data flow, be sure to regularly save your configuration."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0001-000000000090"),
                        Title = "Deploy",
                        Subtitle = "Data Flow Tour",
                        Lookup = "deploy",
                        Selector = "#dataFlowIdeDeployBtn",
                        Orientation = OrientationTypes.Bottom,
                        Content = "Once your data flow is complete, simply click here to deploy the configured resources in your Azure portal."
                    }
                }
            };
        }

        protected virtual GuidedTour createProSettingsTour(string lookup)
        {
            return new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000010"),
                Lookup = lookup,
                UseOrb = false,
                Steps = new List<GuidedTourStep>()
                {
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000100"),
                        Title = "Architecture",
                        Subtitle = "IDE Settings Tour",
                        Lookup = "architecture",
                        IframeSelector = "#externalDialogIframe",
                        Selector = "#settingsArchitectureNavLink",
                        Orientation = OrientationTypes.Right,
                        Content = "Add, edit, delete your organization's Low-Code Unit NPM packages."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000101"),
                        Title = "Configuration",
                        Subtitle = "IDE Settings Tour",
                        Lookup = "configuration",
                        IframeSelector = "#externalDialogIframe",
                        Selector = "#settingsConfigurationNavLink",
                        Orientation = OrientationTypes.Right,
                        Content = "Manage which elements of your Low Code Units appear in your IDE."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000102"),
                        Title = "Setup",
                        Subtitle = "IDE Settings Tour",
                        Lookup = "setup",
                        IframeSelector = "#externalDialogIframe",
                        Selector = "#settingsSetupNavLink",
                        Orientation = OrientationTypes.Right,
                        Content = "Modify the layout of your IDE’s activity bar and side bar."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000103"),
                        Title = "Marketplace",
                        Subtitle = "IDE Settings Tour",
                        Lookup = "marketplace",
                        IframeSelector = "#externalDialogIframe",
                        Selector = "#settingsMarketplaceNavLink",
                        Orientation = OrientationTypes.Right,
                        Content = "Our Low-Code Unit™ marketplace is coming soon."
                    }
                }
            };
        }
        #endregion
    }
}
