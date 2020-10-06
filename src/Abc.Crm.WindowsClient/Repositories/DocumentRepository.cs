using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abc.Crm.WindowsClient.Interfaces;
using Abc.Crm.WindowsClient.Models;
using JetBrains.Annotations;
using RestSharp;

namespace Abc.Crm.WindowsClient.Repositories
{
    [UsedImplicitly]
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IRestClientFactory _restClientFactory;

        private readonly Guid _noTagDefId = new Guid("ead13e9b-20af-47f0-96b2-89c2704b4467");

        private readonly Guid _dateTagDefId = new Guid("57710b3c-4661-4bea-a81a-7e4c485146ad");

        public DocumentRepository(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory;
        }

        /// <summary>
        /// API answers for Location attribute in header for new resources
        /// </summary>
        /// <param name="response">Response after creating a resource</param>
        /// <returns>Value of Location</returns>
        private static string GetLocation(IRestResponse response)
        {
            return response.Headers.SingleOrDefault(i => i.Name == "Location")?.Value.ToString();
        }

        /// <summary>
        /// Creates an empty document wrapper
        /// </summary>
        /// <param name="vaultId">Vault to use</param>
        /// <returns>Path of new document</returns>
        private string CreateDocument(Guid vaultId)
        {
            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest($"vaults/{vaultId}/documents", Method.POST);

            var body = $@"
                {{
                  ""metadata"": {{
                    ""createDate"": ""{DateTime.UtcNow:O}"",
                    ""changeDate"": ""{DateTime.UtcNow:O}"",
                    ""name"": ""HelloWorld_{DateTime.UtcNow:O}"",
                    ""size"": 100
                  }}
                }}";

            request.AddBody(body);

            var response = client.Execute<dynamic>(request);

            if (!response.IsSuccessful)
                throw new Exception("Error creating new document");

            return GetLocation(response);
        }

        /// <summary>
        /// Uploads new file to a document
        /// </summary>
        /// <param name="location">The location i.g. documents/{id}</param>
        /// <param name="filePath">Path of file to upload</param>
        private void UploadFile(string location, string filePath)
        {
            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest(location + "/file", Method.PUT, "multipart/form-data");

            request.AddHeader("Amagno-File-CreateNode-Date", DateTime.UtcNow.ToString("O"));
            request.AddHeader("Amagno-File-Change-Date", DateTime.UtcNow.ToString("O"));

            request.AddFile("file", filePath, "formData");

            var response = client.Execute<dynamic>(request);

            if (!response.IsSuccessful)
                throw new Exception("Upload failed" + response.Content);
        }

        public IEnumerable<Document> GetAll(string customerId, Guid vaultId)
        {
            var documents = GetSearchResults(customerId, vaultId).ToArray();

            foreach (var document in documents.Take(5))
            {
                document.Preview = GetPreview(document.Id);

                var tags = GetDocumentsTags(document.Id);

                document.No = tags.SingleLineStrings.FirstOrDefault(i => i.TagDefinitionId == _noTagDefId)?.Value;
                document.Date = tags.Dates.FirstOrDefault(i => i.TagDefinitionId == _dateTagDefId)?.Value;
            }

            return documents;
        }

        public void Set(Guid vaultId, string filePath)
        {
            var location = CreateDocument(vaultId);

            if(!string.IsNullOrEmpty(location))
                UploadFile(location, filePath);
        }

        private byte[] GetPreview(Guid documentId)
        {
            var client = _restClientFactory.CreateWithAuthentication();
            var request =
                _restClientFactory.CreateClientRequest($"documents/{documentId}/preview?page=1&size=large",
                    Method.GET);
            var response = client.Execute(request);
            return response.RawBytes;
        }

        private IEnumerable<Document> GetSearchResults(string customerId, Guid vaultId)
        {
            var location = GetSearchLocation(customerId, vaultId);

            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest(location + "/results", Method.GET);

            var response = client.Execute<List<Document>>(request);

            return response?.Data.OrderBy(i => i.Name).ToArray();
        }

        private DocumentTags GetDocumentsTags(Guid docId)
        {
            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest($"documents/{docId}/tags", Method.GET);

            var response = client.Execute<DocumentTags>(request);

            return response.Data;
        }

        private string[] GetTagsDefinition()
        {
            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest("documents/tag-definitions", Method.GET);

            var response = client.Execute<dynamic>(request);

            var result = new List<string>();

            foreach (var item in response.Data["singleLineStringDefinitions"])
            {
                result.Add(item["id"]);
            }

            return result.ToArray();
        }

        private string GetSearchLocation(string customerId, Guid vaultId)
        {
            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest("documents/advanced-search", Method.POST);

            var body = $@"
                {{
                    ""Count"": 100,                
                    ""Order"": ""Desc"",
                    ""VaultIds"": 
                    [ 
                        ""{vaultId}"" 
                    ],
                    ""Condition"": 
                    {{
                        ""Type"":""StringEqualsCondition"",
                        ""TagDefinitionIds"":
                        [
                            ""{string.Join("\",\"", GetTagsDefinition())}""
                        ],
                    ""Value"":""{customerId}""  
                    }}
                }}";

            request.AddBody(body);

            Debug.WriteLine(body);

            var response = client.Execute(request);

            return GetLocation(response);
        }
    }
}