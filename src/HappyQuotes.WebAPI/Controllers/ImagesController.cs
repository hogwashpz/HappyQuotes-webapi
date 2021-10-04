using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HappyQuotes.Application.Models;
using HappyQuotes.Application.Services;
using HappyQuotes.WebAPI.Infrastructure.ApiConventions;
using Microsoft.Extensions.Logging;

namespace HappyQuotes.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageSearchService _imageSearchService;

        public ImagesController(IImageSearchService imageSearchService, ILogger<ImagesController> logger)
        {
            _imageSearchService = imageSearchService;
        }

        [HttpGet(nameof(Random))]
        [ApiConventionMethod(typeof(HandledExceptionConvention), nameof(Random))]
        public async Task<ActionResult<ImageLinkModel>> Random()
        {
            var images = await _imageSearchService.HappyQoutesSearchAsync();
            return Ok(images);
        }
    }
}
