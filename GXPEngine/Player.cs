using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {

       private string filename;
       private int cols;
       private int rows;
       
       private bool addCollider;
        private float playerSpeed;
        private float posY = 500;

        private bool moving;

        private List<Bullet> bullets = new List<Bullet>();


       public Player(string filename, int cols, int rows, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            this.filename = filename;
            this.cols = cols;
            this.rows = rows;
            

            this.addCollider = addCollider;
            SetXY(game.width/2 - 25, posY);
        }

        public void Update()
        {
            Move();
            Shoot();
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }
        }
        
        void Move()
        {
            moving = false;

            if (Input.GetKey(Key.LEFT))
            {
                Walk(-1);
            }
            if (Input.GetKey(Key.RIGHT)) 
            {  
                Walk(1); 
            }
            if (moving)
            {
                Translate(playerSpeed, 0);
                if (playerSpeed < 0 &&  x < 0) 
                {
                    x = 0;
                }
                else if (playerSpeed > 0 && x > 735)
                {
                    x = 735;
                }

            }

        }

        private void Walk(int Direction)
        {
            if (Direction < 0)
            {

                playerSpeed = -1;
                moving = true;
                //dust.Mirror(true, false);
                //dust.x = 55;
                Mirror(true, false);
            }
            else
            {
                playerSpeed = 1;
                moving = true;
                //dust.Mirror(false, false);
                //dust.x = 10;
                Mirror(false, false);
            }
        }

        void Shoot()
        {
            if (Input.GetKeyDown(Key.SPACE))
            {
                Bullet newBullet = new Bullet("circle.png", x + width/2s, y - 25);
                game.AddChild(newBullet);
                bullets.Add(newBullet);
            }
        }
    }
}
