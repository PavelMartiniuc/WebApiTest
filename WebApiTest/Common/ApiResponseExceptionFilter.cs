using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiTest.Common
{
    public class ApiResponseExceptionFilter : ExceptionFilterAttribute
    {
        private class ErrorData
        {
            public int Status { get; set; }
            public string Error { get; set; }
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            if (context.Exception is ClientErrorResponse)
            {
                var exception = (ClientErrorResponse)context.Exception;
                var errorData = new ErrorData()
                {
                    Status = (int)exception.statusCode,
                    Error = exception.Message
                };
                context.Result = new ObjectResult(errorData)
                {
                    StatusCode = (int)exception.statusCode
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
