using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SpriteConfig
{
    // This class is used for runtime configuration of the animated sprite data
    // frames, start/stop, aspect ratio? , framecounter, maxframes, 
    public class AnimatedSpriteInfo 
    {
        public int version { get; set; } = 1;
        public string AnimationName { get; set; }
        public string SourceFileName { get; }
        public string SourceFullPath { get; }
        public int SourceWidth { get; }
        public int SourceHeight { get; }
        public string AnimatedSpriteFilename { get; set; } = string.Empty;
        public string AnimatedSpriteFullPath { get; set; } = string.Empty;

        public int FirstFrame { get; set; } 
        private int _lastFrame { get; set; }
        public int LastFrame 
        {
            get { return _lastFrame; }  
            set {
                _lastFrame = value;
                SaveValidateAndUpdate();
            } 
        } 
        public int FrameWidth
        {
            get { return _frameWidth; }
            set
            {                
                _frameWidth = value;
                SaveValidateAndUpdate();
            }
        }
        private int _frameWidth;
        public int FrameHeight 
        {
            get { return _frameHeight; }
            set
            {
                _frameHeight = value;
                SaveValidateAndUpdate();
            }
        }
        private int _frameHeight;
            
        private int _maxFrameHeight;
        private int _maxFrameWidth;
        public int FrameRow { get; set; } 
        public int DesiredFrameRate { get; set; }  //in fps 
        protected Rectangle CurrentFrame { get; set; }


        public void UpdateCurrentFrame(Rectangle rect)
        {
            CurrentFrame = rect;
        }

        public Rectangle GetCurrentFrame() => CurrentFrame;

        private void SaveValidateAndUpdate()
        {
            // calculate the framewith/height
            if (_frameHeight > _maxFrameHeight)
            
                _frameHeight = _maxFrameHeight;

            if (_frameWidth > 0)
            {
                if (_frameWidth > _maxFrameWidth)
                    _frameWidth = _maxFrameWidth;

                if (_lastFrame > SourceWidth / _frameWidth)
                    _lastFrame = SourceWidth / _frameWidth;
            }
             
        }

        public AnimatedSpriteInfo(string srcFilename, string srcFullPath, int srcWidth, int srcHeight)
        {
            SourceHeight = srcHeight;
            _maxFrameHeight = SourceHeight;
            SourceWidth = srcWidth;
            _maxFrameWidth = SourceWidth;
            SourceFileName = srcFilename;
            SourceFullPath = srcFullPath;


            //set some reasonable defaults
            FrameWidth = FrameHeight = 32;
            FirstFrame = LastFrame = 0;
            FrameRow = 0;
            DesiredFrameRate = 15;           
            CurrentFrame = new Rectangle(0, 0, FrameWidth, FrameHeight);

            AnimatedSpriteFilename = "NewAnimation.spx";
            AnimationName = "NewIdleAnimation";
        }
    }
}
