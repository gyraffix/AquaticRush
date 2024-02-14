using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {

       private string filename;
       private int cols;
       private int rows;
       private List<int> toDestroy = new List<int>();

       private bool addCollider;
        private float playerSpeed;
        
        

        private float rotateSpeed;
        private bool rotating;
        private bool moving;
        private bool coolDown;
        private bool canShoot = true;

        private List<Bullet> bullets = new List<Bullet>();


       public Player(string filename, int cols, int rows, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            this.filename = filename;
            this.cols = cols;
            this.rows = rows;
            

            this.addCollider = addCollider;
            SetXY(game.width/2 - 25, game.height - 100);
        }

        public void Update()
        {
            SetOrigin(width/2, height/2);
            Move();
            Shoot();
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
                if ( /*bullet.y < 0 ||*/ bullet.flagged)
                {
                    
                    
                    //Console.WriteLine("bullet destroyed");
                    
                    
                    toDestroy.Add(bullets.IndexOf(bullet));
                }
            }
            foreach (int index in toDestroy)
            {
                
                bullets[index].LateDestroy();
                bullets.RemoveAt(index);
                
                
            }
            toDestroy.Clear();

            if (coolDown)
            {

                parent.AddChild(new Coroutine(shootCoolDown()));
                coolDown = false;
            }
        }
        
        void Move()
        {
            moving = false;
            rotating = false;

            if (Input.GetKey(Key.A))
            {
                Walk(-1);
            }
            if (Input.GetKey(Key.D)) 
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
                else if (playerSpeed > 0 && x > game.width - 65)
                {
                    x = 735;
                }

            }

            if (Input.GetKey(Key.LEFT))
            {
                Rotate(-1);
            }
            if (Input.GetKey(Key.RIGHT))
            {
                Rotate(1);
            }

            if (rotating)
            {
                Turn(rotateSpeed);
                if ( rotation < -45)
                {
                    rotation = -45;
                }
                else if (rotation > 45)
                {
                    rotation = 45;
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

        private void Rotate(int Direction)
        {
            if (Direction < 0)
            {

                rotateSpeed = -1;
                rotating = true;
            }
            else
            {
                rotateSpeed = 1;
                rotating = true;
            }
        }

        void Shoot()
        {
            if (Input.GetKeyDown(Key.SPACE) && canShoot)
            {
                Bullet newBullet = new Bullet("circle.png", x, y, rotation/45);
                parent.AddChild(newBullet);
                bullets.Add(newBullet);
                Console.WriteLine("there are currently " + bullets.Count + " bullets");
                canShoot = false;
                coolDown = true;
            }
            
        }

        IEnumerator shootCoolDown()
        {
            yield return new WaitForSeconds(0.5f);
            canShoot = true;
        }
    }
}
