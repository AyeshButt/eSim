using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.Common.StaticClasses
{
    public static class HttpStatusCodeMapper
    {
        public static int FetchStatusCode(int statusCode) =>
            statusCode switch
            {
                400 => StatusCodes.Status400BadRequest,
                403 => StatusCodes.Status403Forbidden,
                401 => StatusCodes.Status401Unauthorized,
                404 => StatusCodes.Status404NotFound,
                429 => StatusCodes.Status429TooManyRequests,
                500 => StatusCodes.Status500InternalServerError,
                503 => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status200OK
            };
    }

}
