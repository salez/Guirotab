using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 5.499;
            var result = a.ToString("#.##");
            
            Console.Write(result + " " + a);
        }
    }
}
