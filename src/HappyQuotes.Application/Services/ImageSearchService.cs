using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using HappyQuotes.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HappyQuotes.Application.Services
{
    public class ImageSearchService : IImageSearchService
    {
        public async Task<List<ImageLinkModel>> HappyQoutesSearchAsync(string queryTerms = "happy quotes")
        {
            const string apiKey = "AIzaSyBwnPErO63g8qxJHaMLOp8wmoN0ILBd9JQ";
            const string searchEngineId = "7441b6d4e6ddd40e6";

            //throw new ArgumentException($"{nameof(ArgumentException)} : {queryTerms}");

            var customSearchService = new CustomSearchAPIService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List();
            listRequest.Cx = searchEngineId;
            listRequest.ExactTerms = queryTerms;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;

            //List<Result> paging = new List<Result>();
            List<ImageLinkModel> dataModel = new List<ImageLinkModel>();
            var count = 0;
            while (dataModel != null)
            {
                Console.WriteLine($"Page {count}");
                listRequest.Start = count;

                var result = await listRequest.ExecuteAsync();

                dataModel.AddRange(result.Items.Select(r => new ImageLinkModel
                {
                    Content = r.Snippet,
                    Link = r.Link,
                    Title = r.Title,
                    Name = r.HtmlTitle
                }));

                count++;
                if (count >= 1)
                {
                    break;
                }
            }
            return dataModel;
        }
    }
}
