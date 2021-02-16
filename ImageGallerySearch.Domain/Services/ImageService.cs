using ImageGallerySearch.Common.Enums;
using ImageGallerySearch.Common.Utils;
using ImageGallerySearch.DataTransferObjects;
using ImageGallerySearch.Domain.IServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallerySearch.Domain.Services
{
    public class ImageService : IImageService
    {
        private readonly AgileEngine.Services.ImageService ImageServiceAPI;
        private readonly ICacheManager cache;

        public ImageService(AgileEngine.Services.ImageService imageServiceAPI, ICacheManager cache)
        {
            ImageServiceAPI = imageServiceAPI;
            this.cache = cache;
        }

        public async Task<ImageListResponseDTO> List(string page = "", string limit = "")
        {
            return await ImageServiceAPI.List(page, limit);
        }
        public async Task<PictureGetResponseDTO> Get(string id)
        {
            IList<PictureGetResponseDTO> cachedImages;
            PictureGetResponseDTO cachedImage;
            if (!cache.TryGetValue(CacheKeys.Picture, out cachedImages))
            {
                cachedImages = new List<PictureGetResponseDTO>();
                cache.Set(CacheKeys.Picture, cachedImages, TimeSpan.FromMinutes(15));
            }
            cachedImage = cachedImages.FirstOrDefault(s => s.id == id);
            if (cachedImage == null)
            {
                cachedImage = await ImageServiceAPI.Get(id);
                cachedImages.Add(cachedImage);
                cache.Set(CacheKeys.Picture, cachedImages, TimeSpan.FromMinutes(15));
            }
            return cachedImage;
        }
        public IList<PictureGetResponseDTO> Search(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            IList<PictureGetResponseDTO> cachedImages;
            if (cache.TryGetValue(CacheKeys.Picture, out cachedImages))
            {
                return cachedImages.Where(s => s.author.ToLower().Contains(searchTerm)
                || s.camera.ToLower().Contains(searchTerm)
                || s.tags.ToLower().Contains(searchTerm)
                || s.id.ToLower().Contains(searchTerm)
                ).ToList();
            }
            return new List<PictureGetResponseDTO>();
        }
    }
}
