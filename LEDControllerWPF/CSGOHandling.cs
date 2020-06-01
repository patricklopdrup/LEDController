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
        private bool _firstTimeStarted = false;

        // all the game titles we want to look at. Creating List in constructor
        private string[] gameTitles = {"calculator", "notepad" };
        private List<ProcessRunningResult> games;


        public CsgoHandling()
        {
            games = new List<ProcessRunningResult>();
            // creating ProcessRunningResults of all the gametitles
            foreach (var gameTitle in gameTitles)
            {
                games.Add(new ProcessRunningResult(gameTitle));
            }
            _processListener = new ProcessListener(games);

            // subscribe to methods in ProcessListener
            _processListener.ProgramRunning += OnProgramRunning;
            _processListener.ProgramShutdown += OnProgramShutdown;
        }

        // when the program is started
        public void OnProgramRunning(object sender, ProcessEventArgs e)
        {
            Console.WriteLine($"Program startet: {e.processName}");
            StartCsgoListening();
        }

        // when the program is shutdown again
        public void OnProgramShutdown(object sender, ProcessEventArgs e)
        {
            Console.WriteLine($"Program stoppet: {e.processName}");
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
