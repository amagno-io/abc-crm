using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abc.Crm.WindowsClient.Models
{
    public class DocumentTag
    {
        public Guid Id { get; set; }
        public Guid TagDefinitionId { get; set; }
        public string Value { get; set; }
    }
}
