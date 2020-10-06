using System;
using System.Configuration;
using Abc.Crm.WindowsClient.Interfaces;
using JetBrains.Annotations;
using RestSharp;

namespace Abc.Crm.WindowsClient.Services
{
    [UsedImplicitly]
    public class Authenticator : IAuthenticator
    {
        [NotNull]
        private readonly IAuthToken _authToken;

        [NotNull]
        private readonly IRestClientFactory _restClientFactory;

        [NotNull]
        private readonly string _amagnoUser;

        [NotNull]
        private readonly string _amagnoPass;

        public Authenticator([NotNull] IAuthToken authToken, [NotNull] IRestClientFactory restClientFactory)
        {
            _authToken = authToken;
            _restClientFactory = restClientFactory;

            _amagnoUser = ConfigurationManager.AppSettings.Get("amagno_user") ??
                          throw new InvalidOperationException("Repository is useless without amagno user");

            _amagnoPass = ConfigurationManager.AppSettings.Get("amagno_pass") ??
                          throw new InvalidOperationException("Repository is useless without amagno pass");
        }

        public bool Login()
        {
            try
            {
                _authToken.Token = GetToken(_amagnoUser, _amagnoPass);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetToken(string amagnoUser, string amagnoPass)
        {
            var client = _restClientFactory.Create();

            var request = _restClientFactory.CreateClientRequest("token", Method.POST);

            request.AddParameter(
                "undefined",
                $"{{\"Username\":\"{amagnoUser}\",\"Password\":\"{amagnoPass}\"}}",
                ParameterType.RequestBody);

            var response = client.Execute(request);

            return !response.IsSuccessful ? "" : response.Content.Trim('"');
        }
    }
}
