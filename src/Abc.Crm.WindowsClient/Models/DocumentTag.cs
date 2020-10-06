using System;
using JetBrains.Annotations;

namespace Abc.Crm.WindowsClient.Models
{
    [UsedImplicitly]
    public class DocumentTag
    {
        public Guid Id { get; set; }

        public Guid TagDefinitionId { get; set; }

        public string Value { get; set; }
    }
}
