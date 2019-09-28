using System;
using System.Collections.Generic;
using System.Linq;
using Abc.Crm.WindowsClient.Models;
using RestSharp;
using RestSharp.Authenticators;
using DataFormat = RestSharp.DataFormat;

namespace Abc.Crm.WindowsClient.Services
{
    public class CustomerDocumentRepository : ICustomerDocumentRepository
    {
        #region REST Helper Methods

        private IRestClient GetRestClient()
        {
            return new RestClient("http://localhost/amagnome/api/v2/");
        }

        private string GetToken()
        {
            var client = GetRestClient();
            var request = CreateRequest("token", Method.POST);
            request.AddParameter("undefined",
                "{\n\t\"Username\":\"ged@amagno.de\", \n\t\"Password\":\"hannes\"\n}",
                ParameterType.RequestBody);
            var response = client.Execute(request);

            return response.Content.Trim('"');
        }

        private RestRequest CreateRequest(string resourcePath, Method method)
        {
            var request = new RestRequest(resourcePath, method);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            return request;
        }

        private string GetLocation(IRestResponse response)
        {
            return response.Headers.SingleOrDefault(i => i.Name == "Location")?.Value.ToString();
        }

        private IRestClient GetAuthenticatedRestClient()
        {
            var client = GetRestClient();
            client.Authenticator = new JwtAuthenticator(GetToken());

            return client;
        }

        #endregion

        #region Consts

        private Guid NoTagDefId = new Guid("ead13e9b-20af-47f0-96b2-89c2704b4467");

        private Guid DateTagDefId = new Guid("57710b3c-4661-4bea-a81a-7e4c485146ad");

        #endregion

        #region Implementation of ICustomerDocumentRepository

        public IEnumerable<CustomerDocument> GetAll()
        {
            var documents = GetSearchResults();

            var client = GetAuthenticatedRestClient();

            foreach (var document in documents?.Take(5))
            {
                document.Preview = GetPreview(document.Id);

                var tags = GetDocumentsTags(document.Id);

                document.No = tags.SingleLineStrings.
                    First(i => i.TagDefinitionId == NoTagDefId).Value;
                document.Date = tags.Dates.
                    First(i => i.TagDefinitionId == DateTagDefId).Value;
            }

            return documents;
        }

        #endregion

        #region REST API Methods

        private byte[] GetPreview(Guid documentId)
        {
            var client = GetAuthenticatedRestClient();
            var request = CreateRequest($"documents/{documentId}/preview?page=1&size=large", Method.GET);
            var response = client.Execute(request);
            return response.RawBytes;
        }

        private IEnumerable<CustomerDocument> GetSearchResults()
        {
            var location = GetSearchLocation();
            var client = GetAuthenticatedRestClient();
            var request = CreateRequest(location + "/results", Method.GET);

            var response = client.Execute<List<CustomerDocument>>(request);

            return response?.Data.OrderBy(i => i.Name).ToArray();
        }

        private DocumentTags GetDocumentsTags(Guid docId)
        {
            var client = GetAuthenticatedRestClient();
            var request = CreateRequest($"documents/{docId}/tags", Method.GET);
            var response = client.Execute<DocumentTags>(request);

            return response.Data;
        }

        private string GetSearchLocation()
        {
            var client = GetAuthenticatedRestClient();
            var request = CreateRequest("documents/advanced-search", Method.POST);

            request.AddParameter("undefined", "{\r\n  \r\n  \"Count\":100,\r\n  \"Order\":\"Desc\",\r\n  " +
                "\"VaultIds\":\r\n  [\r\n    \"a1188a99-97eb-42e8-ad09-28df94518a8d\"\r\n  ],\r\n  \"Condition\":\r\n  {\r\n    " +
                "\"Type\":\"StringEqualsCondition\",\r\n    " +
                "\"TagDefinitionIds\":\r\n    [\r\n      \"c49f72a3-07fc-4ab3-aa10-046afd37c48d\"\r\n    ],\r\n    " +
                "\"Value\":\"SLKD1003\"\r\n  }\r\n}", ParameterType.RequestBody);
            var response = client.Execute(request);

            return GetLocation(response);
        }

        #endregion
    }
}