using ImageGallerySearch.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallerySearch.Domain.IServices
{
   public interface IImageService
    {
        Task<ImageListResponseDTO> List(string page = "", string limit = "");
        Task<PictureGetResponseDTO> Get(string id);
        IList<PictureGetResponseDTO> Search(string searchTerm);
    }
}
