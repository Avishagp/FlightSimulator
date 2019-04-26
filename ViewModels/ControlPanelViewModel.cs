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

        public void SendMessagesToSim(object parameter)
        {
            autoTextBoxChanged = true;
            Color = "White";
            Thread send_to_sim = new Thread(new ParameterizedThreadStart(DataWriterClient.Instance.SendMassages));
            send_to_sim.Start(parameter);
        }
        public ICommand _autoPilotOKCommand;
        public ICommand autoPilotOKCommand
        {
            get
            {
                return _autoPilotOKCommand ?? (_autoPilotOKCommand = new CommandHandler(() => SendMessagesToSim(textBox)));
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
    }
}
