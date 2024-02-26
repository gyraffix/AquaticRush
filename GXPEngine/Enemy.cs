using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Enemy : AnimationSprite
    {
        private Random rnd = new Random();
        public int enemyType;
        private float enemySpeed = 0.5f;
        private float sideSpeed= 0.5f;
        public bool breakable = true;
        private bool left;
        private float startX;
        public bool dead;
        public Enemy(String filename, int cols, int rows, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            
            if (filename.Equals("shark.png")) 
            { 
                enemyType = 0;
                scale = 0.75f;
                SetCycle(0, 17, 18);
                enemySpeed = 0.75f;
            }
            if (filename.Equals("wood.png") || filename.Equals("rock.png") || filename.Equals("rock1.png")) 
            { 
                if (filename.Equals("wood.png"))
                {
                    enemySpeed = 0.55f;
                }
                enemyType = 1;
                SetCycle(0, 4, 18);
                breakable = false;
            }
            if (filename.Equals("tentacle.png")) 
            { 
                enemyType = 2;
                SetCycle(0, 8, 18);
            }
            x = rnd.Next(game.width);
            startX = x;
            y = -200;

            
        }

        public void Update()
        {
            Animate();
                y += enemySpeed;
                if (enemyType == 2)
                {
                    if (x > startX + width)
                    {
                        left = true;
                        x = startX + width;
                    }
                    else if (x < startX - width)
                    {
                        left = false;
                        x = startX - width;
                    }
                    if (left) { x -= sideSpeed; }
                    else { x += sideSpeed; }
                }
            
            if(dead)
            {

                switch (enemyType)
                {
                    case 0:
                        if (currentFrame == 16)
                        {
                            enemySpeed = 0.60f;
                            SetCycle(18, 6, 18);
                        }
                        if (currentFrame == 23)
                        {
                            LateRemove();
                        }
                        break;
                    case 2:
                        if (currentFrame == 7)
                        {
                            sideSpeed = 0.25f;
                            SetCycle(8, 7, 18);
                        }
                        if (currentFrame == 14)
                        {
                            LateRemove();
                        }
                        break;

                }
                    

                
            }
        }

        void OnCollision(GameObject other)
        {
            if (enemyType == 1 && other.GetType().Equals(typeof(Enemy)))
            {
                other.LateRemove();
            }
        }

        public void Death()
        {
            enemySpeed = 0.5f;
            noCol = true;
            dead = true;
        }
    }
}

