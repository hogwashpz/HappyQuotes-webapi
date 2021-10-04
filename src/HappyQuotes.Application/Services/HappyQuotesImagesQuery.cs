using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.CustomSearchAPI.v1.Data;
using Google.Apis.Services;
using HappyQuotes.Application.Options;
using Microsoft.Extensions.Options;
using System.Threading;

using Microsoft.Extensions.Logging;

namespace HappyQuotes.Application.Services
{
    public class HappyQuotesImagesQuery
    {
        private const string queryTerms = "happy quotes";

        private readonly SemaphoreSlim singleSlimLock = new SemaphoreSlim(1, 1);

        private readonly CseResource.ListRequest searchRequest;

        #region Constructor

        public HappyQuotesImagesQuery(IOptions<GoogleCustomSearchOptions> configs, ILogger<HappyQuotesImagesQuery> logger)
        {
            var customSearchService = new CustomSearchAPIService(new BaseClientService.Initializer { ApiKey = configs.Value.ApiKey });

            logger.LogInformation($"ApiKey={configs.Value.ApiKey}" + Environment.NewLine + $"SearchEngineID={configs.Value.SearchEngineID}");

            searchRequest = customSearchService.Cse.List();
            searchRequest.Cx = configs.Value.SearchEngineID;
            searchRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            searchRequest.ExactTerms = queryTerms;
        }

        #endregion

        public async Task<(bool lockTimeouted, List<Result> results)> TryExecuteAsync()
        { 
            // if lock is aquired try to await for 3 seconds then try again and return false if not success, try retriving from cache
            if (await singleSlimLock.WaitAsync(3_000))
            {
                try
                {
                    var searchResults = new List<Result>(100);

                    var count = 0;
                    do
                    {
                        searchRequest.Start = count;

                        var result = await searchRequest.ExecuteAsync();  // TODO: check if each page is different
                        searchResults.AddRange(result.Items);

                        count++;
                    } while (count < 10);

                    return (false, searchResults);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    searchRequest.Start = 0;
                    singleSlimLock.Release();
                }
            }
            else
            {
                return (true, null);
            }
        }
    }
}
