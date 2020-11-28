using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace MovieSearch.RenderHelper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute: ActionFilterAttribute
    {
        private RequestHeaders _headers;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _headers = context.HttpContext.Request.GetTypedHeaders();

            if (_headers.Referer == null ||
                _headers.Host.Host != _headers.Referer.Host)
            {
                context.HttpContext.Response.Redirect("/Movies/");
            }
        }
    }
}
