using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using FlightSimulator.Views.Windows;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class FlightPanelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private SettingsWindow settings = new SettingsWindow();

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => OnClick()));
            }
        }
        private void OnClick()
        {
            settings.Show();
        }
    }
}
