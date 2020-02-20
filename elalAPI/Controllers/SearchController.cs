using Examine;
using Examine.LuceneEngine;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Examine;
using Umbraco.Web.WebApi;

namespace elalAPI.Controllers
{
    public class SearchController : UmbracoApiController 
    {
        [HttpGet]
        public string GetTerm(string term) {
            //http://elal.test/umbraco/api/search/GetTerm?term=NIS

            string stringResult = "Showing results for:" + term + "-->";
            if (ExamineManager.Instance.TryGetIndex("ExternalIndex", out var index))
            {
                var searcher = index.GetSearcher();
                //var results = searcher.CreateQuery("content").All();
                //var results2 = searcher.CreateQuery("content").NodeTypeAlias("basePage").And().Field("nodeName", term).Execute();
                //var results3 = searcher.CreateQuery("content").NodeTypeAlias("currency").And().Field("nodeName", "NIS").Execute();

                LuceneSearchResults res = searcher.Search(term) as LuceneSearchResults;
                
                stringResult += "Count: " + res.TotalItemCount + "-->";

                foreach (var item in res.TopDocs.ScoreDocs)
                {
                    stringResult+= item.ToString() + "-->";
                }

                //stringResult += "Umbraco ->> "+ Umbraco.ContentQuery.Search(term);
            }

            return stringResult;
        }     
    }
}