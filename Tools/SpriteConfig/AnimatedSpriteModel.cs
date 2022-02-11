using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SpriteConfig
{
    // This class is used for runtime configuration of the animated sprite data
    // frames, start/stop, aspect ratio? , framecounter, maxframes, 
    public class AnimatedSpriteModel 
    {        
        [Description("The name you will use to reference this animation.")]
        public string AnimationName { get; set; }
        [Description("The filename of the source image")]
        public string SourceFileName { get; }
        [Description("The full path of the source image")]
        public string SourceFullPath { get; }
        [Description("The width of the source file.")]
        public int SourceWidth { get; }
        [Description("The height of the source file.")]
        public int SourceHeight { get; }
        [Description("The name of this file when saved.")]
        public string AnimatedSpriteFilename { get; set; } = string.Empty;                        
        [Description("The first frame to be used in the image (starting from 0) calculated using the FameWidth, FrameHeight, and Row.")]
        public int FirstFrame { get; set; } 
        private int _lastFrame { get; set; }
        [Description("The last Frame to be used in the animation (calculated using the FrameWidth, FrameHeight, and Row). Defaults to the max of this calculated value.")]
        public int LastFrame 
        {
            get { return _lastFrame; }  
            set {
                _lastFrame = value;
                SaveValidateAndUpdate();
            } 
        }
        [Description("The width of an individual frame of this animation.")]
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
        [Description("The height of an individual frame of this animaiton.")]
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
        [Description("The row that this animation starts at (for multi-row sheets)")]
        public int FrameRow { get; set; }
        [Description("The desired playback speed (for viewing in-app only, value is exported but has to be implemented manually)")]
        public int DesiredFrameRate { get; set; }  //in fps 
        [Description("Rectanble representing the current Frame being rendered")]
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

        public AnimatedSpriteModel(string srcFilename, string srcFullPath, int srcWidth, int srcHeight)
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
