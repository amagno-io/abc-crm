using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Abc.Crm.WindowsClient.Models;
using Abc.Crm.WindowsClient.Properties;
using Abc.Crm.WindowsClient.Services;
using GalaSoft.MvvmLight;

namespace Abc.Crm.WindowsClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Customer _selectedCustomer;
        private CustomerDocument _selectedDocument;
        private ObservableCollection<CustomerDocument> _documentList;

        private readonly ICustomerDocumentRepository _documentRepository;

        public string Title => $"Kunde - {SelectedCustomer.Name} ({SelectedCustomer.Number})";

        public Customer SelectedCustomer { get => _selectedCustomer; set => Set(ref _selectedCustomer, value); }

        public ObservableCollection<CustomerDocument> DocumentList { get => _documentList; set => Set(ref _documentList, value); }

        public CustomerDocument SelectedDocument { get => _selectedDocument; set => Set(ref _selectedDocument, value); }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ICustomerDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;

            SelectedCustomer = new Customer
            {
                Address = "Citykai 12",
                Country = "Deutschland",
                City = "Hamburg",
                Name = "Clean Power AG",
                Number = "SLKD1003",
                Postcode = "20457",
                Logo = ImageToByte2(Resources.cleanpower_logo)
            };

            DocumentList = new ObservableCollection<CustomerDocument>(_documentRepository.GetAll());

            SelectedDocument = DocumentList.First();
        }

        private static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}