using DeerTier.Web.Utils;
using System.Web.Mvc;

namespace DeerTier.Web.Filters
{
    public class ApiErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                return;
            }

            var exception = filterContext.Exception as ApiException;
            if (exception == null)
            {
                exception = new ApiException("API exception", filterContext.Exception);
            }

            // Return a sanitized error result
            filterContext.Result = new HttpStatusCodeResult(exception.StatusCode, exception.Message);
        }
    }
}