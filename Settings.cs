using Newtonsoft.Json;


namespace USBKey
{
    internal static class Settings
    {
        public static readonly System.Text.Json.JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private static readonly string _fileName = "settings.json";
        private static SettingsData _optionsData = new();
        public static string DataFileName { get => _optionsData.DataFileName; }
        public static Keys Keys { get => _optionsData.Keys; }
        public static int? Seed { get => _optionsData.Seed; }
        public static Stage[] Stages { get => _optionsData.Stages; }
        public static void Load()
        {

            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            var settingsfile = dir.EnumerateFiles(_fileName, SearchOption.AllDirectories).FirstOrDefault();
            if (settingsfile != null)
            {
                try
                {
                    using StreamReader optionsStream = File.OpenText(settingsfile.FullName);
                    JsonSerializer serializer = new();
                    var loadedDptions = (SettingsData?)serializer.Deserialize(optionsStream, typeof(SettingsData));
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
            else
            {
                Logger.Log(LogType.Error, $"File {_fileName} not found");
                Environment.Exit(0);
            }
        }
    }


}
