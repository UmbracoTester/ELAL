using elalAPI.Models.DataTypeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PublishedModels;

namespace elalAPI.Models
{

    public class FlightDestinationModel
    {
        public string Airport { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsDestinationActive { get; set; }
        public string DestinationName { get; set; }
        public bool ShowInFlightPackages { get; set; }
        public bool ShowInRegularEngine { get; set; }
        public bool ShowInVehiclePackages { get; set; }
        public string IAta { get; set; }
        public bool ShowInFooter { get; set; }
        public bool ShowInNavigation { get; set; }
        public bool IsNew { get; set; }
        public ImageModel Image { get; set; }

        public FlightDestinationModel(IPublishedContent ipc)
        {
            if (ipc is FlightDestination) { AssignProps(ipc as FlightDestination); }


        }

        private void AssignProps(FlightDestination fd)
        {
            Airport = fd.Airport;
            City = fd.City;
            Country = fd.Country;
            IsDestinationActive = fd.IsDestinationActive;
            DestinationName = fd.DestinationName;
            ShowInFlightPackages = fd.ShowInFlightPackages;
            ShowInRegularEngine = fd.ShowInRegularEngine;
            ShowInVehiclePackages = fd.ShowInVehiclePackages;
            IAta = fd.Iata;
            ShowInFooter = fd.ShowInFooter;
            IsNew = fd.IsNew;
            ShowInNavigation = fd.ShowInNavigation;

            if (fd.Image is Image)
            {
                Image = new ImageModel(fd.Image as Image);
            }

          


        }
    }
}