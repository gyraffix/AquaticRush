using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections;
using System.ComponentModel;
using TiledMapParser;

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
        public Sound playerHit = new Sound("Hit.wav");

        private float playerSpeed;
        public float lives = 3;
        public float scaleOG;
        private float rotateSpeed;
        private bool rotating;
        private bool moving;
        public bool jumping;
        public bool hit;
        
        private bool canJump = true;
        private bool coolDown;
        public bool canShoot = true;
        public int shootSpeed = 50;

        private EasyDraw playerUI;
        private bool addUI;

        private MyGame game1;
        private List<Bullet> bullets = new List<Bullet>();

        private PlayerHitbox hitBox;


        public Player(string filename, int cols, int rows, MyGame game, int frames = -1, bool addCollider = false) : base(filename, cols, rows, frames, addCollider)
        {
            
            game1 = game;

            SetXY(game.width/2 , game.height - 250);
            SetOrigin(width / 2, height / 2);
            SetScaleXY(0.5f, 0.5f);
            scaleOG = scale;
            playerUI = new EasyDraw(game.width, game.height, false);
            hitBox = new PlayerHitbox("colors.png", this, game1);
            AddChild(hitBox);
        }
        
        public void Update()
        {
            

            if (start)
            {
                if (currentFrame != 37 && currentFrame != 43)
                Animate();
                Move();
                Shoot();
                
                if (currentFrame == 23 || currentFrame == 47)
                {
                    SetCycle(0, 12, 24);
                }

                if (y < game.height -100)
                {
                    y += 0.5f * Time.deltaTime / 5;
                }

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
                
            }
            else
            {
                playerSpeed = 1.5f;
                moving = true;
                
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

        public void Jump()
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
            
            for (int i = 0; i < shootSpeed; i++)
            {
                
                yield return new WaitForSeconds(0.01f);

                playerUI.Fill(255, 0, 0);
                playerUI.Rect(game.width/2, game.height - 40, (i*100)/shootSpeed, 10);
            }
            gunLoaded.Play();
            playerUI.ClearTransparent();
            canShoot = true;
        }
        
        IEnumerator jumpTimer()
        {
            SetCycle(38,6,24);
            SetScaleXY(scaleX +0.01f * scaleOG, scaleY+0.01f * scaleOG);
            bool down = false;
            while (scaleX > scaleOG)
            {
                yield return new WaitForSeconds(0.015f);
                if (scaleX < 1.6f * scaleOG && !down) SetScaleXY(scaleX + 0.01f * scaleOG * Time.deltaTime / 5, scaleY + 0.01f * scaleOG * Time.deltaTime / 5);

                else
                {
                    down = true;
                    SetScaleXY(scaleX - 0.01f * scaleOG * Time.deltaTime / 5, scaleY - 0.01f * scaleOG * Time.deltaTime / 5);
                    
                }
            }
            SetCycle(44, 4, 32);
            jumpStop.Play();
            jumping = false;
        }


        public IEnumerator hitFeedback()
        {
            hit = true;
            uint colorOG = color;
            SetCycle(12, 12, 30);
            for (int i = 0; i < 4; i++)
            {
                color = 0x555555;
                yield return new WaitForSeconds(0.3f);
                color = colorOG;
                yield return new WaitForSeconds(0.3f);
            }
            hit = false;
        }

        
    }
}
