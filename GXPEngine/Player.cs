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
        public float lives = 3;
        public float scaleOG;
        private float rotateSpeed;
        private bool rotating;
        private bool moving;
        private bool jumping;
        private bool canJump = true;
        private bool coolDown;
        private bool canShoot = true;

        private List<Bullet> bullets = new List<Bullet>();


       public Player(string filename, int cols, int rows, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            this.filename = filename;
            this.cols = cols;
            this.rows = rows;

            scaleOG = scale;
            this.addCollider = addCollider;
            SetXY(game.width/2 - 25, game.height - 100);
            SetOrigin(width / 2, height / 2);
        }

        public void Update()
        {
            Move();
            Shoot();
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
                if ( bullet.y < 0 || bullet.flagged)
                { 
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
                Rotate(-1);
            }
            if (Input.GetKey(Key.D)) 
            {  
                Walk(1); 
                Rotate(1);
            }
            if(Input.GetKeyDown(Key.LEFT_SHIFT) && !jumping)
            {
                Jump();
            }

            if (moving)
            {
                Translate(playerSpeed, 0);
                if (playerSpeed < 0 &&  x < 0) 
                {
                    x = 0;
                }
                else if (playerSpeed > 0 && x > game.width)
                {
                    x = game.width;
                }

            }
            else if (rotation != 0)
            {
                if (rotation < 0) Turn(1);
                else Turn(-1);
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
                Mirror(true, false);
            }
            else
            {
                playerSpeed = 1;
                moving = true;
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

        void Jump()
        {
            if (canJump)
            {
                jumping = true;
                AddChild(new Coroutine(jumpCooldown()));
                AddChild(new Coroutine(jumpTimer()));
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

        IEnumerator jumpTimer()
        {

            SetScaleXY(scaleX +0.01f, scaleY+0.01f);
            bool down = false;
            while (scaleX != scaleOG)
            {
                yield return new WaitForSeconds(0.01f);
                if (scaleX < 1.6f && !down) SetScaleXY(scaleX + 0.01f, scaleY + 0.01f);

                else
                {
                    down = true;
                    SetScaleXY(scaleX - 0.01f, scaleY - 0.01f);
                }
            }
            jumping = false;
        }

        IEnumerator jumpCooldown()
        {
            canJump = false;
            yield return new WaitForSeconds(3);
            canJump = true;
        }

        void OnCollision(GameObject other)
        {
            if (other.GetType().Equals(typeof(Enemy)) && !jumping)
            {
                other.flagged = true;
                lives -= 0.5f;
            }
        }
    }
}
