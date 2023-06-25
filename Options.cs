using System.Text.Json;

namespace USBKey
{
    internal static class Options
    {
        public static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static OptionsData _optionsData = new();
        public static string DataFileName
        {
            get => _optionsData.DataFileName;
        }
        public static Keys Keys
        {
            get => _optionsData.Keys;
        }
        public static void Load()
        {
            
        DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            var optionsfile = dir.EnumerateFiles("options.json", SearchOption.AllDirectories).FirstOrDefault();
            if (optionsfile != null)
            {
                try
                {
                    using FileStream optionsStream = File.OpenRead(optionsfile.FullName);
                    var loadedDptions = JsonSerializer.Deserialize<OptionsData>(optionsStream, jsonOptions);
                    if (loadedDptions is not null)
                    {
                        _optionsData = loadedDptions;
                        Logger.Log(LogType.Info, "Nastavení úspěšně načteno");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }


}
