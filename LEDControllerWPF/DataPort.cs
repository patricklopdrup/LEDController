using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Mime;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LEDControllerWPF
{
    class DataPort
    {
        private SerialPort _port;
        private int _baudRate = 9600;

        private string[] hej = new string[] {"test1", "test2", "test3", "COM3"};

        // get which port is selected
        private static string _selectedPort = "COM3";
        public string SelectedPort
        {
            get => _selectedPort; 
            set => _selectedPort = value;
        }

        // UI binding to AvailablePorts.
        public string[] AvailablePorts
        {
            //get => SerialPort.GetPortNames();
            get => hej;
        }

        public DataPort()
        {
            // init _port
            _port = new SerialPort(_selectedPort, _baudRate);
        }

        public void SendData()
        {
            try
            {
                // if the _port is closed - open it.
                if (!_port.IsOpen)
                {
                    _port.Open();
                    Console.WriteLine($"open? {_port.IsOpen}");
                }

                // TEST: send data over COM _port
                while (true)
                {
                    var hej = Console.ReadLine();
                    if (hej == "q")
                    {
                        _port.Close();
                        Environment.Exit(0);
                    }
                    _port.WriteLine(hej);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendData(string category, byte red, byte green, byte blue, int duration = -1)
        {
            // startup:R20G100B250:1500
            StringBuilder sb = new StringBuilder();
            string delimiter = ":";
            sb.Append(category);
            sb.Append(delimiter);
            sb.Append("R" + red);
            sb.Append("G" + green);
            sb.Append("B" + blue);
            sb.Append(delimiter);
            sb.Append(duration);

            _port.WriteLine(sb.ToString());
        }

    }
}
