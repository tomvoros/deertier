using System;
using System.Net;

namespace DeerTier.Web.Utils
{
    // Exception in the API with a "friendly" message that can be returned to the user
    public class ApiException : Exception
    {
        public ApiException(string message)
            : this(HttpStatusCode.InternalServerError, message) { }

        public ApiException(string message, Exception innerException)
            : this(HttpStatusCode.InternalServerError, message, innerException) { }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        
        public HttpStatusCode StatusCode { get; }
    }
}