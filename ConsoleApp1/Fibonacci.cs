using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Fibonacci
    {
        public int FibonacciSeries(int n)
        {
            if (n == 0) return 0;

            int x = 0;
            int y = 1;
            for(int i = 0; i < n; i++)
            {
                int z = x + y;
                x = y;
                y = z;
            }

            return y;
        }

        public int NextFibonacci(int n)
        {

            int i = 0;

            while(n > FibonacciSeries(i))
            {
                i++;
            }

            return FibonacciSeries(++i);
        }
    }
}
