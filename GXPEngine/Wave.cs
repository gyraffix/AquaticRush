using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Wave : AnimationSprite
    {
        private float moveSpeed = 0.75f;

        public Wave(string filename, int cols, int rows, int posX, int frames) : base(filename, cols, rows, frames)
        {
            scaleX = 1.5f;
            scaleY = 0.8f;
            x = posX;
            y = -480;
        }


        public void Update()
        {
            Animate();
            y += moveSpeed * Time.deltaTime / 5;
        }

        public void OnCollision()
        {
            LateRemove();
        }
    }
}
