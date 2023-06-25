using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBKey
{
    internal class ProgressBar
    {
        private string _taskName;
        private int _length = 20;
        private int _value = 0;
        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    if (value < 0)
                        _value = 0;
                    else if (value > 100)
                        _value = 100;
                    else
                        _value = value;
                    Draw();
                }
            }
        }
        public ProgressBar(int length, string taskName)
        {
            _length = length;
            _taskName = taskName;
        }
        private void Draw()
        {
            var color = Console.ForegroundColor;
            ConsoleUtil.ClearCurrentLine();
            
            Console.Write($"{_taskName} [");
            int full = (int)Math.Round(Value / (100.0 / _length));
            int empty = _length - full;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(new string('=', full));
            Console.ForegroundColor = color;
            Console.Write(new string('-', empty) + "]");
            

            if(Value == 100)
            {
                Thread.Sleep(200);
                ConsoleUtil.ClearCurrentLine();
                Console.Write(_taskName);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" ✓\n");
                Console.ForegroundColor = color;
            }
            else
            {
                ConsoleUtil.MoveCursorToStart();
            }
        }
    }
}
