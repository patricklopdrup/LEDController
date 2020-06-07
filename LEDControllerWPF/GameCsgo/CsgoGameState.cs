using System;
using CSGSI;
using CSGSI.Nodes;

namespace LEDControllerWPF.GameCsgo
{
    class CsgoGameState
    {
        private DataPort _port = new DataPort();

        private enum CsgoStates
        {
            Bombplant,
            Bombexplosion,
            Flash,
            Defused,

        }

        public void GameStates(GameState gs)
        {
            BombPlant(gs);
            BombExplosion(gs);
            Defused(gs);

            int flash = FlashBang(gs);
            if (flash > 0)
            {
                Console.WriteLine($"Flash: {flash}");
                // send "flash120" if player is flashed for 120
                _port.SendData(CsgoStates.Flash.ToString() + flash);
            }

            Console.WriteLine($"Health: {gs.Player.State.Health}");
            Console.WriteLine($"Money: {gs.Player.State.Money}");
            Console.WriteLine($"Ammo: {gs.Player.Weapons.ActiveWeapon.AmmoClip}");
        }

        public void Startup()
        {
            // run startup animation
        }

        public void ShutDown()
        {
            // run shutdown animation
        }

        private void BombPlant(GameState gs)
        {
            if (gs.Round.Phase == RoundPhase.Live &&
                gs.Round.Bomb == BombState.Planted)
            {
                Console.WriteLine("Bomb has been planted.");
                _port.SendData(CsgoStates.Bombplant.ToString());
            }
        }

        private void BombExplosion(GameState gs)
        {
            if (gs.Round.Phase == RoundPhase.Live &&
                gs.Previously.Round.Bomb == BombState.Planted &&
                gs.Round.Bomb == BombState.Exploded)
            {
                Console.WriteLine("Bomb exploded");
                _port.SendData(CsgoStates.Bombexplosion.ToString());
            }
        }

        private void Defused(GameState gs)
        {
            if (gs.Round.Phase == RoundPhase.Live &&
                gs.Round.Bomb == BombState.Defused &&
                gs.Previously.Round.Bomb == BombState.Planted)
            {
                Console.WriteLine("Bomb has been defused");
                _port.SendData(CsgoStates.Defused.ToString());
            }
        }

        private int FlashBang(GameState gs)
        {
            return gs.Player.State.Flashed;
        }
    }
}
