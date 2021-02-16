using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImageGallerySearch.Common.Enums;
using ImageGallerySearch.Common.Exceptions;
using ImageGallerySearch.DataTransferObjects;
using ImageGallerySearch.Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ImageGallerySearch.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IImageService imageService;
        public SearchController( IImageService imageService)
        {
             this.imageService = imageService;
        }
        /// <summary>
        /// GET a list of images that contains the search value
        /// </summary>
        /// <returns>a list of images, otherwise returns an error</returns>
        /// <response code="200">Returns a list of images</response>
        /// <response code="500">If Agile Engine's server returned an error</response>
        [HttpGet("{searchTerm}")]
        public ActionResult List(string searchTerm)
        {
            try
            {
                var imagesData = this.imageService.Search(searchTerm);
                return Ok(imagesData);
            }
            catch (AgileEngineException aex)
            {
                return StatusCode(500, aex.Message);
            }
        }


    }
}
