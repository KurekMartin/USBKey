namespace USBKey
{
    internal class SettingsData
    {
        public string DataFileName { get; set; } = "data";
        public Keys Keys { get; set; } = new();
        public string[] Messages { get; set; } = Array.Empty<string>();
    }
    internal class Keys
    {
        public string Correct { get; set; } = "cky";
        public string Troll { get; set; } = "tky";
    }
}
