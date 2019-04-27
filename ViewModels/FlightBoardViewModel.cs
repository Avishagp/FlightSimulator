using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {

        public FlightBoardViewModel()
        {
            Model.SymbolTable.Instance.mapUpdatedEvent += OnNewData;
        }

        public void OnNewData(string key, double value)
        {
            if (key.Equals("/position/longitude-deg"))
            {
                Lon = value;
            }
            else if (key.Equals("/position/latitude-deg"))
            {
                Lat = value;
            }
        }

        private double lon;
        public double Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        private double lat;
        public double Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
                NotifyPropertyChanged("Lat");
            }
        }
    }
}
