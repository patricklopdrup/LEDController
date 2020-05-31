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
        // events for csgoHandler class to subscribe to
        public event EventHandler ProgramRunning;
        public event EventHandler ProgramShutdown;

        private DispatcherTimer _timer;
        private string _fileName;
        private bool _wasRunning;

        public ProcessListener(string FileName)
        {
            _fileName = FileName;
            SetTimer();
        }

        // check every x sec if the program is running
        public void SetTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(CheckProgramRunning);
            _timer.Interval = new TimeSpan(0,0,0,1);
            _timer.Start();
        }

        public void CheckProgramRunning(object sender, EventArgs e)
        {
            // when program is started
            if (!_wasRunning && IsProgramRunning(_fileName))
            {
                _wasRunning = true;
                OnProgramRunning();
            }
            // when program is closing
            else if (_wasRunning && !IsProgramRunning(_fileName))
            {
                _wasRunning = false;
                OnProgramShutdown();
            }
        }

        // is called when the program is starting up
        protected virtual void OnProgramRunning()
        {
            if (ProgramRunning != null)
                ProgramRunning(this, EventArgs.Empty);
        }

        // is called when the program stops again
        protected virtual void OnProgramShutdown()
        {
            ProgramShutdown?.Invoke(this, EventArgs.Empty);
        }

        // gets all processes with the FileName and returns if such a process is running
        private bool IsProgramRunning(string FileName)
        {
            Process[] processes = Process.GetProcessesByName(FileName);
            return processes.Length > 0;
        }

        
    }
}
