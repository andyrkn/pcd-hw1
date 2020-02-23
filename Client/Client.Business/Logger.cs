using System;
using System.ComponentModel;

namespace Client.Business
{
    public sealed class Logger
    {
        public static void Connected() => Console.WriteLine("Connected");
        
        public static void LogSpeed(int data)
        {
            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"{(double) data /  1048576 * 10} MB/s");
        }

        public static void LogPercentage(int percent, int transferSize)
        {
            Console.SetCursorPosition(0, 2);
            Console.WriteLine($"{percent * 100 / transferSize} %");
        }
    }
}
