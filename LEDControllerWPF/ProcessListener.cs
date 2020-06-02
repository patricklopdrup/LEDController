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
    // allows us to send the name of the process to the classes that subscribe to us
    class ProcessEventArgs : EventArgs
    {
        public string processName { get; set; }
    }

    // we need the name and the running status of a given program
    class ProcessObject
    {
        public string Name { get; set; }
        public bool IsRunning { get; set; }

        public ProcessObject(string name)
        {
            Name = name;
            IsRunning = false;
        }

        public override string ToString()
        {
            return Name + ": " + IsRunning;
        }
    }

    class ProcessListener
    {
        // events for csgoHandler class to subscribe to
        public event EventHandler<ProcessEventArgs> ProgramRunning;
        public event EventHandler<ProcessEventArgs> ProgramShutdown;

        private DispatcherTimer _timer;
        private List<ProcessObject> _games;

        // we only look at one program at a time
        private bool _allreadyRunning = false;

        public ProcessListener(List<ProcessObject> games)
        {
            _games = games;
            SetTimer();
        }

        // check every x sec if the program is running
        public void SetTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(CheckGames);
            _timer.Interval = new TimeSpan(0,0,0,1);
            _timer.Start();
        }

        // check running status of all the games in out List
        public void CheckGames(object sender, EventArgs e)
        {
            foreach (var game in _games)
            {
                //Console.WriteLine($"checking: {game.Name}: running? {game.IsRunning}");
                CheckProgramRunning(game);
            }
        }

        // allow only one program to send OnProgramRunning at a time with _allreadyRunning bool
        public void CheckProgramRunning(ProcessObject game)
        {
            // Program starting
            if (!_allreadyRunning && !game.IsRunning && IsProgramRunning(game.Name))
            {
                game.IsRunning = true;
                _allreadyRunning = true;
                Console.WriteLine(game.ToString());
                OnProgramRunning(game);
            }
            // Program shutting down
            // when the program that started up first shuts down, we change _allreadyRunning to false
            // so another program can be our priority
            else if (game.IsRunning && !IsProgramRunning(game.Name))
            {
                game.IsRunning = false;
                _allreadyRunning = false;
                OnProgramShutdown(game);
            }
        }

        // is called when the program is starting up
        protected virtual void OnProgramRunning(ProcessObject game)
        {
            if (ProgramRunning != null)
                ProgramRunning(this, new ProcessEventArgs() {processName = game.Name});
        }

        // is called when the program stops again
        protected virtual void OnProgramShutdown(ProcessObject game)
        {
            ProgramShutdown?.Invoke(this, new ProcessEventArgs() {processName = game.Name});
        }

        // gets all processes with the FileName and returns if such a process is running
        private bool IsProgramRunning(string FileName)
        {
            Process[] processes = Process.GetProcessesByName(FileName);
            return processes.Length > 0;
        }

        
    }
}
