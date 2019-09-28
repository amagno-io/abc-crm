using System.Configuration;
using System.Windows;

namespace Abc.Crm.WindowsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            MessageBox.Show(ConfigurationManager.AppSettings["amagno_host"], "a" );
            InitializeComponent();
        }
    }
}
