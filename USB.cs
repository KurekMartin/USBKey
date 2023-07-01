using Newtonsoft.Json;

namespace USBKey
{
    internal static class USB
    {
        public static Data? GetDataFromDrive(string path)
        {
            var dataFilePath = FindDataFile(path);
            if (!string.IsNullOrEmpty(dataFilePath))
            {
                var data = ReadData(dataFilePath);
                return data;
            }
            else
            {
                Logger.Log(LogType.Error, Settings.Messages.KeyNotFound);
            }
            return null;
        }

        private static string FindDataFile(string root)
        {
            DirectoryInfo dir = new(root);
            var file = dir.EnumerateFiles($"{Settings.DataFileName}.json", SearchOption.AllDirectories).FirstOrDefault();
            if (file != null)
            {
                return file.FullName;
            }
            return string.Empty;
        }
        private static Data? ReadData(string path)
        {
            Data? data = null;
            try
            {
                var dataFile = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<Data>(dataFile, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                });
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Error, string.Format(Settings.Messages.DataReadError, path));
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
