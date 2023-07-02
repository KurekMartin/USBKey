namespace USBKey
{
    internal class ProgressBar
    {
        private readonly string _taskName;
        private readonly int _length = 20;
        private int _value = 0;
        public readonly int FinishWait = 1000;
        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = Math.Clamp(value, 0, 100);
                    Draw();
                }
            }
        }
        public ProgressBar(int length, string taskName)
        {
            var maxLength = Console.WindowWidth - taskName.Length - 3;
            _length = Math.Min(length, maxLength);
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
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('-', empty));
            Console.ForegroundColor = color;
            Console.Write("]");


            if (Value == 100)
            {
                Thread.Sleep(FinishWait);
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
