namespace Colonia.Engine
{
    internal class Settings
    {
        public int ResolutionWidth { get; set; } = 1280;
        public int ResolutionHeight { get; set; } = 720;
        public bool Fullscreen { get; set; } = false;
        public bool Borderless { get; set; } = true;
        public bool VSync { get; set; } = false;
        public bool ShowFrameRate { get; set; } = true;
        public bool FixedFrameRate { get; set; } = false;
        public float TargetFrameRate { get; set; } = 60.0f;
        public float GlobalVolume { get; set; } = 1.0f;
        public float SFXVolume { get; set; } = 1.0f;
        public float MusicVolume { get; set; } = 0.8f;
        public string Language { get; set; } = "en-US";
    }
}
