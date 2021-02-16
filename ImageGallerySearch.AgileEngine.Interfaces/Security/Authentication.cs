using ImageGallerySearch.AgileEngine.Configuration;
using ImageGallerySearch.Common.Exceptions;
using ImageGallerySearch.DataTransferObjects;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallerySearch.AgileEngine.Security
{
    public class Authentication
    {
        private AuthenticationTokenDTO AuthenticationToken { get; set; }
        private readonly IAgileEngineApiConfiguration Configuration;

        public Authentication(IAgileEngineApiConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<AuthenticationTokenDTO> GetAuthenticationToken()
        {
            if (this.AuthenticationToken != null) return this.AuthenticationToken;
            RestClient client = new RestClient(Configuration.Url);
            RestRequest request = new RestRequest("auth", Method.POST);
            request.AddJsonBody(new { apiKey = Configuration.ApiKey });
            IRestResponse<AuthenticationTokenDTO> result = await client.ExecuteAsync<AuthenticationTokenDTO>(request);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AuthenticationToken = result.Data;
                return result.Data;
            }
            else
            {
                throw new AgileEngineException(result.Content);
            }
        }

    }
}
