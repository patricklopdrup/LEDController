using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGSI;
using CSGSI.Nodes;

namespace LEDControllerWPF
{
    class CsgoGameState
    {

        public void GameStates(GameState gs)
        {
            BombPlant(gs);

            int flash = FlashBang(gs);
            if (flash > 0)
            {
                Console.WriteLine($"Flash: {flash}");
            }

            Console.WriteLine($"Health: {gs.Player.State.Health}");
            Console.WriteLine($"Money: {gs.Player.State.Money}");
            Console.WriteLine($"Ammo: {gs.Player.Weapons.ActiveWeapon.AmmoClip}");
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
