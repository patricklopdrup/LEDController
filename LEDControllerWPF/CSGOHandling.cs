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
    class CsgoHandling
    {
        private GameStateListener _gsl;
        private ProcessListener _processListener;
        private CsgoGameState _gameState = new CsgoGameState();
        private string _csgoFileName = "csgo";
        private bool _firstTimeStarted = false;

        public CsgoHandling()
        {
            _processListener = new ProcessListener(_csgoFileName);
            // subscribe to methods in ProcessListener
            _processListener.ProgramRunning += OnProgramRunning;
            _processListener.ProgramShutdown += OnProgramShutdown;

        }

        // when the program is started
        public void OnProgramRunning(object sender, EventArgs e)
        {
            Console.WriteLine("Program startet");
            StartCsgoListening();
        }

        // when the program is shutdown again
        public void OnProgramShutdown(object sender, EventArgs e)
        {
            Console.WriteLine("Program stoppet");
        }


        // creating GameStateListener when the game is running
        private void StartCsgoListening()
        {
            // only start the first time otherwise it crashes
            if (!_firstTimeStarted)
            {
                _firstTimeStarted = true;
                _gsl = new GameStateListener(3000);

                _gsl.NewGameState += new NewGameStateHandler(OnNewGameState);
                if (!_gsl.Start())
                {
                    Environment.Exit(0);
                }
                Console.WriteLine("Listening...");
            }
        }

        // call for gamestates. E.g. bomb explotion and flash
        public void OnNewGameState(GameState gs)
        {
            _gameState.GameStates(gs);
        }

    }
}
