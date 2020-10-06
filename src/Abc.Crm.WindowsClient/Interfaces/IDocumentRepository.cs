using System;
using System.Collections.Generic;
using Abc.Crm.WindowsClient.Models;
using JetBrains.Annotations;

namespace Abc.Crm.WindowsClient.Interfaces
{
    public interface IDocumentRepository
    {
        [NotNull, ItemNotNull]
        IEnumerable<Document> GetAll([NotNull] string customerId, Guid vaultId);

        void Set(Guid vaultId, [NotNull] string filePath);
    }
}