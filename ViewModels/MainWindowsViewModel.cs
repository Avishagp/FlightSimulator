using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator.ViewModels
{
    class MainWindowsViewModel : BaseNotify
    {
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Close connections.
            Model.DataReaderServer.Instance.stopServer = true;
            Model.DataWriterClient.Instance.CloseConnection();

            // Close windows
            Application.Current.Shutdown();
        }
    }
}
