using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Simulator.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _sliderValue;
        private double _transmittedValue;
        private DispatcherTimer _updateTimer;

        public MainPageViewModel()
        {
            _updateTimer          = new DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromSeconds(11);
            _updateTimer.Tick += SendValuesToIoTHub;
            _updateTimer.Start();
        }


        private async void SendValuesToIoTHub(object sender, object e)
        {
            var melding = new
            {
                deviceId = "WorkshopDevice", 
                verdi = SliderValue
            };
            var kodetMelding = JsonConvert.SerializeObject(melding);

            var message = new Message(Encoding.ASCII.GetBytes(kodetMelding));


            string connectionstring = "HostName=demo-iothub-pedro.azure-devices.net;DeviceId=WorkshopDevice;SharedAccessKey=LIB5qteYzkg5D86kG8mecesNXbCmW84exJtB2O8DdHQ=";
            var deviceClient = DeviceClient.CreateFromConnectionString(connectionstring);

            

            await deviceClient.SendEventAsync(message);

            TransmittedValue = SliderValue;
        }


        public void ShoutAbout([CallerMemberName] string propertyName  ="Unknown")
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public double SliderValue
        {
            get { return _sliderValue; }
            set { _sliderValue = value; ShoutAbout(); }
        }

        public double TransmittedValue
        {
            get { return _transmittedValue; }
            set { _transmittedValue = value; ShoutAbout(); }
        }
    }
}
