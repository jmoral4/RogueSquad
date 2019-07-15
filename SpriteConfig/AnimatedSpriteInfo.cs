using System.Drawing;

namespace SpriteConfig
{
    // This class is used for runtime configuration of the animated sprite data
    // frames, start/stop, aspect ratio? , framecounter, maxframes, 
    public class AnimatedSpriteInfo 
    {
        public string AnimationName { get; set; }
        public string SourceFileName { get; }
        public string SourceFullPath { get; }
        public int SourceWidth { get; }
        public int SourceHeight { get; }
        public string AnimatedSpriteFilename { get; set; } = string.Empty;
        public string AnimatedSpriteFullPath { get; set; } = string.Empty;

        public int FirstFrame { get; set; } 
        public int LastFrame { get; set; } 
        public int FrameWidth { get; set; } 
        public int FrameHeight { get; set; } 
        public int FrameRow { get; set; } 
        public int DesiredFrameRate { get; set; }  //in fps 
        protected Rectangle CurrentFrame { get; set; }


        public void UpdateCurrentFrame(Rectangle rect)
        {
            CurrentFrame = rect;
        }

        public Rectangle GetCurrentFrame() => CurrentFrame;

        public AnimatedSpriteInfo(string srcFilename, string srcFullPath, int srcWidth, int srcHeight)
        {
            //set some reasonable defaults
            FrameWidth = FrameHeight = 50;
            FirstFrame = LastFrame = 0;
            FrameRow = 0;
            DesiredFrameRate = 15;
            SourceFileName = srcFilename;
            SourceFullPath = srcFullPath;
            SourceHeight = srcHeight;
            SourceWidth = srcWidth;
            CurrentFrame = new Rectangle(0, 0, FrameWidth, FrameHeight);
            AnimatedSpriteFilename = "NewAnimation.spx";
            AnimationName = "NewIdleAnimation";
        }
    }
}
