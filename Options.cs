﻿namespace USBKey
{
    internal class Options
    {
        public string DataFileName { get; set; } = "data";
        public Keys Keys { get; set; } = new();
    }

    internal class Keys
    {
        public string Correct { get; set; } = "cky";
        public string Troll { get; set; } = "tky";
    }
}
