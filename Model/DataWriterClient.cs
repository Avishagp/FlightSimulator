using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FlightSimulator.Model
{
    class DataWriterClient
    {
        private TcpClient tcpClient;

        public DataWriterClient()
        {
            tcpClient = null;
        }

        public void StartClient(string ip, int port)
        {

            // If port or ip are unvalid abort.
            if (!ValidateIPv4(ip) || !ValidatePort(port))
            {
                throw new ArgumentException("IP or port are unvalid.\n");
            }

            // If this is not the first run, check status:
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient.Dispose();
                tcpClient = null;
            }

            // Create a TCP/IP  socket.
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpClient = new TcpClient();
            tcpClient.Connect(iPEndPoint);
            tcpClient.NoDelay = true;
            Console.WriteLine("Client connected");
        }

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

            Console.WriteLine("Massage \"{0}\" of size {1}",
                    msg, data.Length);
        }

        public void CloseConnection()
        {
            if (tcpClient == null)
            {
                throw new NullReferenceException("There isn't a connection active.\n");
            }
            tcpClient.Close();
            tcpClient.Dispose();
            tcpClient = null;
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
