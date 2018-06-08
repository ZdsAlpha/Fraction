using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zds;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Fraction f1 = "0.5";            //1/2
            Fraction f2 = "1.2/0.6";        //2/1
            Fraction f3 = f1 + f2;          //5/2
            Fraction f4 = f1 * f2;          //1/1
            Fraction f5 = f1.Square;        //1/4
            Fraction pi1 = "3.14";          //157/50
            Fraction pi2 = "22/7";          //22/7
            bool b = pi1.Approches(pi2);    //true
            Console.ReadKey();
        }
    }
}
