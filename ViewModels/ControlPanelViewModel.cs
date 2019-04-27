using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class ControlPanelViewModel : INotifyPropertyChanged
    {
        private bool autoTextBoxChanged = false;
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void SendMessagesToSim(object parameter, bool fromTextBox = false)
        {
            autoTextBoxChanged = true;
            if (fromTextBox)
            {
                Color = "White";
            }
            Thread send_to_sim = new Thread(new ParameterizedThreadStart(DataWriterClient.Instance.SendMassages));
            send_to_sim.Start(parameter);
        }

        public ICommand _autoPilotOKCommand;
        public ICommand autoPilotOKCommand
        {
            get
            {
                return _autoPilotOKCommand ?? (_autoPilotOKCommand = new CommandHandler(() => SendMessagesToSim(textBox, true)));
            }
        }
        
        public void ClearTextBox()
        {
             TextBox1 = string.Empty;
        }
        public ICommand _autoPilotClearCommand;
        public ICommand autoPilotClearCommand
        {
            get
            {
                return _autoPilotClearCommand ?? (_autoPilotClearCommand = new CommandHandler(() => ClearTextBox()));
            }
        }

        
        
        public string textBox;
        public string TextBox1
        {
            get
            {
                return textBox;
            }
            set
            {
                textBox = value;
                color = "Salmon";
                NotifyPropertyChanged("Color");
                NotifyPropertyChanged("TextBox1");
            }
        }

        public string color;
        public string Color
        {
            get
            {
                if (string.IsNullOrEmpty(textBox))
                {
                    return "White";
                }
                return color;
            }
            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }

        public double throttle;
        public double ThrottleVal
        {
            set
            {
                throttle = value;
                string msg = "set " + "/controls/engines/current-engine/throttle " + throttle;
                SendMessagesToSim(msg);
            }
            get
            {
                return throttle;
            }
        }

        public double rudder;
        public double RudderVal
        {
            set
            {
                rudder = value;
                string msg = "set " + "/controls/flight/rudder " + rudder;
                SendMessagesToSim(msg);
            }
            get
            {
                return rudder;
            }
        }

        public double aileron;
        public double AileronVal
        {
            set
            {
                aileron = value;
                string msg = "set " + "/controls/flight/aileron " + aileron;
                SendMessagesToSim(msg);
            }
            get
            {
                return aileron;
            }
        }

        public double elevator;
        public double ElevatorVal
        {
            set
            {
                elevator = value;
                string msg = "set " + "/controls/flight/elevator " + elevator;
                SendMessagesToSim(msg);
            }
            get
            {
                return elevator;
            }
        }
    }
}
