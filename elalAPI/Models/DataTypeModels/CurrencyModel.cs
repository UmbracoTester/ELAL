using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.PublishedModels;

namespace elalAPI.Models.DataTypeModels
{
    public class CurrencyModel
    {
        public string Sign { get; set; }
        public string Title { get; set; }

        public CurrencyModel(Currency _c)
        {
            GetProps(_c);
        }

       

        public void GetProps(Currency _c) {
            Sign = _c.Sign;
            Title = _c.Title;
        }
    }
}