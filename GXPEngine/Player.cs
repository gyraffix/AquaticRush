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

        private Sound gunShoot = new Sound("Gun Shoot.wav");
        private Sound gunLoaded = new Sound("Gun Reload Ready.wav");
        private Sound jumpStart = new Sound("Jump Start.wav");
        private Sound jumpStop = new Sound("Splash Down.wav");
        private Sound playerHit = new Sound("Hit.wav");

        private float playerSpeed;
        public float lives = 3;
        public float scaleOG;
        private float rotateSpeed;
        private bool rotating;
        private bool moving;
        private bool jumping;
        public bool hit;
        
        private bool canJump = true;
        private bool coolDown;
        public bool canShoot = true;

        private EasyDraw playerUI;
        private bool addUI;

        private MyGame game1;
        private List<Bullet> bullets = new List<Bullet>();


        public Player(string filename, int cols, int rows, MyGame game, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            
            game1 = game;

            SetXY(game.width/2 , game.height - 100);
            SetOrigin(width / 2, height / 2);
            SetScaleXY(0.06f, 0.06f);
            scaleOG = scale;
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

                    bullets[index].LateRemove();
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
            

            if (moving)
            {
                Translate(playerSpeed * Time.deltaTime/5, 0);
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
                game1.changeScore(100);
                jumping = true;
                jumpStart.Play();
                Console.WriteLine();
                LateAddChild(new Coroutine(jumpTimer()));
            }
        }

        void Shoot()
        {
            if (Input.GetKeyDown(Key.SPACE) && canShoot)
            {
                if (!addUI) parent.AddChild(playerUI);
                Bullet newBullet = new Bullet("circle.png", x, y, rotation/45);
                gunShoot.Play();

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
            gunLoaded.Play();
            playerUI.ClearTransparent();
            canShoot = true;
        }
        
        IEnumerator jumpTimer()
        {

            SetScaleXY(scaleX +0.01f * scaleOG, scaleY+0.01f * scaleOG);
            bool down = false;
            while (scaleX > scaleOG)
            {
                yield return new WaitForSeconds(0.01f);
                if (scaleX < 1.6f * scaleOG && !down) SetScaleXY(scaleX + 0.01f * scaleOG * Time.deltaTime / 5, scaleY + 0.01f * scaleOG * Time.deltaTime / 5);

                else
                {
                    down = true;
                    SetScaleXY(scaleX - 0.01f * scaleOG * Time.deltaTime / 5, scaleY - 0.01f * scaleOG * Time.deltaTime / 5);
                }
            }
            jumpStop.Play();
            jumping = false;
        }


        IEnumerator hitFeedback()
        {
            hit = true;
            for (int i = 0; i < 4; i++)
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
            if (other.GetType().Equals(typeof(Enemy)) && other.noCol == false && !jumping && !hit)
            {
                game1.multiplier = 0;
                game1.changeScore(-50);
                other.LateRemove();
                lives -= 1;
                playerHit.Play();
                LateAddChild(new Coroutine(hitFeedback()));
            }

            if (other.GetType().Equals(typeof(Wave)) && jumping == false)
            {
                Jump();
            }
        }
    }
}
