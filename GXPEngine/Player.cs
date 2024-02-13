using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Media;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {

       private string filename;
       private int cols;
       private int rows;
       private int frames;
       private bool addCollider;

       public Player(string filename, int cols, int rows, int frames = -1, bool addCollider = true) : base(filename, cols, rows, frames, addCollider)
        {
            this.filename = filename;
            this.cols = cols;
            this.rows = rows;
            this.frames = frames;

            this.addCollider = addCollider;
        }
    }
}
