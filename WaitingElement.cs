
using System.Diagnostics;

namespace USBKey
{
    internal class WaitingElement
    {
        private readonly string[] _steps = { "◝", "◞", "◟", "◜" };
        private bool _running = false;
        private int _stepDuration;
        public int StepDuration
        {
            get => _stepDuration;
            set
            {
                _stepDuration = Math.Max(value, 0);
            }
        }

        public WaitingElement(int stepDuration = 150)
        {
            _stepDuration = stepDuration;
        }

        private async void Loop(CancellationToken token)
        {
            var cursorStartPos = Console.CursorLeft;
            int step = 0;
            _running = true;
            do
            {
                Console.Write(_steps[step]);
                step = (step + 1) % _steps.Length;
                Console.CursorLeft = cursorStartPos;
                try
                {
                    await Task.Delay(_stepDuration, token);
                }
                catch
                {
                    break;
                }
            } while (!token.IsCancellationRequested);
            Console.Write(new string(' ', Console.WindowWidth - cursorStartPos));
            Console.WriteLine();
            _running = false;
        }

        public bool Running
        {
            get => _running;
        }

        public void Show(CancellationToken token)
        {
            Loop(token);
        }
    }
}
