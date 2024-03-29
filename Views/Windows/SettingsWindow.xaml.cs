﻿using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using FlightSimulator.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightSimulator.Views.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            SettingsWindowViewModel viewModel = new SettingsWindowViewModel(ApplicationSettingsModel.Instance);
            DataContext = viewModel;
            // Sign the view model to Closing event.
            Closing += viewModel.OnWindowClosing;
        }
    }
}
