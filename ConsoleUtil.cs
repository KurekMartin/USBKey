using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBKey
{
    internal static class ConsoleUtil
    {
        public static void ClearCurrentLine()
        {
            var currentLinePos = Console.CursorTop;
            Console.SetCursorPosition(0, currentLinePos);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLinePos);
        }

        public static void MoveCursorToStart()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
