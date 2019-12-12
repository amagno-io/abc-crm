using System;
using System.Configuration;
using Abc.Crm.WindowsClient.Interfaces;
using JetBrains.Annotations;
using RestSharp;
using RestSharp.Authenticators;

namespace Abc.Crm.WindowsClient.Factories
{
    [UsedImplicitly]
    public class RestClientFactory : IRestClientFactory
    {
        [NotNull]
        private readonly Uri _baseUri;

        [NotNull]
        private readonly IAuthToken _token;

        private const int Timeout = 5000;

        private const string RelativePath = "AmagnoMe/api/v2/";

        public RestClientFactory([NotNull] IAuthToken token)
        {
            _baseUri = new Uri(ConfigurationManager.AppSettings["amagno_host"] ?? throw new NotSupportedException("Cannot use rest client without url"));
            _token = token;
        }

        public IRestClient Create()
        {
            return new RestClient(_baseUri)
            {
                Timeout = Timeout
            };
        }

        public IRestClient CreateWithAuthentication()
        {
            return new RestClient(_baseUri)
            {
                Timeout = Timeout,
                Authenticator = new JwtAuthenticator(_token.Token)
            };
        }

        public IRestRequest CreateClientRequest(string resourcePath, Method method)
        {
            var request = new RestRequest(_baseUri + RelativePath + resourcePath, method);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;

            return request;
        }

        public IRestRequest CreateClientRequest(string resourcePath, Method method, string contentType)
        {
            var request = new RestRequest(_baseUri + RelativePath + resourcePath, method);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", contentType);

            return request;
        }
    }
}
