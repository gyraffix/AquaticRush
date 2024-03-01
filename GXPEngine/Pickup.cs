using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GXPEngine
{
    internal class Pickup : AnimationSprite
    {

        public int type;
        private float speed = 0.5f;
        private Player player;
        private MyGame game;
        bool used;

        public Pickup(string filename, int startX, Player player, MyGame game, int type, int cols, int rows, int frames = -1) : base(filename, cols, rows, frames)
        {
            x = startX;
            y = -80;

            this.player = player;
            this.game = game;
            this.type = type;
            
            scale = 0.35f;
        }

        public void Update()
        {
            Animate();
            y += speed * Time.deltaTime / 5;
        }

        public void OnCollision(GameObject other)
        {
            if (other.GetType().Equals(typeof(PlayerHitbox)) && !used)
            {
                game.multUp.Play();
                this.alpha = 0;
                used = true;
                LateAddChild(new Coroutine(powerUp()));
            }
        }

        IEnumerator powerUp() 
        {
            switch(type)
            {
                case 0:
                    game.text("Shot Speed UP!!!", x + width/2, y + 50, Color.LightBlue, 1, 30);
                    player.shootSpeed = 10;
                    yield return new WaitForSeconds(10);
                    player.shootSpeed = 50;
                    break; 
                case 1:
                    game.text("1UP!!!", x + width / 2, y + 50, Color.Red, 1, 30);
                    player.lives++;
                    break;
                case 2:
                    game.text("MAX MULTIPLIER!!!", x + width / 2, y + 50, Color.Yellow, 1, 24);
                    game.multiplier = 5;
                    game.multiplierPU = true;
                    yield return new WaitForSeconds(10);
                    game.multiplierPU = false;
                    break;
            }
            
        }

    }
}
