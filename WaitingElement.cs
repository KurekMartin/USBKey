using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBKey
{
    internal class WaitingElement
    {
        private readonly string[] _steps = { "◝", "◞", "◟", "◜" };
        private bool _stop = false;
        private bool _running = false;
        private int _stepDuration = 10;
        public int StepDuration
        {
            get => _stepDuration;
            set
            {
                if (_stepDuration != value)
                {
                    if (value < 0)
                        _stepDuration = 0;
                    else
                        _stepDuration = value;
                }
            }
        }

        public WaitingElement(int stepDuration)
        {
            _stepDuration = stepDuration;
            Task.Run(() => { Loop(); });
        }

        private void Loop()
        {
            var cursorStartPos = Console.CursorLeft;
            int step = 0;
            _running = true;
            do
            {
                Console.Write(_steps[step]);
                step = (step + 1) % _steps.Length;
                Console.CursorLeft = cursorStartPos;
                Thread.Sleep(_stepDuration);
            } while (!_stop);
            Console.Write(new string(' ', Console.WindowWidth - cursorStartPos));
            Console.WriteLine();
            _running = false;
        }

        public void Stop()
        {
            _stop = true;
            while (_running) { }
        }

    }
}
