using AIFAutoFillDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AIFAutoFillDB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AppHelper _appHelper;

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //if (_appHelper != null && _appHelper.LocalizationService != null)
            //{
            //    MessageBox.Show(e.Exception.Message, _appHelper.LocalizationService.GetString("App_ErrMsg_InternalErr"), MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //{
            //    MessageBox.Show(e.Exception.Message, "Circles of Trust™ Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            MessageBox.Show(e.Exception.Message, "CryptoCurrency Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex.Message, "CryptoCurrency Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //if (_appHelper != null && _appHelper.LocalizationService != null)
            //{
            //    MessageBox.Show(ex.Message, _appHelper.LocalizationService.GetString("App_ErrMsg_InternalErr"), MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //{
            //    MessageBox.Show(ex.Message, "Circles of Trust™ Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // For debugging only
            // System.Threading.Thread.Sleep(10000);
            //System.Windows.MessageBox.Show("Close this to continue...");

            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            bool isQuitNow = false;

            try
            {
                do
                {
                    _appHelper = new AppHelper();
                    _appHelper.CreateApplicationMainWindow();

                    _appHelper.Init();

                    _appHelper.PostInit();
                    _appHelper.NavTo(AppHelper.ViewID.Login);

                } while (false);

                if (isQuitNow)
                {
                    Application.Current.Shutdown();
                    return;
                }
            }
            catch (Exception tbmexception)
            {
                MessageBox.Show(tbmexception.Message);
                Application.Current.Shutdown();
            }
        }

    }
}
