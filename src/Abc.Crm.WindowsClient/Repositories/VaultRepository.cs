using System;
using System.Collections.Generic;
using Abc.Crm.WindowsClient.Interfaces;
using Abc.Crm.WindowsClient.Models;
using JetBrains.Annotations;
using RestSharp;

namespace Abc.Crm.WindowsClient.Repositories
{
    [UsedImplicitly]
    public class VaultRepository : IVaultRepository
    {
        private readonly IRestClientFactory _restClientFactory;

        public VaultRepository(IRestClientFactory restClientFactory)
        {
            _restClientFactory = restClientFactory;
        }

        public IEnumerable<Vault> GetAll()
        {
            var client = _restClientFactory.CreateWithAuthentication();

            var request = _restClientFactory.CreateClientRequest("vaults/", Method.GET);

            var response = client.Execute<List<Vault>>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception("Something went wrong getting vaults");
            }

            return response.Data;
        }
    }
}
