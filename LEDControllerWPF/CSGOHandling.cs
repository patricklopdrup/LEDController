using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSGSI;
using CSGSI.Nodes;

namespace LEDControllerWPF
{
    class CSGOHandling
    {
        private GameStateListener _gsl;
        public CSGOHandling()
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

            Console.WriteLine(gs.Player.Weapons.ActiveWeapon.AmmoClip);
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
