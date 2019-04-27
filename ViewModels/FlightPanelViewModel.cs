using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using FlightSimulator.Views.Windows;
using FlightSimulator.Model;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    class FlightPanelViewModel : BaseNotify
    { 
        private SettingsWindow settings = new SettingsWindow();

        private ICommand _PanelSettingsCommand;
        public ICommand PanelSettingsCommand
        {
            get
            {
                return _PanelSettingsCommand ?? (_PanelSettingsCommand = new CommandHandler(() => PanelSettings()));
            }
        }
        private void PanelSettings()
        {
            settings.Show();
        }

        private ICommand _PanelConnectCommand;
        public ICommand PanelConnectCommand
        {
            get
            {
                return _PanelConnectCommand ?? (_PanelConnectCommand = new CommandHandler(()=> PanelConnect()));
            }
        }
        private void PanelConnect()
        {
            //TODO Loading screen?

            // Get server and client instances.
            DataWriterClient client = DataWriterClient.Instance;
            DataReaderServer server = DataReaderServer.Instance;

            // Get connection's settings.
            int flightInfoPort    = ApplicationSettingsModel.Instance.FlightInfoPort;
            int flightCommandPort = ApplicationSettingsModel.Instance.FlightCommandPort;
            string FlightServerIP = ApplicationSettingsModel.Instance.FlightServerIP;

            // Check connection status
            bool isClientConnected = client.isConnected;
            bool isServerConnected = server.isRunning;

            // TODO If connected check if it's for the same IPEndPoint.

            #region Server
            // If already connected to this client, don't do anything.
            if (!server.isConnectedToEndPoint(FlightServerIP, flightInfoPort))
            {
                // If server connected, stop it.
                if (isServerConnected)
                {
                    server.stopServer = true;
                    // Wait for server to stop.
                    while (server.isRunning)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
                Thread serverThread = new Thread(new ParameterizedThreadStart(server.StartServer));
                serverThread.Start(new Tuple<string, int>(FlightServerIP, flightInfoPort));
            }

            #endregion
            #region Client
            if (!client.isConnectedToEndPoint(FlightServerIP, flightCommandPort))
            {
                // Wait for server to open
                while (!server.isRunning)
                {
                    System.Threading.Thread.Sleep(100);
                }
                // If client connected, stop it.
                if (isClientConnected)
                {
                    client.CloseConnection();
                }
                // Start client.
                client.StartClient(FlightServerIP, flightCommandPort);
            }
            #endregion
        }

    }
}
