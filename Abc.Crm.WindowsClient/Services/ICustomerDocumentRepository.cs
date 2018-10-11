using System.Collections.Generic;
using Abc.Crm.WindowsClient.Models;

namespace Abc.Crm.WindowsClient.Services
{
    public interface ICustomerDocumentRepository
    {
        IEnumerable<CustomerDocument> GetAll();
    }
}