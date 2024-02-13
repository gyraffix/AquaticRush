using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Bullet : Sprite
    {

        private string filename;
        private bool addCollider;
        private float bulletSpeed = 2;
        
        
        

        public Bullet(string filename,float startX, float startY, bool keepInCache = false, bool addCollider = true) : base(filename, keepInCache, addCollider)
        {
            this.filename = filename;
            this.addCollider = addCollider;
            x = startX - (width* 0.2f)/2;
            y = startY;
            Console.WriteLine("bullet created");
            scale = 0.2f;
        }

        public void Update()
        {
            y -= bulletSpeed;

            
        }

        void OnCollision(GameObject other)
        {
            if (other != FindObjectOfType<Player>())
            {
                flagged = true;
                other.flagged = true;
            }
        }
    }
}
