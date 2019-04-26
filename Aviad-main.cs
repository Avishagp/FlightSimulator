using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    class Aviad_main
    {
        public static void Hello()
        {
            Model.DataWriterClient.Instance.StartClient("127.0.0.1", 5402);
        }
    }
}
