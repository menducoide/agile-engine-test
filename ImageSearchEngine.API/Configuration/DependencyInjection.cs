using ImageGallerySearch.AgileEngine.Configuration;
using ImageGallerySearch.AgileEngine.Security;
using ImageGallerySearch.Common.Utils;
using ImageGallerySearch.Domain.IServices;
using ImageGallerySearch.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallerySearch.API.Configuration
{
    public static class DependencyInjection
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ImageGallerySearch.AgileEngine.Services.ImageService>();
            services.AddSingleton<Authentication>();
            services.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache,Microsoft.Extensions.Caching.Memory.MemoryCache>();
            services.AddSingleton<ICacheManager,CacheManager>();
            services.AddSingleton<IAgileEngineApiConfiguration>(sp =>
                 sp.GetRequiredService<IOptions<AgileEngineApiConfiguration>>().Value);

        }
    }
}
