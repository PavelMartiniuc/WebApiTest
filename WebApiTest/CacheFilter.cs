/*
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net.Http.Headers
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest
{
    public class CacheFilter : ActionFilterAttribute
    {
        public int TimeDuration { get; set; }
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Ac.Response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(TimeDuration),
                MustRevalidate = true,
                Public = true
            };
        }
    }
}
*/