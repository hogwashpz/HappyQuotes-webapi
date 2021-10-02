using HappyQuotes.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyQuotes.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            //services.AddHttpClient();
            services.AddScoped<IImageSearchService, ImageSearchService>();
        }
    }
}
