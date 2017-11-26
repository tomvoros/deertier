using DeerTier.Web.Utils;
using log4net;
using System;
using System.Web.Mvc;

namespace DeerTier.Web.Filters
{
    public class ApiAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string ApiKeyHeader = "X-API-Key";

        private static readonly ILog _logger = LogManager.GetLogger(typeof(ApiAuthorizeAttribute));

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            
            if (filterContext.HttpContext != null && filterContext.HttpContext.Request != null && filterContext.HttpContext.Request.Headers != null)
            {
                var apiKey = filterContext.HttpContext.Request.Headers.Get(ApiKeyHeader);
                if (apiKey == ConfigHelper.ApiKey)
                {
                    // Authorized
                    return;
                }

                _logger.Error($"Invalid API key: [{apiKey}]");
            }

            _logger.Error("Unauthorized API call");

            filterContext.Result = new HttpUnauthorizedResult();

            if (filterContext.HttpContext != null && filterContext.HttpContext.Response != null)
            {
                // Suppress redirect to login page
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }
    }
}