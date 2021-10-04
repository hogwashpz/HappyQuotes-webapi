using HappyQuotes.Application.Models;
using System.Threading.Tasks;

namespace HappyQuotes.Application.Services
{
    public interface IImageSearchService
    {
        Task<ImageLinkModel> HappyQoutesSearchAsync();
    }
}
