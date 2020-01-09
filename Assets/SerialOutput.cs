using System;
using System.IO.Ports;

namespace Aurora.Assets
{
    public class SerialOutput
    {
        private SerialPort SerialPort;
        byte[] message = new byte[3 + 1 + (256 * 3)];
        private string portname = null;
        private int writes = 0;

        public SerialOutput()
        {
            portname = Properties.Settings.Default.serial_port;
        }

        public void FillLEDs(byte[] leds)
        {
            message = new byte[3 + 1 + (256 * 3)];

            int counter = 3;
            int ledShift = 0;

            //Fill cells from argument
            for (int i = 0; i < leds.Length / 3; i++)
            {
                message[counter++] = leds[ledShift++]; //RED
                message[counter++] = leds[ledShift++]; //GREEN
                message[counter++] = leds[ledShift++]; //BLUE
            }

            //Fill empty cells
            for (int i = 0; i < 256 - (leds.Length / 3); i++)
            {
                message[counter++] = 0x00; //RED
                message[counter++] = 0x00; //GREEN
                message[counter++] = 0x00; //BLUE
            }
        }

        public void Send()
        {
            if (portname != null && portname != "")
            {
                try
                {
                    SerialPort = new SerialPort(portname, 115200);
                    SerialPort.Open();

                    //Preamble
                    message[0] = 0x00;
                    message[1] = 0x01;
                    message[2] = 0x02;

                    //Mode
                    message[3] = 0x00;

                    SerialPort.Write(message, 0, message.Length);

                    writes++;
                    if (writes > 50)
                    {
                        SerialPort.Close();
                        SerialPort.Open();
                        SerialPort.Write("RESET");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    if (null != SerialPort && SerialPort.IsOpen)
                    {
                        SerialPort.Close();
                        SerialPort.Dispose();
                    }
                }
            }
            else
            {
                Console.Write("Serial port name cannot be empty");
            }
        }
    }
}
