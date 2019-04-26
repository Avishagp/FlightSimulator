using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.Model
{
    class AutoPilot_SendMassages_Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DataWriterClient.Instance.isConnected;
        }

        public void Execute(object parameter)
        {
            Thread send_to_sim = new Thread(new ParameterizedThreadStart(DataWriterClient.Instance.SendMassages));
            send_to_sim.Start(parameter);
        }
    }
}