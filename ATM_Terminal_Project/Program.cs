using System;

namespace ATMTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ATM Terminal ===");
            var atm = new ATM("codes.txt");
            atm.Run();
        }
    }
}