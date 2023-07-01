

using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace USBKey
{
    internal class SettingsData
    {
        public string DataFileName = "data";
        public Keys Keys = new();
        public int? Seed;
        public Stage[] Stages = Array.Empty<Stage>();
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
        private byte _progressBarLength = 30;
        public byte ProgressBarLength
        {
            get => _progressBarLength;
            set
            {
                _progressBarLength = Math.Max(value, (byte)10);
            }
        }
        private byte _maxStepCount = 10;
        public byte MaxStepCount
        {
            get => _maxStepCount;
            set
            {
                _maxStepCount = Math.Max(value, (byte)1);
            }
        }

    }
}
