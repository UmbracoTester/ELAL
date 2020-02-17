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
        public bool DestinationIsActive { get; set; }
        public string DisplayName { get; set; }
        public bool DisplayOnFlightPackages { get; set; }
        public bool DisplayOnSearchEngine { get; set; }
        public bool DisplayOnVehiclePackages { get; set; }
        public string IAta { get; set; }
        public bool ShowInFooter { get; set; }
        public bool ShowInNavigation { get; set; }
        public string NewLabel { get; set; }
        public Image Image { get; set; }

        public FlightDestinationModel(IPublishedContent ipc)
        {
            if (ipc is FlightDesination) { AssignProps(ipc as FlightDesination); }


        }

        private void AssignProps(FlightDesination fd)
        {
            Airport = fd.Airport;
            City = fd.City;
            Country = fd.Country;
            DestinationIsActive = fd.DestinationIsActive;
            DisplayName = fd.DisplayName;
            DisplayOnFlightPackages = fd.DisplayOnFlightPackages;
            DisplayOnSearchEngine = fd.DisplayOnSearchEngine;
            DisplayOnVehiclePackages = fd.DisplayOnVehiclePackages;
            IAta = fd.IAta;
            ShowInFooter = fd.ShowInFooter;
            NewLabel = fd.NewLabel;
            ShowInNavigation = fd.ShowInNavigation;
            Image =  fd.Image is Image ?  fd.Image as Image :  null;


        }
    }
}