using elalAPI.Models;
using System;
using System.Collections.Generic;
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

        [System.Web.Http.HttpGet]
        public string DestinationById(int id ) {

            //http://elal.test/umbraco/api/destinations/destinationbyid?id=1077
            var ipc = UmbracoContext.Content.GetById(id);
            FlightDestinationModel destination = new FlightDestinationModel(ipc);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(destination);
            
        }

        [System.Web.Http.HttpGet]
        public string GetAllDestinations() {
            //http://elal.test/umbraco/api/destinations/GetAllDestinations

            //1: Get element 
            Plazma plazma = UmbracoContext.Content.GetByRoute("/he/first-plazma/") as Plazma;
            //2: Extract destination picker 
            var picker = plazma.DestinationPicker;
            //3: Loop through picker children
            List<FlightDestinationModel> destinations = new List<FlightDestinationModel>();
            foreach (FlightDesination item in picker)
            {
                FlightDestinationModel dest = new FlightDestinationModel(item);
                destinations.Add(dest);
            }
            //4: return children as DestinationModel 
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(destinations);

        }



    }
}