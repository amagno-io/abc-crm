using JetBrains.Annotations;

namespace Abc.Crm.WindowsClient.Interfaces
{
    public interface IAuthToken
    {
        [CanBeNull]
        string Token { get; set; }
    }
}
