using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Clinic.Core.CustomMiddlewares
{
    public static class CustomMiddlewaresExtensions
    {
        public static IApplicationBuilder UseCustomResponseCode(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlingMiddleware>();
        }
    }
}
