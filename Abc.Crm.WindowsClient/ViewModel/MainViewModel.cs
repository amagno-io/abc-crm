using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using Abc.Crm.WindowsClient.Interfaces;
using Abc.Crm.WindowsClient.Models;
using Abc.Crm.WindowsClient.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace Abc.Crm.WindowsClient.ViewModel
{
    [UsedImplicitly]
    public class MainViewModel : ViewModelBase
    {
        [NotNull]
        private readonly IDocumentRepository _documentRepository;

        private string _uploadFileName;

        private Customer _selectedCustomer;

        private Document _selectedDocument;

        private Vault _selectedVault;

        private ObservableCollection<Document> _documentList;

        private ObservableCollection<Vault> _vaultList;

        public string Title => $"Kunde - {SelectedCustomer.Name} ({SelectedCustomer.Number})";

        public Customer SelectedCustomer { get => _selectedCustomer; set => Set(ref _selectedCustomer, value); }

        public ObservableCollection<Document> DocumentList { get => _documentList; set => Set(ref _documentList, value); }

        public Document SelectedDocument { get => _selectedDocument; set => Set(ref _selectedDocument, value); }

        public ObservableCollection<Vault> VaultList { get => _vaultList; set => Set(ref _vaultList, value); }

        public Vault SelectedVault { get => _selectedVault; set => Set(ref _selectedVault, value); }

        public string UploadFileName { get => _uploadFileName;set => Set(ref _uploadFileName, value); }
    
        public RelayCommand UploadCommand { get; }

        public RelayCommand BrowseCommand { get; }

        public RelayCommand SearchCommand { get;  }
        
        public MainViewModel(
            [NotNull] IDocumentRepository documentRepository,
            [NotNull] IVaultRepository vaultRepository,
            [NotNull] IAuthenticator authenticator,
            [NotNull] IAuthToken authToken)
        {
            _documentRepository = documentRepository;

            var auth = authenticator.Login();

            if (!auth || authToken.Token == null)
            {
                MessageBox.Show("Login", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SearchCommand = new RelayCommand(ExecuteSearch);
            UploadCommand = new RelayCommand(ExecuteUpload);
            BrowseCommand = new RelayCommand(ExecuteBrowse);

            VaultList = new ObservableCollection<Vault>(vaultRepository.GetAll());
            if (VaultList.Any())
            {
                SelectedVault = VaultList.First();
            }

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
        }

        private void ExecuteBrowse()
        {
            var openFileDialog = new OpenFileDialog();

            var result = openFileDialog.ShowDialog();

            if (result != null && result.Value)
            {
                UploadFileName = openFileDialog.FileName;
            }
        }

        private void ExecuteUpload()
        {
            if (SelectedVault == null)
            {
                MessageBox.Show("Select a vault!");
                return;
            }

            if (string.IsNullOrEmpty(UploadFileName))
                return;

            _documentRepository.Set(_selectedVault.Id, UploadFileName);
        }

        private void ExecuteSearch()
        {
            if (SelectedVault == null)
            {
                MessageBox.Show("Select a vault!");
                return;
            }

            DocumentList = new ObservableCollection<Document>(_documentRepository.GetAll(SelectedCustomer.Number, SelectedVault.Id));
            if (DocumentList.Any())
            {
                SelectedDocument = DocumentList.First();
            }
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