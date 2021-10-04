using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyQuotes.WebAPI.Infrastructure.ApiConventions
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-2.2
    /// and
    /// https://github.com/dotnet/aspnetcore/issues/10899
    /// </summary>
    public static class HandledExceptionConvention
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
        public static void Random()
        {
        }
    }
}
