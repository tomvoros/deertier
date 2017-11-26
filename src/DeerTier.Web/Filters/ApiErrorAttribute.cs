using DeerTier.Web.Utils;
using System.Net;
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
            if (exception != null)
            {
                // Return error from exception
                filterContext.Result = new HttpStatusCodeResult(exception.StatusCode, exception.Message);
            }
            else
            {
                // Return a generic error
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "API Exception");
            }
        }
    }
}