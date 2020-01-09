using System;
using System.IO.Ports;

namespace Aurora.Assets
{
    public class SerialOutput
    {
        private string serialPortName = null;
        private SerialPort serialPort;

        private int writes = 0;

        //3 - wellcome,  1 - mode, 256 * 3 - led count  (768)
        byte[] message = new byte[3 + 1 + (256 * 3)];

        public SerialOutput()
        {
            serialPortName = Properties.Settings.Default.serial_port;

            if(serialPortName != null && serialPortName != "")
                serialPort = new SerialPort(serialPortName, 115200);
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

        public byte[] Message()
        {
            //Preamble
            message[0] = 0x00;
            message[1] = 0x01;
            message[2] = 0x02;

            //Mode
            message[3] = 0x00;

            return message;
        }

        public void Send()
        {
            if (serialPortName != null && serialPortName != "")
            {
                try
                {
                    serialPort = new SerialPort(serialPortName, 115200);
                    serialPort.Open();

                    byte[] outputStream = Message();
                    serialPort.Write(outputStream, 0, outputStream.Length);

                    writes++;
                    if (writes > 50)
                    {
                        serialPort.Close();
                        serialPort.Open();
                        serialPort.Write("RESET");
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                finally
                {
                    if (null != serialPort && serialPort.IsOpen)
                    {
                        serialPort.Close();
                        serialPort.Dispose();
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
