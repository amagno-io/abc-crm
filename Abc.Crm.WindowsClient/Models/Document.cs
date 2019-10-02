using System;
using JetBrains.Annotations;
using RestSharp.Deserializers;

namespace Abc.Crm.WindowsClient.Models
{
    [UsedImplicitly]
    public class Document
    {
        public Guid Id { get; set; }

        public string No { get; set; }

        public string Date { get; set; }

        public string Name { get; set; }

        [DeserializeAs(Name = "CreateDate" )]
        public string Created { get; set; }

        public byte[] Preview { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}