using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;
using System.Windows;

namespace Aurora.Assets
{
    public class SerialOutput
    {
        private readonly byte[] preamble = { 0x00, 0x01, 0x02 };
        private string serialPortName = null;
        
        private BackgroundWorker bgWorker = new BackgroundWorker();
        private SerialPort serialPort;

        byte ledDir;

        public SerialOutput()
        {
            Init();
        }

        public bool isOpen()
        {
            return serialPort.IsOpen;
        }

        private void Init()
        {
            serialPortName = Properties.Settings.Default.serial_port;
            serialPort = new SerialPort(serialPortName, 115200);

            if (Properties.Settings.Default.led_dir == 0)
                ledDir = 0x00;
            else
                ledDir = 0x01;
        }

        public byte[] Message()
        {
            byte[] message;

            //3 - wellcome,  1 - mode, 128 * 3 - led count  (384)
            message = new byte[3 + 1 + (128 * 3)];

            //Preamble
            message[0] = 0x00;
            message[1] = 0x01;
            message[2] = 0x02;

            //Mode
            message[3] = 0x00;

            int counter = 3;
            for (int i = 0; i < 128; i++)
            {
                if(i <= 5)
                {
                    message[counter++] = 0x00; //RED
                    message[counter++] = 0x00; //GREEN
                    message[counter++] = 0xFF; //BLUE
                }
                else if(i <= 10)
                {
                    message[counter++] = 0x00; //RED
                    message[counter++] = 0xFF; //GREEN
                    message[counter++] = 0x00; //BLUE
                }
                else
                {
                    message[counter++] = 0xFF; //RED
                    message[counter++] = 0x00; //GREEN
                    message[counter++] = 0x00; //BLUE
                }

            }

            return message;
        }

        public void Send()
        {
            try
            {
                serialPort = new SerialPort(serialPortName, 115200);
                serialPort.Open();

                byte[] outputStream = Message();
                serialPort.Write(outputStream, 0, outputStream.Length);
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
    }
}
