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

namespace LCU.State.API.NapkinIDE.NapkinIDE.GuidedTour.State
{
    public class GuidedTourStateHarness : LCUStateHarness<GuidedTourState>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public GuidedTourStateHarness(GuidedTourState state)
            : base(state ?? new GuidedTourState())
        { }
        #endregion

        #region API Methods
        public virtual void LoadGuidedTours()
        {
            State.Tours = new List<GuidedTour>();

            State.Tours.Add(createDemoTour("demo-tour"));

            State.Tours.Add(createLimitedTrialTour("limited-trial-tour"));
        }

        public virtual void Refresh()
        {
            LoadGuidedTours();

            State.CurrentTour = State.Tours.FirstOrDefault(tour => tour.Lookup == "limited-trial-tour");
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
                        Content = "Welcome to the LCU-Guided-Tour library! This library provides the functionality to do your own guided tour of an application. <br/><br/> Click the <b>Next</b> button to get started with an example Tour!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Title",
                        Subtitle = "Guided Tour",
                        Selector = "#guidedTourHeader",
                        Orientation = OrientationTypes.Bottom,
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
                        Content = "Welcome to the LCU-Guided-Tour library! This library provides the functionality to do your own guided tour of an application. <br/><br/> Click the <b>Next</b> button to get started with an example Tour!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Title",
                        Subtitle = "Guided Tour",
                        Selector = "#guidedTourHeader",
                        Orientation = OrientationTypes.Bottom,
                        Content = "With the LCU-Guided-Tour, you can select anything that is on the screen that has a valid CSS selector.  For example, you can select this title, which as an id of <b>#guidedTourHeader</b>. <br/><br/> Valid selectors are as follows: <ul><li>.class</li><li>#id</li><li>element</li></ul>"
                    },
                    new GuidedTourStep()
                    {
                        Title = "First Paragraph",
                        Subtitle = "Guided Tour",
                        Selector = "p",
                        Orientation = OrientationTypes.BottomRight,
                        Content = "Here, we are selecting the first paragraph element on the screen with <b>p</b>."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Second Paragraph",
                        Subtitle = "Guided Tour",
                        Selector = "#p2",
                        Orientation = OrientationTypes.Top,
                        Content = "Now we are selecting the second paragraph, that has an id of <b>#p2</b>, in which we are targeting."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Complex Selectors",
                        Subtitle = "Guided Tour",
                        Selector = ".section:nth-of-type(2) .mat-radio-button:nth-child(3)",
                        Orientation = OrientationTypes.Right,
                        Content = "You can even target more specific, complex elements, by using various built-in CSS selectors. In this case, we are targeting the third radio item in the second section with the selector of: <br/> <b>.section:nth-of-type(2) .mat-radio-button:nth-child(3)</b>"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Modifiers",
                        Subtitle = "Guided Tour",
                        Selector = "#formBox",
                        Orientation = OrientationTypes.Right,
                        Content = "As for the bot, you can modify certain properties of it in order to customize it to your needs. Here we can change the position it lives on the screen, the container it should position itself in, as well as the amount of padding we would like to have between the bot and the container."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Bounding Container",
                        Subtitle = "Guided Tour",
                        Selector = "#boundingBox",
                        Orientation = OrientationTypes.Left,
                        Content = "As an example, you can set the Bot to be positioned inside this box by setting the container to the <b>#boundingBox</b> selector."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Assigning Actions",
                        Subtitle = "Guided Tour",
                        Selector = ".mat-tab-label:nth-of-type(2)",
                        Orientation = OrientationTypes.BottomLeft,
                        Content = "You can assign each step an action as well, in case you want to run logic before or after a step is displayed.  Click <b>Next</b> to see this in action!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Tab Movement",
                        Subtitle = "Guided Tour",
                        Selector = "#boxLogoForm",
                        Orientation = OrientationTypes.BottomLeft,
                        ActionDelay = 500,
                        Content = "As you can see, this tab was selected so that the Tour could continue after the DOM has rendered a different view.  You can also use the <b>actionDelay</b> property to specify a time delay before showing the next step, in order to properly render the next view."
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
                        ID = new Guid("00000000-0000-0000-0000-000000000000"),
                        Title = "Welcome Page",
                        Subtitle = "Limited Trial Tour",
                        Content = "Welcome to the live demo of the Fathym Low-Code Framework. I’m <b>Thinky</b>! I’ll guide you through a few tours to show you some of Fathym’s low-code tools, such as the <b>Data Flow Manager</b> and <b>Data Applications</b>, and I’ll explain how to interact and dig deeper with the tools so you can customize them for your needs."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000001"),
                        Title = "Welcome Page Resources",
                        Subtitle = "Limited Trial Tour",
                        Selector = ".mat-tab-body-wrapper",
                        Orientation = OrientationTypes.Left,
                        Content = "Here are a number of resources and guided tours you can select that will educate you of the different developer journeys you can undertake through Fathym."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000002"),
                        Title = "Data Flows",
                        Subtitle = "Limited Trial Tour",
                        Selector = ".ide-side-bar-action:nth-of-type(2)",
                        Orientation = OrientationTypes.Right,
                        Content = "The <b>Data Flow Manager</b> is a powerful drag and drop interface for easily configuring and provisioning end-to-end cloud infrastructure. Navigate here to explore further."
                    },
                    new GuidedTourStep()
                    {
                        ID = new Guid("00000000-0000-0000-0000-000000000003"),
                        Title = "Data Applications",
                        Subtitle = "Limited Trial Tour",
                        Selector = ".ide-side-bar-action:nth-of-type(3)",
                        Orientation = OrientationTypes.Right,
                        Content = "<b>Data applications</b> are quick and easy ways to build and deliver enterprise scalable experiences to your users. Create your own, configure your own, or use pre-existing applications."
                    }
                }
            };
        }

        #endregion
    }
}
