using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PublishedModels;

namespace elalAPI.Models.DataTypeModels
{
    public class DealModel
    {

        public string RedLabel { get; set; }

        public string Title { get; set; }
        public float Price { get; set; }
        public float DeletedPrice { get; set; }
        public int Points { get; set; }
        public CurrencyModel Currency { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public ImageModel Sunny { get; set; }



        public DealModel(Deal _d)
        {
            GetProps(_d);
        }
        public DealModel(IPublishedContent ipc)
        {
            if (ipc is Deal)
            {
                GetProps(ipc as Deal);
            }
        }


        public void GetProps(Deal _d)
        {
            Title = _d.Title;
            Price = (float)_d.Price;
            DeletedPrice = (float)_d.DeletedPrice;
            Points = _d.Points;
            StartDate = _d.StartDate;
            EndDate = _d.EndDate;
            Sunny = new ImageModel(_d.Sunny as Image);
            Currency = new CurrencyModel(_d.Currency as Currency);
        }
    }
}