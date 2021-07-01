using System;
using System.Net;

namespace WebApiTest.Common
{
    public class ClientErrorResponse : Exception
    {
        public HttpStatusCode statusCode { get; set; }

        public ClientErrorResponse() { }

        public ClientErrorResponse(HttpStatusCode statusCode, string message) : base(message)
        {
            this.statusCode = statusCode;
        }
    }
}
