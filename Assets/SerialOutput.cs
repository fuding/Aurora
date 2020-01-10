using System;
using System.IO.Ports;

namespace Aurora.Assets
{
    public class SerialOutput
    {
        private SerialPort SerialPort;
        private byte[] message = new byte[3 + 1 + (256 * 3)];

        private string portname = null;
        private int writes = 0;
        private int led_count = 0;

        public SerialOutput()
        {
            UpdateLedCount();
            portname = Properties.Settings.Default.serial_port;
        }

        private void UpdateLedCount()
        {
            if (Properties.Settings.Default.active_top)
                led_count += Properties.Settings.Default.top_led;

            if (Properties.Settings.Default.active_bottom)
                led_count += Properties.Settings.Default.top_led;

            if (Properties.Settings.Default.active_left)
                led_count += Properties.Settings.Default.side_led;

            if (Properties.Settings.Default.active_right)
                led_count += Properties.Settings.Default.side_led;
        }

        public void FillLEDs(byte[] color_array)
        {
            message = new byte[3 + 1 + (led_count * 3)];

            int counter = 3;
            int ledShift = 0;

            //Fill cells from argument
            for (int i = 0; i < color_array.Length / 3; i++)
            {
                message[counter++] = color_array[ledShift++]; //RED
                message[counter++] = color_array[ledShift++]; //GREEN
                message[counter++] = color_array[ledShift++]; //BLUE
            }

            //Fill empty cells
            for (int i = 0; i < led_count - (color_array.Length / 3); i++)
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

                    /*
                    writes++;
                    if (writes > 50)
                    {
                        SerialPort.Close();
                        SerialPort.Open();
                        SerialPort.Write("RESET");
                    }
                    */
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
