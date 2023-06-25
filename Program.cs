using System.Text;
using Usb.Events;

namespace USBKey
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Settings.Load();

            Console.Write("Vložte USB klíč ");
            WaitingElement waitingElement = new(150);

            using IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();
            Data? data = null;
            usbEventWatcher.UsbDriveMounted += (_, path) =>
            {
                data = USB.GetDataFromDrive(path);
                waitingElement.Stop();
                MainLoop(data);
            };

            while (true) { };
        }

        static void MainLoop(Data? data)
        {
            Random random = new();
            ProgressBar progressBar = new(30, "SampleTask");
            do
            {
                progressBar.Value += random.Next(10);
                Thread.Sleep(random.Next(1000));
            } while (progressBar.Value != 100);
            ProcessData(data);
        }

        static void ProcessData(Data? data)
        {
            if (data is not null)
            {
                if (data.Key == Settings.Keys.Correct)
                {
                    Logger.Log(LogType.Info, "Správný klíč");
                }
                else if (data.Key == Settings.Keys.Troll)
                {
                    Logger.Log(LogType.Info, "Trololololol");
                }
                else
                {
                    Logger.Log(LogType.Info, "Špatně");
                }
            }
            else
            {
                Logger.Log(LogType.Error, "Chyba čtení");
            }
        }
    }
}