using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Enemy : AnimationSprite
    {
{
        private string filename;
        private int cols;
        private int rows;
        private float startY;

        private bool addCollider;

        public Enemy(String filename, int cols, int rows, float startY, int frames = -1, bool addCollider = true, ) : base(filename, cols, rows, frames, addCollider)
        {
            this.filename = filename;
            this.cols = cols;
            this.rows = rows;
            this.startY = startY;

            this.addCollider = addCollider;
        }
    }
}
}
