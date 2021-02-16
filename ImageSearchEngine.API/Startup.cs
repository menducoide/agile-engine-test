using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallerySearch.AgileEngine.Configuration;
using ImageGallerySearch.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.Swagger;

namespace ImageGallerySearch.API
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Image Gallery Search API",
                    Description = "Image Gallery Search API for Agile Engine Test",
                    Contact = new OpenApiContact
                    {
                        Name = "Matías Mónaco",
                        Email = "mati.monaco.91@gmail.com",
                        Url = new Uri("https://linkedin.com/in/matias-monaco-00173b112"),
                    },
                });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            services.AddResponseCaching();
            services.Configure<AgileEngineApiConfiguration>(options =>
            {
                options.Url = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT_AGILEENGINE_API_URL");
                options.ApiKey = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT_AGILEENGINE_API_KEY");

            });
            services.AddDI();
            services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Image Gallery Search API V1");
            });
            app.UseCors("AllowAll");

            // allow response caching directives in the API Controllers
            app.UseResponseCaching();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
