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

        private SerialPort port;

        public void Message()
        {
            byte[] outputStream;
            int counter = preamble.Length;
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
