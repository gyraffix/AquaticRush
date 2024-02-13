using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Coroutine : GameObject
    {
        public bool Done { get; private set; } = false;

        IEnumerator enumerator;
        object item = null;

        public Coroutine(IEnumerator pEnumerator)
        {
            Console.WriteLine("Creating coroutine");
            enumerator = pEnumerator;
        }

        void Step()
        {
            if (!enumerator.MoveNext())
            {
                Destroy();
                Console.WriteLine("Coroutine done: destroying Coroutine game object");
                Done = true;
            }
            else
            {
                item = enumerator.Current;
            }
        }

        public void Update()
        {
            if (parent == null) 
                return;

            if (item == null)
            { 
                Step();
            }
            else if (item is WaitForSeconds)
            {
                if (((WaitForSeconds)item).IsDone())
                { 
                    Step();
                }
                
            }
            else
            {
                Console.WriteLine("Coroutine error: return type not supported: " + item.GetType());
                Step();
            }
        }
    }
}
