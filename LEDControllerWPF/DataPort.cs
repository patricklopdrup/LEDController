using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Mime;

namespace LEDControllerWPF
{
    class DataPort
    {
        private string _portName = "COM3";
        private int _baudRate = 9600;

        private SerialPort port;

        public DataPort()
        {
            port = new SerialPort(_portName, _baudRate);
            

        }

        public void SendData()
        {
            try
            {
                // if the port is closed - open it.
                if (!port.IsOpen)
                {
                    port.Open();
                    Console.WriteLine($"open? {port.IsOpen}");
                }

                // TEST: send data over COM port
                while (true)
                {
                    var hej = Console.ReadLine();
                    if (hej == "q")
                    {
                        port.Close();
                        Environment.Exit(0);
                    }
                    port.WriteLine(hej);
                }
                
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

    }
}
