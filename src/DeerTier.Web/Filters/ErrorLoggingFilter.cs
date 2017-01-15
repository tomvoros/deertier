using log4net;
using System.Web.Mvc;

namespace DeerTier.Web.Filters
{
    public class ErrorLoggingFilter : IExceptionFilter
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ErrorLoggingFilter));

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.ExceptionHandled && filterContext.Exception != null)
            {
                _logger.Error("Caught exception", filterContext.Exception);
            }
        }
    }
}