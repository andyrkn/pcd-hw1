using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Presentation
{
    public sealed class TransferSizes
    {
        static TransferSizes()
        {
            Sizes = new Dictionary<string, int>
            {
                {"10", 100 },
                {"11", 250 },
                {"12", 500 },
                {"13", 750 },
                {"14", 1000 }
            };
        }

        private static Dictionary<string, int> Sizes { get; }

        public static int GetTransferSize(string command) =>
            Sizes.ContainsKey(command) ? Sizes[command] : 100;
    }
}
