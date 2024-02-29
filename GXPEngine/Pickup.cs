using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Pickup : Sprite
    {

        public int type;
        private float speed = 0.5f;
        private Player player;
        private MyGame game;
        bool used;

        public Pickup(string filename, int startX, Player player, MyGame game, int type) : base(filename)
        {
            x = startX;
            y = -80;

            this.player = player;
            this.game = game;
            this.type = type;
            
            scale = 0.5f;
        }

        public void Update()
        {
            y += speed * Time.deltaTime / 5;
        }

        public void OnCollision(GameObject other)
        {
            if (other.GetType().Equals(typeof(PlayerHitbox)) && !used)
            {
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
                    player.shootSpeed = 30;
                    yield return new WaitForSeconds(10);
                    player.shootSpeed = 50;
                    break; 
                case 1:
                    player.lives++;
                    break;
                case 2:
                    game.multiplier = 5;
                    game.multiplierPU = true;
                    yield return new WaitForSeconds(10);
                    game.multiplierPU = false;
                    break;
            }
            
        }

    }
}
