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
        public static Messages Messages { get => _optionsData.Messages; }
        public static bool Maximize { get => _optionsData.Maximize; }
        public static bool ShowTrollVideo { get => _optionsData.ShowTrollVideo; }
        public static string TrollVideoFileName { get => _optionsData.TrollVideoFileName; }
        public static bool Loaded { get; private set; } = false;
        public static void Load()
        {

            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            var settingsfile = dir.EnumerateFiles(_fileName, SearchOption.AllDirectories).FirstOrDefault();
            if (settingsfile != null)
            {
                try
                {
                    var settings = File.ReadAllText(settingsfile.FullName);
                    var loadedDptions = JsonConvert.DeserializeObject<SettingsData>(settings, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error,
                    });
                    if (loadedDptions is not null)
                    {
                        _optionsData = loadedDptions;
                        Loaded = true;
                    }
                }
                catch (Exception ex)
                {
                    Loaded = false;
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Logger.Log(LogType.Error, string.Format(Messages.FileNotFound, _fileName));
                Environment.Exit(0);
            }
        }
    }


}
