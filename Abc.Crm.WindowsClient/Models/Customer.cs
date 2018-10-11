namespace Abc.Crm.WindowsClient.Models
{
    public class Customer
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public byte[] Logo { get; set; }
    }
}