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
            searchRequest.Num = 10;
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

                    var count = 1;
                    searchRequest.Start = count;
                    do
                    {
                        var result = await searchRequest.ExecuteAsync();
                        if (result.Items is not null)
                            searchResults.AddRange(result.Items);

                        count += 10;
                        searchRequest.Start = count;

                    } while (count < 11); // TODO: Temporary take just 10 results

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
