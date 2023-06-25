namespace USBKey
{
    enum LogType
    {
        Info, Warning, Error
    }
    internal static class Logger
    {        
        public static void Log(LogType type, string message)
        {
            Console.WriteLine($"[{type.ToString().ToUpper()}] {message}");
        }
    }
}
