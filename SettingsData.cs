

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace USBKey
{
    internal class SettingsData
    {
        public string DataFileName = "data";
        public Keys Keys = new();
        public int? Seed;
        public bool Maximize = false;
        public Stage[] Stages = Array.Empty<Stage>();
        public Messages Messages = new();
        public bool ShowTrollVideo = false;
        public string TrollVideoFileName = string.Empty;
    }
    internal class Keys
    {
        public string Correct = "cky";
        public string Troll = "tky";
    }
    public enum StageType
    {
        Message, Progress
    }
    internal class Stage
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public StageType Type = StageType.Message;
        public string Text = string.Empty;
        private int _duration = 1000;
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = Math.Max(value, 0);
            }
        }
        private float _stepDurationVariance = 0;
        public float StepDurationVariance
        {
            get => _stepDurationVariance;
            set
            {
                _stepDurationVariance = Math.Clamp(value, 0, 1);
            }
        }
        private float _stepProgressVariance = 0;
        public float StepProgressVariance
        {
            get => _stepProgressVariance;
            set
            {
                _stepProgressVariance = Math.Clamp(value, 0, 1);
            }
        }
        private int _progressBarLength = 30;
        public int ProgressBarLength
        {
            get => _progressBarLength;
            set
            {
                _progressBarLength = Math.Max(value, 10);
            }
        }
        private byte _maxStepCount = 10;
        public byte MaxStepCount
        {
            get => _maxStepCount;
            set
            {
                _maxStepCount = Math.Clamp(value, (byte)1, (byte)100);
            }
        }

    }
}
