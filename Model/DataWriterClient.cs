using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FlightSimulator.Model
{
    class DataWriterClient
    {
        private static DataWriterClient instance;
        private TcpClient tcpClient;
        public bool isConnected { get; private set; }
        private static readonly object padlock = new object();

        /// <summary>
        /// Returns an instance of the class DataWriterClient.
        /// </summary>
        public static DataWriterClient Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = new DataWriterClient();
                }
                return instance;
            }
        }

        private DataWriterClient()
        {
            tcpClient = null;
            isConnected = false;
        }

        /// <summary>
        /// Starts a client, connects to a server using sent ip:port.
        /// </summary>
        /// <param name="ip">Server IP address.</param>
        /// <param name="port">Server port.</param>
        public void StartClient(string ip, int port)
        {

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            // If this is not the first run, check status:
            if (isConnected)
            {
                // If we are already connected to this IP:Port, abort.
                if (tcpClient != null && tcpClient.Client.RemoteEndPoint.Equals(iPEndPoint))
                {
                    return;
                }
                // If we are connected to a diffrent IP:Port, stop client and continue.
                else
                {
                    tcpClient.Close();
                    isConnected = false;
                    tcpClient.Dispose();
                    tcpClient = null;
                }
            }

            // Create a TCP/IP  socket.
            tcpClient = new TcpClient();
            tcpClient.Connect(iPEndPoint);
            isConnected = true;
            tcpClient.NoDelay = true;
        }

        /// <summary>
        /// Send a string to the server.
        /// </summary>
        /// <param name="msg">The massage to send.</param>
        public void SendMassage(string msg)
        {
            if (tcpClient == null)
            {
                throw new NullReferenceException("There isn't a connection active.\n");
            }
            if (string.IsNullOrEmpty(msg))
            {
                throw new NullReferenceException("There isn't a massage to send.\n");
            }

            // Encode the data string into a byte array.  
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

            // Send the data through the socket.  
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        /// <summary>
        /// Send a list of massages to the sever.
        /// </summary>
        /// <param name="parameters">A list of strings.</param>
        public void SendMassages(object parameters)
        {
            if (tcpClient == null)
            {
                throw new NullReferenceException("There isn't a connection active.\n");
            }

            lock(padlock)
            {
                IList<string> msg_list = (IList<string>)parameters;
                if (msg_list.Count == 0)
                {
                    throw new NullReferenceException("There isn't a massage to send.\n");
                }
                foreach (string msg in msg_list)
                {
                    SendMassage(msg);
                }
            }
        }

        /// <summary>
        /// Closes the connection to the server.
        /// </summary>
        public void CloseConnection()
        {
            if (tcpClient == null)
            {
                return;
            }
            tcpClient.Close();
            tcpClient.Dispose();
            tcpClient = null;
            isConnected = false;
        }

        private bool ValidateIPv4(string ip)
        {
            // Checking we didnt got an empty string.
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            else
            {
                // Checking we have four segments.
                string[] split = ip.Split('.');
                if (split.Length != 4)
                {
                    return false;
                }
                else
                {
                    // Checking each is valid
                    byte temp;
                    foreach (string s in split)
                    {
                        if (!byte.TryParse(s, out temp))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        private bool ValidatePort(int port)
        {
            return (port > 0);
        }
    }
}
