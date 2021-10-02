using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyQuotes.WebAPI.Controllers
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-5.0
    /// and
    /// https://www.c-sharpcorner.com/article/global-error-handling-in-asp-net-core-5/
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GlobalErrorsController : ControllerBase
    {
        [HttpGet]
        [Route("/errors")]
        public IActionResult Errors()
        {
            var contextException = HttpContext.Features.Get<IExceptionHandlerFeature>(); // Capture the exception

            var responseStatusCode = contextException.Error switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status503ServiceUnavailable
            };

            return Problem(detail: contextException.Error.Message, statusCode: responseStatusCode);
        }
    }
}
