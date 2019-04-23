using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class DataReaderServer
    {
        // Singlton.
        private static DataReaderServer instance;

        // Thread safety.
        private static readonly object padlock = new object();

        /// <summary>
        /// Private constructor.
        /// </summary>
        private DataReaderServer()
        {
        }

        /// <summary>
        /// Returns an instance of the class ListenerServer.
        /// </summary>
        public static DataReaderServer Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DataReaderServer();
                    }
                    return instance;
                }
            }
        }


        /// <summary>
        /// Starting the server.
        /// </summary>
        public static void StartServer(object parameters)
        {
            Tuple<string, int> args = (Tuple<string, int>)parameters;
            string ip = args.Item1;
            int port = args.Item2;

            string data = null;
            byte[] buffer = new byte[1024];

            // Try to stablish the remote endpoint for the socket.  
            // Local computer on port 10005.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress local = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(local, 5400);

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            TcpListener tcpListener = new TcpListener(iPEndPoint);

            // Create a TCP/IP  socket.
            tcpListener.Start();
            Socket listener = tcpListener.AcceptSocket();

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                Console.WriteLine("Connected to client.");
                data = null;

                // Start listening for connections.  
                while (true)
                {

                    /* Save our read data in a string. */
                    int bytesRec = listener.Receive(buffer);
                    data += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    //Console.WriteLine("Recieved: {0}", Encoding.Default.GetString(buffer));

                    /* Check if this is a full set of data. */
                    if (data.Contains('\n'))
                    {
                        /* Get the set of data. */
                        string till_new_line = data.Substring(0, data.IndexOf('\n'));
                        data = data.Substring(data.IndexOf('\n'));
                        //Console.WriteLine("DATA: {0}", till_new_line);

                        /* Making sure there are no "leftovers" from data. */
                        while (!string.IsNullOrEmpty(data) && data[0] == '\n')
                        {
                            data = data.Substring(1);
                        }

                        /* Update map If we got new data. */
                        if (!string.IsNullOrEmpty(till_new_line))
                        {
                            DataReaderServer.updateMap(till_new_line);
                        }

                        till_new_line = string.Empty;
                    }
                }
                tcpListener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();


        }

        /// <summary>
        /// Update the map with the client's data.
        /// </summary>
        /// <param name="str">A string sent by the client.</param>
        private static void updateMap(string str)
        {
            // Get symbol table instance.
            SymbolTable table = SymbolTable.Instance;

            // Split the string to values
            string[] split = str.Split(',');
            double[] values = new double[23];

            for (int i = 0; i < split.Length; i++)
            {
                values[i] = Convert.ToDouble(split[i]);
            }

            /* Update the map. */
            table.setValueOf("/instrumentation/airspeed-indicator/indicated-speed-kt", values[0]);
            table.setValueOf("/instrumentation/altimeter/indicated-altitude-ft", values[1]);
            table.setValueOf("/instrumentation/altimeter/pressure-alt-ft", values[2]);
            table.setValueOf("/instrumentation/attitude-indicator/indicated-pitch-deg", values[3]);
            table.setValueOf("/instrumentation/attitude-indicator/indicated-roll-deg", values[4]);
            table.setValueOf("/instrumentation/attitude-indicator/internal-pitch-deg", values[5]);
            table.setValueOf("/instrumentation/attitude-indicator/internal-roll-deg", values[6]);
            table.setValueOf("/instrumentation/encoder/indicated-altitude-ft", values[7]);
            table.setValueOf("/instrumentation/encoder/pressure-alt-ft", values[8]);
            table.setValueOf("/instrumentation/gps/indicated-altitude-ft", values[9]);
            table.setValueOf("/instrumentation/gps/indicated-ground-speed-kt", values[10]);
            table.setValueOf("/instrumentation/gps/indicated-vertical-speed", values[11]);
            table.setValueOf("/instrumentation/heading-indicator/indicated-heading-deg", values[12]);
            table.setValueOf("/instrumentation/magnetic-compass/indicated-heading-deg", values[13]);
            table.setValueOf("/instrumentation/slip-skid-ball/indicated-slip-skid", values[14]);
            table.setValueOf("/instrumentation/turn-indicator/indicated-turn-rate", values[15]);
            table.setValueOf("/instrumentation/vertical-speed-indicator/indicated-speed-fpm", values[16]);
            table.setValueOf("/controls/flight/aileron", values[17]);
            table.setValueOf("/controls/flight/elevator", values[18]);
            table.setValueOf("/controls/flight/rudder", values[19]);
            table.setValueOf("/controls/flight/flaps", values[20]);
            table.setValueOf("/controls/engines/current-engine/throttle", values[21]);
            table.setValueOf("/engines/engine/rpm", values[22]);
        }
    }
}
