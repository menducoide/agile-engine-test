using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGallerySearch.AgileEngine.Configuration
{

    public class AgileEngineApiConfiguration : IAgileEngineApiConfiguration
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }

    }
    public interface IAgileEngineApiConfiguration
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }

    }
}
