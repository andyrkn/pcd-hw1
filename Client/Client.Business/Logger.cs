using System;
using System.IO;

namespace Client.Business
{
    public sealed class Logger : IDisposable
    {
        private string FileName { get; set; }
        private StreamWriter streamWriter;
        private StreamWriter streamWriterErrors;

        public void Connected(int transferSize, int chunkSize)
        { 
            Console.WriteLine("Connected");
            FileName = $"{DateTime.Now.ToString("hh-mm-ss")}_{transferSize}_{chunkSize}";
            streamWriter = new StreamWriter(FileName + ".txt", true);
            streamWriterErrors = new StreamWriter(FileName + "_errors.txt", true);
        }
        
        public void LogSpeed(int data, long ms)
        {
            var speedLog = $"{Math.Round((double)data / 1048576 * 10, 3, MidpointRounding.ToPositiveInfinity)}";

            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"{speedLog} MB/s         ");
            streamWriter.WriteLine($"{data} {speedLog} {ms}");
        }
       

        public void LogPercentage(int percent, int transferSize)
        {
            var log = $"{percent * 100 / transferSize} %";
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(log);
        }

        public void LogError(int i, int j, int chunkSize)
        {
            streamWriterErrors.WriteLine($"{i} {j} {chunkSize}");
            streamWriterErrors.Flush();
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"Package Error: {i} {j}");
        }

        public void Dispose()
        {
            streamWriter.Flush();
            streamWriter.Dispose();
            streamWriterErrors.Flush();
            streamWriterErrors.Dispose();
        }
    }
}
