using elalAPI.Models.DataTypeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PublishedModels;

namespace elalAPI.Models
{
    public class PlazmaModel
    {
        public DealModel[] Deals { get; set; }

        public PlazmaModel(IPublishedContent ipc )
        {
            
            if (ipc is Plazma)
            {
                GetProps(ipc as Plazma);
            }
        }


        public PlazmaModel(Plazma _p)
        {
            GetProps(_p); 
        }

        private void GetProps(Plazma _p)
        {
          
            List<DealModel> _deals = new List<DealModel>();
            foreach (Deal _d in _p.Deals)
            {
                _deals.Add(new DealModel(_d));
            }
            Deals = _deals.ToArray();
        }
    }
}