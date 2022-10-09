using System;
using System.Linq;

namespace LeetCodeCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var test = new LeetCode_46();
            var ans = test.Permute(new[] {1, 2, 3, 4});

            Console.WriteLine();
        }
    }
}