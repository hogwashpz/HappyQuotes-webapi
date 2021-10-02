using HappyQuotes.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyQuotes.Application.Services
{
    public interface IImageSearchService
    {
        Task<List<ImageLinkModel>> HappyQoutesSearchAsync(string queryTerms = "happy quotes");
    }
}
