/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Abc.Crm.WindowsClient"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Abc.Crm.WindowsClient.Factories;
using Abc.Crm.WindowsClient.Interfaces;
using Abc.Crm.WindowsClient.Models;
using Abc.Crm.WindowsClient.Repositories;
using Abc.Crm.WindowsClient.Services;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Abc.Crm.WindowsClient.ViewModel
{
    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            SimpleIoc.Default.Register<IAuthToken>(() => new AuthToken());
            SimpleIoc.Default.Register<IAuthenticator, Authenticator>();
            SimpleIoc.Default.Register<IRestClientFactory, RestClientFactory>();
            SimpleIoc.Default.Register<IDocumentRepository, DocumentRepository>();
            SimpleIoc.Default.Register<IVaultRepository, VaultRepository>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}