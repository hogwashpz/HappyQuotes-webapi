using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HappyQuotes.Application.Models;
using Google.Apis.CustomSearchAPI.v1.Data;
using HappyQuotes.Application.Exceptions;

namespace HappyQuotes.Application.Services
{
    public class ImageSearchService : IImageSearchService
    {
        private readonly HappyQuotesImagesQuery _query; 
        private readonly ILogger<ImageSearchService> _logger;
        private readonly Random _randomNum;

        public ImageSearchService(HappyQuotesImagesQuery query, ILogger<ImageSearchService> logger)
        {
            _query = query;
            _logger = logger;
            _randomNum = new Random();
        }

        public async Task<ImageLinkModel> HappyQoutesSearchAsync()
        {
            (bool lockTimeouted, List<Result>? results) = await _query.TryExecuteAsync();

            _logger.LogInformation("First fetch of happy qoutes image count: {results?.Count}", results?.Count);

            if (lockTimeouted)
            {
                _logger.LogCritical("Slim async lock timedout!");
                // add retries, with some maximum tries 
                throw new TimeoutException("Fetching 100 images somehow timeouted, this will be change to recuring call and cache hit.");
            }

            if (results is null || results?.Count == 0)
                throw new NoResultException("No images returned.");

            var item = results![_randomNum.Next(results.Count)];

            return new ImageLinkModel
            {
                Link = item.Link ?? string.Empty,
                Title = item.Title ?? string.Empty
            };
        }
    }
}
