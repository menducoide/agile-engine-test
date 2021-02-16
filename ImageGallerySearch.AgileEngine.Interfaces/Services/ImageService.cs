using ImageGallerySearch.AgileEngine.Configuration;
using ImageGallerySearch.AgileEngine.Security;
using ImageGallerySearch.DataTransferObjects;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallerySearch.AgileEngine.Services
{
    public class ImageService : BaseService
    {
        public ImageService(IAgileEngineApiConfiguration configuration, Authentication authentication) : base(configuration, authentication)
        {
        }

        public async Task<ImageListResponseDTO> List(string page = "", string limit = "")
        {
            try
            {
                RestRequest request = new RestRequest("images", Method.GET);
                if (!string.IsNullOrEmpty(page.Trim()))
                {
                    request.AddParameter("page", page);
                }
                if (!string.IsNullOrEmpty(limit.Trim()))
                {
                    request.AddParameter("limit", limit);
                }
                var result = await ExecuteAsync<ImageListResponseDTO>(request);
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
          
        }

        public async Task<PictureGetResponseDTO> Get(string id)
        {
            try
            {
                RestRequest request = new RestRequest($"images/{id}", Method.GET);
                 var result = await ExecuteAsync<PictureGetResponseDTO>(request);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
