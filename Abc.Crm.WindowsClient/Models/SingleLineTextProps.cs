using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abc.Crm.WindowsClient.Models
{
    public class DocumentTags
    {
        public IEnumerable<DocumentTag> SingleLineStrings { get; set; }

        public IEnumerable<DocumentTag> Dates { get; set; }
    }
}
