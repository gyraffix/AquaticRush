using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Wave : AnimationSprite
    {
        private float moveSpeed = 0.5f;

        public Wave(string filename, int cols, int rows, int posX) : base(filename, cols, rows)
        {
            scaleX = 1.5f;
            x = posX;
            y = -80;
        }


        public void Update()
        {
            y += moveSpeed * Time.deltaTime / 5;
        }

        public void OnCollision()
        {
            LateRemove();
        }
    }
}
