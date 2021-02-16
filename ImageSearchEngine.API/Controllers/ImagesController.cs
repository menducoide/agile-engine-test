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
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;
 
        public ImagesController(IImageService imageService )
        {
            this.imageService = imageService;
         }



        /// <summary>
        /// GET the images list from Agile Engine
        /// </summary>
        /// <returns>a list of images, otherwise returns an error</returns>
        /// <response code="200">Returns a list of images </response>
        /// <response code="400">If page or limit not contain integer value</response>
        /// <response code="500">If Agile Engine's server returned an error</response>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "page", "limit" })]
        public async Task<ActionResult> List(int? page = null, int? limit = null)
        {
            try
            {
                var imageData = await this.imageService.List(page?.ToString() ?? "", limit?.ToString() ?? "");

                return Ok(imageData);
            }
            catch (AgileEngineException aex)
            {
                return StatusCode(500, aex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Improper API configuration");
            }
        }
        /// <summary>
        /// GET the images list from Agile Engine
        /// </summary>
        /// <returns>an image by id, otherwise returns an error</returns>
        /// <response code="200">Return an image by id</response>
        /// <response code="400">If id has a null value</response>
        /// <response code="500">If Agile Engine's server returned an error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> List(string id)
        {
            try
            {
                var cachedImage = await this.imageService.Get(id);
                return Ok(cachedImage);
            }
            catch (AgileEngineException aex)
            {
                return StatusCode(500, aex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Improper API configuration");
            }
        }

    }
}
