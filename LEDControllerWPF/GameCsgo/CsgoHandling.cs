using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGSI;

namespace LEDControllerWPF.GameCsgo
{
    class CsgoHandling
    {
        private GameStateListener _gsl;
        private CsgoGameState _gameState = new CsgoGameState();
        private bool _firstTimeStarted = false;

        // creating GameStateListener when the game is running
        public void StartCsgoListening()
        {
            // run startup animation
            _gameState.Startup();

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

        public void Shutdown()
        {
            _gameState.ShutDown();
        }
    }
}
