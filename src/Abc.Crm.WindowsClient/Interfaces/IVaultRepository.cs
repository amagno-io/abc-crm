using System.Collections.Generic;
using Abc.Crm.WindowsClient.Models;
using JetBrains.Annotations;

namespace Abc.Crm.WindowsClient.Interfaces
{
    public interface IVaultRepository
    {
        [NotNull, ItemNotNull]
        IEnumerable<Vault> GetAll();
    }
}
