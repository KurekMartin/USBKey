using System.Diagnostics;
using System.Runtime.InteropServices;
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

            if (Settings.Loaded)
            {
                if (Settings.Maximize)
                {
                    Maximize();
                }
                Logger.Log(LogType.Info, Settings.Messages.SettingsLoaded);
            }
            else
            {
                Logger.Log(LogType.Error, Settings.Messages.SettingsLoadError);
                Environment.Exit(0);
            }


            if (Settings.Seed is not null)
            {
                _random = new Random((int)Settings.Seed);
            }

            Console.Write(Settings.Messages.InsertUSB);
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
#if DEBUG
                Logger.Log(LogType.Debug, $"Type: {stage.Type} | Duration: {stage.Duration}ms | ProgressBarLen: {stage.ProgressBarLength}");
                Stopwatch sw = Stopwatch.StartNew();
#endif
                switch (stage.Type)
                {
                    case StageType.Message:
                        ProcessMessage(stage);
                        break;
                    case StageType.Progress:
                        ProcessProgress(stage);
                        break;
                }
#if DEBUG
                sw.Stop();
                Logger.Log(LogType.Debug, $"Real duration: {sw.ElapsedMilliseconds}ms");
#endif
            }
            ProcessData(data);
        }

        static void ProcessMessage(Stage stage)
        {
            var showWaitAnim = stage.Duration > 2000;
            Console.Write(stage.Text + " ");

            if (showWaitAnim)
            {
                _waitingElement.Show();
            }
            Thread.Sleep(stage.Duration);
            _waitingElement.Stop();
            if (!showWaitAnim)
            {
                Console.WriteLine();
            }
        }

        static void ProcessProgress(Stage stage)
        {
            ProgressBar progressBar = new(stage.ProgressBarLength, stage.Text);
            stage.Duration -= progressBar.FinishWait;
            int stepDuration = stage.Duration / stage.MaxStepCount;
            int stepDurationVariance = (int)(stepDuration * stage.StepDurationVariance);
            int stepProgress = 100 / stage.MaxStepCount;
            int stepProgressVariance = (int)(stepProgress * stage.StepProgressVariance);
            int totalDuration = 0;
            do
            {
                var durationVariance = _random.Next(-stepDurationVariance, stepDurationVariance);
                var duration = Math.Max(stepDuration + durationVariance, 0);
                var stepVariance = _random.Next(-stepProgressVariance, stepProgressVariance);
                var step = Math.Max(stepProgress + stepVariance, 1);

                if (totalDuration + duration > stage.Duration) // limit max duration
                {
                    duration = stage.Duration - totalDuration;
                    step = 100 - progressBar.Value;
                }
                else if (progressBar.Value + step >= 100 && totalDuration + duration < stage.Duration) // done sooner than Duration
                {
                    duration = stage.Duration - totalDuration;
                }

                progressBar.Value += step;
                Thread.Sleep(duration);

                totalDuration += duration;
            } while (progressBar.Value != 100);
        }

        static void ProcessData(Data? data)
        {
            if (data is not null)
            {
                if (data.Key == Settings.Keys.Correct)
                {
                    Logger.Log(LogType.Info, Settings.Messages.CorrectKey, ConsoleColor.Green);
                }
                else if (data.Key == Settings.Keys.Troll)
                {
                    Logger.Log(LogType.Info, Settings.Messages.TrollKey, ConsoleColor.Red);
                    if (Settings.ShowTrollVideo && !string.IsNullOrEmpty(Settings.TrollVideoFileName))
                    {
                        DirectoryInfo dir = new(Directory.GetCurrentDirectory());
                        var videoFile = dir.EnumerateFiles(Settings.TrollVideoFileName, SearchOption.AllDirectories).FirstOrDefault();
                        if (videoFile != null)
                        {
                            new Process
                            {
                                StartInfo = new ProcessStartInfo(videoFile.FullName)
                                {
                                    UseShellExecute = true
                                }
                            }.Start();
                        }
                    }
                }
                else
                {
                    Logger.Log(LogType.Info, Settings.Messages.IncorrectKey, ConsoleColor.Red);
                }
            }
            else
            {
                Logger.Log(LogType.Error, Settings.Messages.ReadError);
            }
        }
        // Structure used by GetWindowRect
        struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        private static void Maximize()
        {
            // Import the necessary functions from user32.dll
            [DllImport("user32.dll")]
            static extern IntPtr GetForegroundWindow();
            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            [DllImport("user32.dll")]
            static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
            [DllImport("user32.dll")]
            static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
            // Constants for the ShowWindow function
            const int SW_MAXIMIZE = 3;
            // Get the handle of the console window
            IntPtr consoleWindowHandle = GetForegroundWindow();
            // Maximize the console window
            ShowWindow(consoleWindowHandle, SW_MAXIMIZE);
            // Get the screen size
            Rect screenRect;
            GetWindowRect(consoleWindowHandle, out screenRect);
            // Resize and reposition the console window to fill the screen
            int width = screenRect.Right - screenRect.Left;
            int height = screenRect.Bottom - screenRect.Top;
            MoveWindow(consoleWindowHandle, screenRect.Left, screenRect.Top, width, height, true);
        }
    }
}