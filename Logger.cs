namespace USBKey
{
    enum LogType
    {
        Info, Warning, Error, Debug
    }
    internal static class Logger
    {
        public static void Log(LogType type, string message)
        {
            var color = Console.ForegroundColor;
            switch (type)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.Write($"[{type.ToString().ToUpper()}] ");
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }
    }
}
