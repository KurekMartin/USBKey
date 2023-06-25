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
        public uint Duration = 1000;
        public uint DurationVariance = 0;
        private byte _progressBarLength = 30;
        public byte ProgressBarLength
        {
            get => _progressBarLength;
            set
            {
                if(value < 10)
                {
                    _progressBarLength = 10;
                }
                else
                {
                    _progressBarLength = value;
                }
            }
        }
        private byte _maxStep = 10;
        public byte MaxStep
        {
            get => _maxStep;
            set
            {
                if (value < 1)
                {
                    _maxStep = 1;
                }
                else
                {
                    _maxStep = value;
                }
            }
        }

    }
}
