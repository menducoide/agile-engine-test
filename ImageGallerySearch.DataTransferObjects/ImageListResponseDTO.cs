using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGallerySearch.DataTransferObjects
{
  public  class ImageListResponseDTO
    {
        public PictureListResponseDTO[] pictures { get; set; }
        public int page { get; set; }
        public int pageCount  { get; set; }
        public bool hasMore { get; set; }
    }
}
