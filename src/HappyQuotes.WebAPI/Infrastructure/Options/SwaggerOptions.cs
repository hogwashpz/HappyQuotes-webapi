using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyQuotes.WebAPI.Infrastructure.Options
{
    public class SwaggerOptions
    {
        public const string Swagger = "Swagger";

        public string JsonRoute { get; set; }

        public string Description { get; set; }

        public string UIEndpoint { get; set; }
    }
}
