using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlightSimulator.ViewModels.Windows
{
    public class SettingsWindowViewModel : BaseNotify
    {
        private ISettingsModel model;

        public SettingsWindowViewModel(ISettingsModel model)
        {
            this.model = model;
        }

        public string FlightServerIP
        {
            get { return model.FlightServerIP; }
            set
            {
                model.FlightServerIP = value;
                NotifyPropertyChanged("FlightServerIP");
            }
        }

        public int FlightCommandPort
        {
            get { return model.FlightCommandPort; }
            set
            {
                model.FlightCommandPort = value;
                NotifyPropertyChanged("FlightCommandPort");
            }
        }

        public int FlightInfoPort
        {
            get { return model.FlightInfoPort; }
            set
            {
                model.FlightInfoPort = value;
                NotifyPropertyChanged("FlightInfoPort");
            }
        }



        public void SaveSettings()
        {
            model.SaveSettings();
        }

        public void ReloadSettings()
        {
            model.ReloadSettings();
        }

        /// <summary>
        /// Upon SettingsWindow cloing, cancel closing and hide instead.
        /// Revert back to saved settings, and update View.
        /// </summary>
        /// <param name="sender">The SettingsWindow.</param>
        /// <param name="e">The Cancel Event Arguments.</param>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ((FlightSimulator.Views.Windows.SettingsWindow)sender).Hide();
            //OnCancel();
        }

        #region Commands
        #region ClickCommand
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
            model.SaveSettings();
        }
        #endregion

        #region CancelCommand
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandHandler(() => OnCancel()));
            }
        }
        private void OnCancel()
        {
            model.ReloadSettings();
            NotifyPropertyChanged("FlightServerIP");
            NotifyPropertyChanged("FlightCommandPort");
            NotifyPropertyChanged("FlightInfoPort");
        }
        #endregion
        #endregion
    }
}

