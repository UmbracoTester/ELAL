﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using elalAPI.Models;
using Examine;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Examine;
using Umbraco.Web;
using Umbraco.Web.PublishedModels;
using Umbraco.Web.WebApi;

namespace elalAPI.Managers
{
    public interface IDestinationManager
    {
        FlightDestinationModel GetDestinationModel(IPublishedContent ipc );

        List<FlightDestinationModel> GetAllDestinations(IPublishedContent ipc);
        PlazmaModel GetPlazma(IPublishedContent ipc);
    }
    public class DestinationManager :  IDestinationManager
    {
        public DestinationManager()
        {
           

        }

        public List<FlightDestinationModel> GetAllDestinations(IPublishedContent ipc)
        {
            List<FlightDestinationModel> destinations = new List<FlightDestinationModel>();
            if (ipc is FlightDestination) {
                foreach (FlightDestination item in ipc.Children)
                {
                    FlightDestinationModel dest = new FlightDestinationModel(item);
                    destinations.Add(dest);
                }
            }                      
            return destinations;
        }
         
        public FlightDestinationModel GetDestinationModel(IPublishedContent ipc)
        {           
            FlightDestinationModel destination = new FlightDestinationModel(ipc);
            return destination;
        }

        public PlazmaModel GetPlazma(IPublishedContent ipc)
        {
            return ipc is Plazma ? new PlazmaModel(ipc) : null;

        }
    }
}