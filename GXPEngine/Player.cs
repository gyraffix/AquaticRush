using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Media;
using System.Runtime.CompilerServices;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {

       private string filename;
       private int cols;
       private int rows;
       private int frames;
       private bool addCollider;
        private float playerSpeed;
        private float posY = 500;

        private bool moving;

       public Player(string filename, int cols, int rows, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            this.filename = filename;
            this.cols = cols;
            this.rows = rows;
            this.frames = frames;

            this.addCollider = addCollider;
            SetXY(game.width/2 - 25, posY);
        }

        public void Update()
        {
            Move();
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
    }
}
