using System.Drawing;

namespace SpriteConfig
{
    // This class is used for runtime configuration of the animated sprite data
    // frames, start/stop, aspect ratio? , framecounter, maxframes, 
    public class AnimatedSpriteInfo 
    {
        public string SourceFileName { get; set; }
        public int CurrentFrameCounter { get; set; } = 0;
        public int FirstFrame { get; set; } = 0;
        public int LastFrame { get; set; } = 0;
        public int FrameWidth { get; set; } = 8;
        public int FrameHeight { get; set; } = 8;
        public int FrameRow { get; set; } = 0;
        public int DesiredFrameRate { get; set; } = 15; //in fps 
        public Rectangle CurrentFrame { get; set; }
        
    }
}
