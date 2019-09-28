using System;

namespace Abc.Crm.WindowsClient.Models
{
    public class DocumentTag
    {
        public Guid Id { get; set; }
        public Guid TagDefinitionId { get; set; }
        public string Value { get; set; }
    }
}
