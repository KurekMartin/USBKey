using System.Diagnostics;
using System.Text;
using Usb.Events;

namespace USBKey
{
    class Program
    {
        private static readonly WaitingElement _waitingElement = new();
        private static Random _random = new();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Settings.Load();

            Console.Write("Vložte USB klíč ");
            _waitingElement.Show();

            using IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();
            Data? data = null;
            usbEventWatcher.UsbDriveMounted += (_, path) =>
            {
                data = USB.GetDataFromDrive(path);
                _waitingElement.Stop();
                MainLoop(data);
            };

            while (true) { };
        }

        static void MainLoop(Data? data)
        {

            foreach (var stage in Settings.Stages)
            {
                switch (stage.Type)
                {
                    case StageType.Message:
                        ProcessMessage(stage);
                        break;
                    case StageType.Progress:
                        ProcessProgress(stage);
                        break;
                }
            }


            ProgressBar progressBar = new(30, "SampleTask");
            do
            {
                progressBar.Value += _random.Next(10);
                Thread.Sleep(_random.Next(1000));
            } while (progressBar.Value != 100);
            ProcessData(data);
        }

        static void ProcessMessage(Stage stage)
        {
            Console.WriteLine(stage.Text);
            Thread.Sleep(stage.Duration);
        }

        static void ProcessProgress(Stage stage)
        {
            ProgressBar progressBar = new(stage.ProgressBarLength, stage.Text);
            int totalDuration = 0;
            do
            {
                if (totalDuration < stage.Duration)
                {
                    var durationVariance = _random.Next(-stage.StepDurationVariance, stage.StepDurationVariance);
                    var duration = Math.Max(stage.StepDuration + durationVariance, 0);
                    var step = _random.Next(1, stage.MaxStep);

                    progressBar.Value += step;
                    Thread.Sleep(duration);

                    totalDuration += duration;
                }
                else
                {
                    progressBar.Value = 100;
                }
            } while (progressBar.Value != 100);
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