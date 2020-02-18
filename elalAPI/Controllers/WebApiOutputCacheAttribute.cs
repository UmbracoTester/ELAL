using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
namespace elalAPI.Controllers
{
    /// <summary>
    /// Set output cache on web api
    /// </summary>
    public class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Cache length in seconds
        /// </summary>
        private readonly int _timespan;

        /// <summary>
        /// Client cache length in seconds
        /// </summary>
        private readonly int _clientTimeSpan;

        /// <summary>
        /// Cache for anonymous users only?
        /// </summary>
        private readonly bool _anonymousOnly;

        /// <summary>
        /// Cache key
        /// </summary>
        private string _cachekey;

        /// <summary>
        /// Set cache item policy for dependency cache
        /// </summary>
        private readonly bool _useCacheItemPolicy;

        /// <summary>
        /// Dependency cache file path
        /// </summary>
        private readonly string _cacheItemPolicyFilePath;

        /// <summary>
        /// Cache repository
        /// </summary>
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        /// <summary>
        /// Set output cache on Web method.
        /// </summary>
        /// <param name="outputCacheProfileName"></param>
        public WebApiOutputCacheAttribute(string outputCacheProfileName, bool useCacheItemPolicy = false)
        {
            try
            {
                _timespan = _clientTimeSpan = int.Parse(ConfigurationManager.AppSettings[outputCacheProfileName]);
                _useCacheItemPolicy = useCacheItemPolicy;
                _cacheItemPolicyFilePath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Output.Cache.Policy.File.Path"]);
            }
            catch (Exception ex)
            {
                // Logger.Event(EventType.Error, ex, "WebApiOutputCacheAttribute.cs", "WebApiOutputCacheAttribute");
            }
        }

        public WebApiOutputCacheAttribute(int timespan, int clientTimeSpan, bool anonymousOnly = false, bool useCacheItemPolicy = false)
        {
            try
            {
                _timespan = timespan;
                _clientTimeSpan = clientTimeSpan;
                _anonymousOnly = anonymousOnly;
                _useCacheItemPolicy = useCacheItemPolicy;
            }
            catch (Exception ex)
            {
                //Logger.Event(EventType.Error, ex, "WebApiOutputCacheAttribute.cs", "WebApiOutputCacheAttribute");
            }
        }

        /// <summary>
        /// Checking if the request can be Cacheable
        /// </summary>
        /// <param name="ActionContext">HttpActionContext</param>
        /// <returns>Is the request cacheable</returns>
        private bool _isCacheable(HttpActionContext ActionContext)
        {
            if (_timespan > 0 && _clientTimeSpan > 0)
            {
                if (_anonymousOnly)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;
                if (ActionContext.Request.Method == HttpMethod.Get || ActionContext.Request.Method == HttpMethod.Post) return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }
            return false;
        }

        /// <summary>
        /// Set cache control headers
        /// </summary>
        /// <returns>CacheControlHeaderValue</returns>
        private CacheControlHeaderValue SetClientCache()
        {
            return new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(_clientTimeSpan),
                Public = true
            };
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext != null)
                {
                    if (_isCacheable(actionContext))
                    {
                        string authKey = string.Empty;

                        if (actionContext.Request.Method == HttpMethod.Post)
                        {
                            using (var stream = new System.IO.StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
                            {
                                stream.BaseStream.Position = 0;
                                authKey = stream.ReadToEnd();
                            }
                        }

                        authKey += ":" + (!string.IsNullOrEmpty(actionContext.Request.RequestUri.ParseQueryString()["lang"]) ? actionContext.Request.RequestUri.ParseQueryString()["lang"] : string.Empty);
                        authKey += ":" + (!string.IsNullOrEmpty(actionContext.Request.RequestUri.ParseQueryString()["uri"]) ? actionContext.Request.RequestUri.ParseQueryString()["uri"] : string.Empty);

                        _cachekey = string.Join(":", new string[] { actionContext.Request.RequestUri.AbsolutePath, actionContext.Request.Headers.Accept.FirstOrDefault().ToString(), authKey });
                        if (WebApiCache.Contains(_cachekey))
                        {
                            var cacheValue = (string)WebApiCache.Get(_cachekey);
                            if (cacheValue != null)
                            {
                                actionContext.Response = actionContext.Request.CreateResponse();
                                actionContext.Response.Content = new StringContent(cacheValue);
                                var contenTtype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct");
                                if (contenTtype == null)
                                    contenTtype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                                actionContext.Response.Content.Headers.ContentType = contenTtype;
                                actionContext.Response.Headers.CacheControl = SetClientCache();
                                actionContext.Response.Content.Headers.LastModified = DateTime.Now.AddSeconds(-2);
                            }
                        }
                    }
                }
                else
                {
                    //Logger.Event(EventType.Error, "NOT CACHEABLE! " + ac, "WebApiOutputCacheAttribute.cs", "OnActionExecuting");
                    throw new ArgumentNullException("actionContext");
                }
            }
            catch (Exception ex)
            {
                //Logger.Event(EventType.Error, ex, "WebApiOutputCacheAttribute.cs", "OnActionExecuting");
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (!(WebApiCache.Contains(_cachekey)) && actionExecutedContext.Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

                    if (_useCacheItemPolicy)
                    {
                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { _cacheItemPolicyFilePath }));

                        WebApiCache.Add(_cachekey, body, policy);
                        WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, policy);
                    }
                    else
                    {
                        WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(_timespan));
                        WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_timespan));
                    }
                }
                if (_isCacheable(actionExecutedContext.ActionContext))
                    actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
            }
            catch (Exception ex)
            {
                //Logger.Event(EventType.Error, ex, "WebApiOutputCacheAttribute.cs", "OnActionExecuted");
            }
        }
    }
}
