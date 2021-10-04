using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.CustomSearchAPI.v1.Data;
using Google.Apis.Services;
using HappyQuotes.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using HappyQuotes.Application.Exceptions;


namespace HappyQuotes.Application.Services
{
    public class HappyQuotesImagesQuery
    {
        private const string queryTerms = "happy quotes";

        private readonly ILogger<HappyQuotesImagesQuery> _logger;
        private readonly SemaphoreSlim _singleSlimLock = new SemaphoreSlim(1, 1);
        private readonly CseResource.ListRequest _searchRequest;

        #region Constructor

        public HappyQuotesImagesQuery(IOptions<GoogleCustomSearchOptions> configs, ILogger<HappyQuotesImagesQuery> logger)
        {
            _logger = logger;

            if (string.IsNullOrWhiteSpace(configs.Value.ApiKey) || string.IsNullOrWhiteSpace(configs.Value.SearchEngineID))
            {
                throw new ArgumentException("Google ApiKey or SearchEngineID are not set.");
            }

            var customSearchService = new CustomSearchAPIService(new BaseClientService.Initializer { ApiKey = configs.Value.ApiKey });

            _searchRequest = customSearchService.Cse.List();
            _searchRequest.Cx = configs.Value.SearchEngineID;
            _searchRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            _searchRequest.ImgSize = CseResource.ListRequest.ImgSizeEnum.MEDIUM;
            _searchRequest.Num = 10;
            _searchRequest.ExactTerms = queryTerms;
        }

        #endregion

        public async Task<(bool lockTimeouted, List<Result>? results)> TryExecuteAsync()
        { 
            // if lock is aquired try to await for 3 seconds then try again and return false if not success, try retriving from cache
            if (await _singleSlimLock.WaitAsync(3_000))
            {
                try
                {
                    var searchResults = new List<Result>(100);

                    var count = 1;
                    _searchRequest.Start = count;
                    do
                    {
                        var result = await _searchRequest.ExecuteAsync();
                        if (result.Items is not null)
                            searchResults.AddRange(result.Items);

                        count += 10;
                        _searchRequest.Start = count;

                    } while (count < 11); // TODO: Temporary take just 10 results

                    return (false, searchResults);
                }
                catch (Google.GoogleApiException gex)
                {
                    _logger.LogError("Google api error: {gex.ToString()}", gex.ToString());
                    throw new ExternalApiException("Error while executing google image search api.", gex);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    _searchRequest.Start = 0;
                    _singleSlimLock.Release();
                }
            }
            else
            {
                return (true, null);
            }
        }
    }
}
