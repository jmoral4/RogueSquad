using System.Drawing;

namespace SpriteConfig
{
    // Two options here. Either we load the sprite as a discrete segment and parse using the frame data
    // or we load as a series of rectangles which _may_ load directly into the engine.
    // key requirements, the sourcefilename, the collision data, and the rectangle being snipped from the 
    // source file (i.e., or frame dimensions within that asource sprite row)


    public class AnimatedSpritePersistData
    {
        public string SourceFileName {get;set;}
        public int DesiredFrameRate {get;set;}
        public int StartingRow {get;set;}
        public int StartingFrame {get;set;}
        public int FrameCount {get;set;}
        public int FrameWidth {get;set;}
        public int FrameHeight {get;set;}
        public Rectangle CollisionBoundary {get;set;}
        public Rectangle SourceSpriteRow {get;set;}
    }
}
