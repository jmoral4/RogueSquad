using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteConfig
{
    class ExportableGameSprite
    {
        public TextureAtlas TextureAtlas {  get; set; }
        public List<AnimationCycle> AnimationCycle {  get; set; }
        
    }

    class TextureAtlas
    { 
        public string Texture {  get; set; }
        public int RegionWidth {  get; set; }
        public int RegionHeight {  get; set; }
        public string SourceFilePath {  get; set; }
        public int SourceWidth {  get; set; }
        public int SourceHeight {  get; set; }        
    }

    class AnimationCycle
    { 
        public string Name { get; set; }
        public List<int> Frames {  get; set; }
        public bool IsLooping { get; set; } = false;
        public bool IsPingPong { get; set; } = false;
        public int DesiredFrameRate { get; set; } = 15;
    }

}
