using System;
using System.Collections.Generic;
using System.IO;
using Abc.Crm.WindowsClient.Models;

namespace Abc.Crm.WindowsClient.Services
{
    public class DemoCustomerDocumentRepository : ICustomerDocumentRepository
    {
        private readonly List<CustomerDocument> _customerDocuments = new List<CustomerDocument>();

        public DemoCustomerDocumentRepository()
        {
            var path = @"D:\Alle\Abc.Crm\Abc.Crm.WindowsClient\resources\";
            _customerDocuments.AddRange(new []
            {
                new CustomerDocument
                {
                    Id = Guid.NewGuid(),
                    Name = "rechnung.pdf",
                    Created = "02.01.2018",
                    Date = "15.01.2018",
                    No = "R983247",
                    Preview = File.ReadAllBytes(Path.Combine(path, "rechnung.pdf.png"))
                },
                new CustomerDocument
                {
                    Id = Guid.NewGuid(),
                    Name = "lieferschein.pdf",
                    Created = "17.12.2017",
                    Date = "22.11.2017",
                    No = "L304802",
                    Preview = File.ReadAllBytes(Path.Combine(path, "lieferschein.pdf.png"))
                }
            });
        }

        public void Add(string filename, Stream stream)
        {
            _customerDocuments.Add(new CustomerDocument
            {
                Id = Guid.NewGuid(),
                Name = filename,
                Created = DateTime.Now.ToShortDateString()
            });
        }

        public IEnumerable<CustomerDocument> GetAll()
        {
            return _customerDocuments;
        }
    }
}