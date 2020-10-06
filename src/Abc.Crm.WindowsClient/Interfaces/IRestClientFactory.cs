using JetBrains.Annotations;
using RestSharp;

namespace Abc.Crm.WindowsClient.Interfaces
{
    public interface IRestClientFactory
    {
        [NotNull]
        IRestClient Create();

        [NotNull]
        IRestClient CreateWithAuthentication();

        [NotNull]
        IRestRequest CreateClientRequest([NotNull] string resourcePath, Method method);

        [NotNull]
        IRestRequest CreateClientRequest([NotNull] string resourcePath, Method method, [NotNull] string contentType);
    }
}
