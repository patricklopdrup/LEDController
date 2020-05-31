using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSGSI;
using CSGSI.Nodes;
using System.Windows.Threading;

namespace LEDControllerWPF
{
    class CSGOHandling
    {
        private GameStateListener _gsl;
        private DispatcherTimer _startupTimer;
        private DispatcherTimer _shutdownTimer;
        private string _csgoFileName = "csgo";
        private bool _wasRunning = false;

        public CSGOHandling()
        {
            // start listening after csgo process to start
            SetTimer();
        }

        // every x sec we run our event
        public void SetTimer()
        {
            _startupTimer = new DispatcherTimer();
            _startupTimer.Tick += new EventHandler(CheckStartEvent);
            _startupTimer.Interval = new TimeSpan(0, 0, 0, 1);
            _startupTimer.Start();

            _shutdownTimer = new DispatcherTimer();
            _shutdownTimer.Tick += new EventHandler(CheckShutdownEvent);
            _shutdownTimer.Interval = new TimeSpan(0,0,0,1);
            _shutdownTimer.Start();
        }

        // checks if the program is running
        private void CheckStartEvent(Object source, EventArgs e)
        {
            if (!_wasRunning && IsCsgoRunning(_csgoFileName))
            {
                Console.WriteLine("startet");
                StartCsgoListening();
                _startupTimer.Stop();
                _shutdownTimer.Start();
            }
        }

        // check if the program has stopped
        private void CheckShutdownEvent(Object source, EventArgs e)
        {
            if (_wasRunning && !IsCsgoRunning(_csgoFileName))
            {
                _wasRunning = false;
                Console.WriteLine("stoppet");
                _shutdownTimer.Stop();
                _startupTimer.Start();
            }
        }

        // creating GameStateListener when the game is running
        private void StartCsgoListening()
        {
            _gsl = new GameStateListener(3000);
            _gsl.NewGameState += new NewGameStateHandler(OnNewGameState);
            if (!_gsl.Start())
            {
                Environment.Exit(0);
            }
            Console.WriteLine("Listening...");
        }

        public void OnNewGameState(GameState gs)
        {
            Console.WriteLine($"Health: {gs.Player.State.Health}");
            Console.WriteLine($"Money: {gs.Player.State.Money}");

            BombPlant(gs);

            int flash = FlashBang(gs);
            if (flash > 0)
            {
                Console.WriteLine($"Flash: {flash}");
            }

            Console.WriteLine($"Ammo: {gs.Player.Weapons.ActiveWeapon.AmmoClip}");
        }

        // checks if the process is running and set private bool _wasRunning
        public bool IsCsgoRunning(string FileName)
        {
            Process[] processes = Process.GetProcessesByName(FileName);
            if (!_wasRunning && processes.Length > 0)
            {
                _wasRunning = true;
            }
            Console.WriteLine($"kører: {_wasRunning}");
            return processes.Length > 0;
        }

        

        


        private void BombPlant(GameState gs)
        {
            if (gs.Round.Phase == RoundPhase.Live &&
                gs.Round.Bomb == BombState.Planted)
            {
                Console.WriteLine("Bomb has been planted.");
            }
        }

        private int FlashBang(GameState gs)
        {
            return gs.Player.State.Flashed;
        }

    }
}
