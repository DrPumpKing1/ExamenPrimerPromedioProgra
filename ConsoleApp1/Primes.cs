using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Primes
    {
        public bool IsPrime(int n)
        {
            if(n <= 1) return false;

            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if(n % i == 0) return false;
            }

            return true;
        }

        public int Prime(int n)
        {
            if(n <= 0) return 0;

            int x = 2;

            for(int i = 0; i < n;  i++)
            {
                x++;
                while (!IsPrime(x))
                {
                    x++;
                }
            }

            return x;
        }
    }
}
