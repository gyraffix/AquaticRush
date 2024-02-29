using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class PlayerHitbox : Sprite
    {
        Player player;
        MyGame game;

        public PlayerHitbox(String filename, Player player, MyGame game, bool addCollider = true) : base(filename, addCollider)
        {
            alpha = 0;
            x = -55; y = -180;
            SetScaleXY(2, 4.7f);
            this.player = player;
            this.game = game;
        }


        void OnCollision(GameObject other)
        {
            if (other.GetType().Equals(typeof(Enemy)) && other.noCol == false && !player.jumping && !player.hit && player.lives > 0)
            {
                player.hit = true;
                game.multiplier = 0;
                game.changeScore(-50);
                other.flagged = true;
                player.lives -= 1;
                player.playerHit.Play();
                if (player.lives != 0)
                    LateAddChild(new Coroutine(player.hitFeedback()));
                else player.SetCycle(24, 14, 18);
            }

            if (other.GetType().Equals(typeof(Wave)) && player.jumping == false && player.lives != 0)
            {
                player.Jump();
            }
        }
    }
}
