using System.IO;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Usb.Events;

namespace USBKey
{
    class Program
    {
        public static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public static Options options = new();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            LoadOptions();

            using IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();

            usbEventWatcher.UsbDriveMounted += (_, path) =>
            {
                ProcessDrive(path);
            };
            Console.WriteLine("Insert USB");

            while (true) { };
        }


        static void ProcessDrive(string path)
        {
            var dataFilePath = FindDataFile(path);
            if (!string.IsNullOrEmpty(dataFilePath))
            {
                var data = ReadData(dataFilePath);
                ProcessData(data);
            }
            else
            {
                Log(LogType.Error, "Klíč nenalezen");
            }
        }

        static string FindDataFile(string root)
        {
            DirectoryInfo dir = new(root);
            var file = dir.EnumerateFiles($"{options.DataFileName}.json", SearchOption.AllDirectories).FirstOrDefault();
            if (file != null)
            {
                return file.FullName;
            }
            return string.Empty;
        }

        static Data? ReadData(string path)
        {
            Data? data = null;
            try
            {
                using FileStream dataFile = File.OpenRead(path);
                data = JsonSerializer.Deserialize<Data>(dataFile, jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        static void ProcessData(Data? data)
        {
            if (data is not null)
            {
                if(data.Key == options.Keys.Correct)
                {
                    Log(LogType.Info, "Správný klíč");
                }
                else if (data.Key == options.Keys.Troll)
                {
                    Log(LogType.Info, "Trololololol");
                }
                else
                {
                    Log(LogType.Info, "Špatně");
                }
            }
            else
            {
                Log(LogType.Error, "Chyba čtení");
            }
        }

        static void LoadOptions()
        {
            DirectoryInfo dir = new(Directory.GetCurrentDirectory());
            var optionsfile = dir.EnumerateFiles("options.json", SearchOption.AllDirectories).FirstOrDefault();
            if (optionsfile != null)
            {
                try
                {
                    using FileStream optionsStream = File.OpenRead(optionsfile.FullName);
                    var loadedDptions = JsonSerializer.Deserialize<Options>(optionsStream, jsonOptions);
                    if (loadedDptions is not null)
                    {
                        options = loadedDptions;
                        Log(LogType.Info, "Nastavení úspěšně načteno");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        enum LogType
        {
            Info, Warning, Error
        }
        static void Log(LogType type, string message)
        {
            Console.WriteLine($"[{type.ToString().ToUpper()}] {message}");
        }
    }
}