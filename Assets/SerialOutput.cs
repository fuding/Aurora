using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;

namespace Aurora.Assets
{
    class SerialOutput : IDisposable
    {
        private readonly byte[] preamble = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };
        private BackgroundWorker bgWorker = new BackgroundWorker();

        private SerialPort serialPort;

        public byte[] Message()
        {
            byte[] message;
            int counter = preamble.Length;

            //message = new byte[preamble.Length + (15 * 1 * 3)];
            message = new byte[preamble.Length];
            Buffer.BlockCopy(preamble, 0, message, 0, preamble.Length);
            /*
            for (int i = 0; i < 15; i++)
            {
                Buffer.BlockCopy(new byte[255, 255, 255], 0, message, 0, 3);
            }
            */
            return message;
        }

        public void Send()
        {
            try
            {
                serialPort = new SerialPort("COM3", 115200);
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

        public void Start()
        {
            if (!bgWorker.IsBusy)
                bgWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            if (bgWorker.IsBusy)
                bgWorker.CancelAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Stop();
        }
    }
}
