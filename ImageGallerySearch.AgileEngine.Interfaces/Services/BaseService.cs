using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ImageGallerySearch.AgileEngine.Configuration;
using ImageGallerySearch.AgileEngine.Security;
using ImageGallerySearch.Common.Exceptions;
using ImageGallerySearch.DataTransferObjects;
using Newtonsoft.Json;
using RestSharp;
namespace ImageGallerySearch.AgileEngine.Services
{
    public class BaseService
    {
        protected readonly IAgileEngineApiConfiguration Configuration;
        protected IRestClient Client { get; set; }

        private readonly Authentication Authentication;
        private AuthenticationTokenDTO AuthenticationToken { get; set; }

        private int RetryAuthentication { get; set; }

        public BaseService(IAgileEngineApiConfiguration configuration, Authentication authentication)
        {
            Configuration = configuration;
            Authentication = authentication;
            AuthenticationToken = authentication.GetAuthenticationToken().Result;
            Client = new RestClient(configuration.Url);
            Client.AddDefaultHeader("Authorization", $"bearer {AuthenticationToken.Token}");
            int retry;
            if (int.TryParse(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT_AGILEENGINE_API_RETRYAUTHENTICATION"), out retry))
            {
                RetryAuthentication = retry;
            }
            else
            {
                RetryAuthentication = 0;
            }
        }

        protected async Task<T> ExecuteAsync<T>(IRestRequest request) where T : class
        {
            request.AddParameter("Authorization", "Bearer " + this.AuthenticationToken.Token, ParameterType.HttpHeader);
            IRestResponse<T> response = await Client.ExecuteAsync<T>(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (RetryAuthentication > 0)
                {
                    AuthenticationToken = Authentication.GetAuthenticationToken().Result;
                    RetryAuthentication--;
                    return this.ExecuteAsync<T>(request).Result;
                }
                else
                {
                    throw new AgileEngineException("Unauthorized");
                }
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                throw new AgileEngineException(response.Content);
            }
        }


    }
}
