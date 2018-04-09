using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPCMath;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Vector x = new Vector(5, Math.PI);
            Vector y = Vector.Random(5, -100.0, 1000.0);
            //Vector z;
            //z = x + y;

            //Cnsl.Show("x", x);
            Cnsl.Show("y", y);
            //Cnsl.Show("z", z);

            Cnsl.ReadLine();
        }
    }
}
