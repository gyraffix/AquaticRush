using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    class WaitForSeconds
    {
        float finalTime;

        public WaitForSeconds(float Seconds)
        {
            finalTime = Time.time + Seconds * 1000;
        }

        public bool IsDone()
        {
            return Time.time >= finalTime;
        }
    }
}
