using log4net;
using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DeerTier.Web
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MvcApplication));

#if DEBUG
        public static bool IsPreviewSite = true;
#else
        public static bool IsPreviewSite = false;
#endif

        protected void Application_Start()
        {
            _logger.Info("Starting application...");

            IoCConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            IoCConfig.Dispose();

            _logger.Info("Stopping application...");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Implement HTTP compression
            var application = (HttpApplication)sender;
            
            // Retrieve accepted encodings
            var encodings = application.Request.Headers.Get("Accept-Encoding");
            if (encodings != null)
            {
                // Check the browser accepts deflate or gzip (deflate takes preference)
                encodings = encodings.ToLower();
                if (encodings.Contains("deflate"))
                {
                    application.Response.Filter = new DeflateStream(application.Response.Filter, CompressionMode.Compress);
                    application.Response.AppendHeader("Content-Encoding", "deflate");
                }
                else if (encodings.Contains("gzip"))
                {
                    application.Response.Filter = new GZipStream(application.Response.Filter, CompressionMode.Compress);
                    application.Response.AppendHeader("Content-Encoding", "gzip");
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // We have to remove the compression filter on error because ASP.NET removes the header
            var application = (HttpApplication)sender;
            application.Response.Filter = null;

            var exception = Server.GetLastError();
            var httpException = exception as HttpException;

            // Check for 404 error
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var request = application.Request;
                _logger.Warn($"404 error: {request.RawUrl}, {request.UserAgent}");
            }
            else
            {
                _logger.Error("Unhandled exception", exception);
            }
        }
    }
}
