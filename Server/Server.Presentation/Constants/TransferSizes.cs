
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Presentation.Constants
{
    public sealed class TransferSizes
    {
        static TransferSizes()
        {
            Sizes = new Dictionary<string, int>
            {
                {"0", 100 },
                {"1", 250 },
                {"2", 500 },
                {"3", 750 },
                {"4", 1000 }
            };
        }

        private static Dictionary<string, int> Sizes { get; }

        public static int GetTransferSize(string command) =>
            Sizes.ContainsKey(command) ? Sizes[command] : 100;
    }
}
