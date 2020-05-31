using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LEDControllerWPF
{
    class ProcessListener
    {
        public event EventHandler ProgramRunning;
        private DispatcherTimer _timer;
        private string _fileName;

        public ProcessListener(string FileName)
        {
            _fileName = FileName;
            SetTimer();
        }

        public void SetTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(CheckProgramRunning);
            _timer.Interval = new TimeSpan(0,0,0,1);
            _timer.Start();
        }

        public void CheckProgramRunning(Object sender, EventArgs e)
        {
            if (IsProgramRunning(_fileName))
            {
                OnProgramRunning();
            }
        }

        protected virtual void OnProgramRunning()
        {
            if (ProgramRunning != null)
                ProgramRunning(this, new EventArgs());
        }

        private bool IsProgramRunning(string FileName)
        {
            Process[] processes = Process.GetProcessesByName(FileName);
            return processes.Length > 0;
        }
    }
}
