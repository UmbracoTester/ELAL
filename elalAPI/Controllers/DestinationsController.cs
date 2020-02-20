using elalAPI.Managers;
using elalAPI.Models;
using elalAPI.Models.DataTypeModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Umbraco.Web.PublishedModels;
using Umbraco.Web.WebApi;

namespace elalAPI.Controllers
{
    public class DestinationsController : UmbracoApiController
    {

        public IDestinationManager _manager;
        ILogger _logger;
        private string AllDestinationRoute = "";

        public DestinationsController()
        {
            _manager = new DestinationManager();
            _logger = new ElalLogger();
             
        }


        public string GetPlazma() {
            //http://elal.test/umbraco/api/destinations/GetPlazma 

            var ipc = UmbracoContext.Content.GetByRoute("/he/homepage/blazma/");
            var plazma= _manager.GetPlazma(ipc);
            var js = new JavaScriptSerializer();
            string res = js.Serialize(plazma); 
            return res;
        }
        public string GetDeal()
        {
            //http://elal.test/umbraco/api/destinations/GetDeal

            var ipc = UmbracoContext.Content.GetByRoute("/settings/deals/berlin-deal-1/");
            var deal = new DealModel(ipc as Deal);
            var js = new JavaScriptSerializer();
            return js.Serialize(deal);
        }


        [System.Web.Http.HttpGet]
        public string DestinationById(int id)
        {
            //http://elal.test/umbraco/api/destinations/destinationbyid?id=1061
            _logger.LogStart();

            var ipc = UmbracoContext.Content.GetById(id);
            var destination = _manager.GetDestinationModel(ipc);
            var js = new JavaScriptSerializer();
            return js.Serialize(destination);

        }

        [System.Web.Http.HttpGet]
        public string GetAllDestinations()
        {
            //http://elal.test/umbraco/api/destinations/GetAllDestinations
            var ipc = UmbracoContext.Content.GetByRoute(AllDestinationRoute);
            var destList = _manager.GetAllDestinations(ipc);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(destList);

        }

        [System.Web.Http.HttpGet]
        public string GetAllDestinationsFromPlazma(string route)
        {
            //http://elal.test/umbraco/api/destinations/GetAllDestinations
            var ipc = UmbracoContext.Content.GetByRoute(route);
            var destList = _manager.GetAllDestinations(ipc);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(destList);

        }



    }
}