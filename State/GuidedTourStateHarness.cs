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
        public virtual void Refresh()
        {
            State.CurrentTour = new GuidedTour()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                Lookup = "demo-tour",
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
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.Bottom },
                        Content = "With the LCU-Guided-Tour, you can select anything that is on the screen that has a valid CSS selector.  For example, you can select this title, which as an id of <b>#guidedTourHeader</b>. <br/><br/> Valid selectors are as follows: <ul><li>.class</li><li>#id</li><li>element</li></ul>"
                    },
                    new GuidedTourStep()
                    {
                        Title = "First Paragraph",
                        Subtitle = "Guided Tour",
                        Selector = "p",
                        Orientation = OrientationTypes.BottomRight,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.BottomRight },
                        Content = "Here, we are selecting the first paragraph element on the screen with <b>p</b>."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Second Paragraph",
                        Subtitle = "Guided Tour",
                        Selector = "#p2",
                        Orientation = OrientationTypes.Top,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.Top },
                        Content = "Now we are selecting the second paragraph, that has an id of <b>#p2</b>, in which we are targeting."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Complex Selectors",
                        Subtitle = "Guided Tour",
                        Selector = ".section:nth-of-type(2) .mat-radio-button:nth-child(3)",
                        Orientation = OrientationTypes.Right,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.Right },
                        Content = "You can even target more specific, complex elements, by using various built-in CSS selectors. In this case, we are targeting the third radio item in the second section with the selector of: <br/> <b>.section:nth-of-type(2) .mat-radio-button:nth-child(3)</b>"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Modifiers",
                        Subtitle = "Guided Tour",
                        Selector = "#formBox",
                        Orientation = OrientationTypes.Right,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.Right },
                        Content = "As for the bot, you can modify certain properties of it in order to customize it to your needs. Here we can change the position it lives on the screen, the container it should position itself in, as well as the amount of padding we would like to have between the bot and the container."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Bounding Container",
                        Subtitle = "Guided Tour",
                        Selector = "#boundingBox",
                        Orientation = OrientationTypes.Left,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.Left },
                        Content = "As an example, you can set the Bot to be positioned inside this box by setting the container to the <b>#boundingBox</b> selector."
                    },
                    new GuidedTourStep()
                    {
                        Title = "Assigning Actions",
                        Subtitle = "Guided Tour",
                        Selector = ".mat-tab-label:nth-of-type(2)",
                        Orientation = OrientationTypes.BottomLeft,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.BottomLeft },
                        Content = "You can assign each step an action as well, in case you want to run logic before or after a step is displayed.  Click <b>Next</b> to see this in action!"
                    },
                    new GuidedTourStep()
                    {
                        Title = "Tab Movement",
                        Subtitle = "Guided Tour",
                        Selector = "#boxLogoForm",
                        Orientation = OrientationTypes.BottomLeft,
                        OrientationConfiguration = new OrientationConfiguration() { Orientation = OrientationTypes.BottomLeft },
                        ActionDelay = 500,
                        Content = "As you can see, this tab was selected so that the Tour could continue after the DOM has rendered a different view.  You can also use the <b>actionDelay</b> property to specify a time delay before showing the next step, in order to properly render the next view."
                    }
                }
            };
        }
        #endregion
    }
}
