using System.Collections.Generic;

namespace Abc.Crm.WindowsClient.Models
{
    public class DocumentTags
    {
        public IEnumerable<DocumentTag> SingleLineStrings { get; set; }

        public IEnumerable<DocumentTag> Dates { get; set; }
    }
}
