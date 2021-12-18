using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Clinic.Core.CustomMiddlewares
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch
            {
                var message = "<h1>Exception during processing request</h1>";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                context.Response.StatusCode = 404;
                await context.Response.Body.WriteAsync(data, 0, data.Length);
            }
        }
    }

}
