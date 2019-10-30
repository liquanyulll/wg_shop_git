using System;

namespace UnitTest
{
    class Program
    {

        static void Main(string[] args)
        {
            var sup = new MoneyKeySupport();
            var cdf = sup.AddMoneyKey("0faka", 5, 50);

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
