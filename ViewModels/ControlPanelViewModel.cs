using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace FlightSimulator.ViewModels
{
    class ControlPanelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        
        public string textBox;
        public string TextBox
        {
            get
            {
                return textBox;
            }
            set
            {
                textBox = value;
                NotifyPropertyChanged("Color");
                NotifyPropertyChanged("TextBox");
            }
        }

        public string color;
        public string Color
        {
            get
            {
                if (textBox == "" || TextBox == null)
                {
                    color = "White";
                }
                else
                {
                    color = "Salmon";
                }
                return color;
            }
        }

        
    }
}
