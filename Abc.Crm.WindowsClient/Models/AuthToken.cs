using Abc.Crm.WindowsClient.Interfaces;

namespace Abc.Crm.WindowsClient.Models
{
    public class AuthToken : IAuthToken
    {
        public string Token { get; set; }
    }
}
