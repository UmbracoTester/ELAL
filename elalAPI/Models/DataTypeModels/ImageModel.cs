using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.PublishedModels;

namespace elalAPI.Models.DataTypeModels
{
    public class ImageModel
    {
        public string Url { get; set; }
        public ImageModel(Image  img)
        {
            Url = img.Url;
        }
    }
}