﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using FlightSimulator.Model;
using System.Windows;

namespace FlightSimulator.ViewModels
{
    class ControlPanelViewModel : BaseNotify
    {

        public void SendMessagesToSim(object parameter, bool fromTextBox = false)
        {
            autoTextBoxChanged = true;
            // If there isn't a conenction active, abort.
            if (!Model.DataWriterClient.Instance.isConnected)
            {
                MessageBox.Show("Can't send commands, there isn't a connection available", 
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            
            if (fromTextBox)
            {
            	// Set color
                Color = "White";
            }

            // Send commands.
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
                string msg = "set " + "/controls/engines/current-engine/throttle " + throttle + "\r\n";
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
                string msg = "set " + "/controls/flight/rudder " + rudder + "\r\n";
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
                string msg = "set " + "/controls/flight/aileron " + aileron + "\r\n";
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
                string msg = "set " + "/controls/flight/elevator " + elevator + "\r\n";
                SendMessagesToSim(msg);
            }
            get
            {
                return elevator;
            }
        }
    }
}
