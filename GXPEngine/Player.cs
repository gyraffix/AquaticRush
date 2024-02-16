using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections;
using System.ComponentModel;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {

       private List<int> toDestroy = new List<int>();
        public bool start;



        private float playerSpeed;
        public float lives = 3;
        public float scaleOG;
        private float rotateSpeed;
        private bool rotating;
        private bool moving;
        private bool jumping;
        private bool hit;
        
        private bool canJump = true;
        private bool coolDown;
        private bool canShoot = true;

        private EasyDraw playerUI;
        private bool addUI;

        private MyGame game1;
        private List<Bullet> bullets = new List<Bullet>();


        public Player(string filename, int cols, int rows, MyGame game, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            scaleOG = scale;
            game1 = game;

            SetXY(game.width/2 , game.height - 100);
            SetOrigin(width / 2, height / 2);
            SetScaleXY(0.06f, 0.06f);
            playerUI = new EasyDraw(game.width, game.height, false);
        }
        
        public void Update()
        {


            if (start)
            {
                
                Move();
                Shoot();
                foreach (Bullet bullet in bullets)
                {
                    bullet.Update();
                    if (bullet.y < 0 || bullet.flagged)
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
            if(Input.GetKeyDown(Key.W) && !jumping)
            {
                //Jump();
            }

            if (moving)
            {
                Translate(playerSpeed, 0);
                if (playerSpeed < 0 && x < 0)
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
                if (rotation < 0)
                {
                    Turn(2);

                }
                else
                {
                    Turn(-2);

                }

            }
            if (rotating)
            {
                Turn(rotateSpeed);

                if (rotation < -45)
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

                playerSpeed = -1.5f;
                moving = true;
                Mirror(true, false);
            }
            else
            {
                playerSpeed = 1.5f;
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
                if (!addUI) parent.AddChild(playerUI);
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
            
            for (int i = 0; i < 50; i++)
            {
                
                yield return new WaitForSeconds(0.01f);

                playerUI.Fill(255, 0, 0);
                playerUI.Rect(game.width/2, game.height - 40, i*2, 10);
            }
            playerUI.ClearTransparent();
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

        IEnumerator hitFeedback()
        {
            hit = true;
            for (int i = 0; i < 3; i++)
            {
                alpha = 0;
                yield return new WaitForSeconds(0.3f);
                alpha = 1;
                yield return new WaitForSeconds(0.3f);
            }
            hit = false;
        }

        void OnCollision(GameObject other)
        {
            if (other.GetType().Equals(typeof(Enemy)) && !jumping && !hit)
            {
                other.LateRemove();
                lives -= 1;
                LateAddChild(new Coroutine(hitFeedback()));
            }
        }
    }
}
