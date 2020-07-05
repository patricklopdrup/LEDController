using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSGSI;
using CSGSI.Nodes;
using System.Windows.Threading;
using LEDControllerWPF.GameCsgo;

namespace LEDControllerWPF
{
    class GameHandling
    {
        private ProcessListener _processListener;
        private CsgoHandling _csgoHandling = new CsgoHandling();

        // all the game titles we want to look at
        enum gameTitles
        {
            calculator, notepad, csgo
        }

        private List<ProcessObject> games;

        public GameHandling()
        {
            games = new List<ProcessObject>();
            // creating ProcessObject of all the gametitles
            foreach (var gameTitle in Enum.GetNames(typeof(gameTitles)))
            {
                Console.WriteLine($"gametitle: {gameTitle}");
                games.Add(new ProcessObject(gameTitle));
            }
            // giving the list of games to check for
            _processListener = new ProcessListener(games);

            // subscribe to methods in ProcessListener
            _processListener.ProgramRunning += OnProgramRunning;
            _processListener.ProgramShutdown += OnProgramShutdown;
        }

        // when the program is started
        public void OnProgramRunning(object sender, ProcessEventArgs e)
        {
            Console.WriteLine($"Program startet: {e.processName}");
            
            // compare running program with enum and do different things
            switch (e.processName)
            {
                // csgo
                case nameof(gameTitles.csgo):
                    _csgoHandling.StartCsgoListening();
                    break;
                // notepad
                case nameof(gameTitles.notepad):
                    Console.WriteLine("Startet fra swtich");
                    break;
            }
        }

        // when the program is shutdown again
        public void OnProgramShutdown(object sender, ProcessEventArgs e)
        {
            Console.WriteLine($"Program stoppet: {e.processName}");

            switch (e.processName)
            {
                // csgo
                case nameof(gameTitles.csgo):
                    _csgoHandling.Shutdown();
                    break;
                // notepad
                case nameof(gameTitles.notepad):
                    Console.WriteLine("Stoppet fra swtich");
                    break;
            }
        }


        

    }
}
