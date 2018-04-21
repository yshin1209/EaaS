using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class QuadraticProgrammingInput
    {
        public double[,] Q { get; set; }
        public double [] d { get; set; }
        public double[,] A { get; set; }
        public double[] b { get; set; }
        public int m { get; set; }
    }

}
