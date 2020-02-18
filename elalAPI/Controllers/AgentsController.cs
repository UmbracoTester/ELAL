using ClientDependency.Core.Logging;
using elalAPI.Managers;
using elalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Umbraco.Web.PublishedModels;
using Umbraco.Web.WebApi;

namespace elalAPI.Controllers
{
    public class AgentsController : UmbracoApiController
    {

        private Managers.ILogger _logger;

        public AgentsController()
        {
            _logger = new Logger();
        }


        [System.Web.Http.HttpGet]
        public string GetAgentPage()
        {
            //http://elal.test/umbraco/api/Agents/GetAgentPage
            _logger.LogStart();

            var ipc = UmbracoContext.Content.GetByRoute("/agentspage/");
            var treePicker = (ipc as AgentsPage).TreePicker;
            BasePage treePickerParentNode = treePicker.First() as BasePage;
            List<FlightDestinationModel> list = new List<FlightDestinationModel>();

            foreach (FlightDestination item in treePickerParentNode.Children)
            {
                list.Add(new FlightDestinationModel(item));
            }
            var js = new JavaScriptSerializer();
            return js.Serialize(list);

        }
    }
}