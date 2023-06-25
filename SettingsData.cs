using System.CodeDom;

namespace USBKey
{
    internal class SettingsData
    {
        public string DataFileName { get; set; } = "data";
        public Keys Keys { get; set; } = new();
        public Stage[] Stages { get; set; } = Array.Empty<Stage>();
    }
    internal class Keys
    {
        public string Correct { get; set; } = "cky";
        public string Troll { get; set; } = "tky";
    }
    public enum StageType
    {
        Message, Progress
    }
    internal class Stage
    {
        public StageType Type { get; set; } = StageType.Message;
        public string Text { get; set; } = string.Empty;
        private int _duration = 1000;
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = Math.Max(value, 0);
            }
        }
        private int _stepDuration = 100;
        public int StepDuration
        {
            get => _stepDuration;
            set
            {
                _stepDuration = Math.Max(value, 1);
            }
        }
        private int _stepDurationVariance = 0;
        public int StepDurationVariance
        {
            get => _stepDurationVariance;
            set
            {
                _stepDurationVariance = Math.Max(value, 0);
            }
        }
        private byte _progressBarLength = 30;
        public byte ProgressBarLength
        {
            get => _progressBarLength;
            set
            {
                _progressBarLength = Math.Max(value, (byte)10);
            }
        }
        private byte _maxStep = 10;
        public byte MaxStep
        {
            get => _maxStep;
            set
            {
                _maxStep = Math.Max(value, (byte)1);
            }
        }

    }
}
