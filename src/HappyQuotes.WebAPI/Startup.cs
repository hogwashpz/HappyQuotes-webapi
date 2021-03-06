 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyQuotes.Application;
using HappyQuotes.Application.Options;
using HappyQuotes.WebAPI.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HappyQuotes.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Application
            services.AddApplication();
            services.Configure<GoogleCustomSearchOptions>(Configuration.GetSection(GoogleCustomSearchOptions.GoogleCustomSearch));

            // WebAPI
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HappyQuotes.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseExceptionHandler("/errors");

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(SwaggerOptions.Swagger).Bind(swaggerOptions);
            app.UseSwagger(opt => opt.RouteTemplate = swaggerOptions.JsonRoute);
            app.UseSwaggerUI(opt => opt.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description));

            app.UseHttpsRedirection();

            app.UseRouting();

            if (env.IsDevelopment() || env.EnvironmentName.Equals("Test"))
            {
                app.UseCors(configure =>
                    configure.AllowAnyOrigin()
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                    );
                // User diffrent CORS for Production
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
