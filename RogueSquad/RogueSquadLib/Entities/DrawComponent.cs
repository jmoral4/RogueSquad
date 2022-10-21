using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquadLib.Entities
{
    class DrawComponent
    { 
        public Vector2 Scale { get; set; }
        public Color Tint { get; set; }
        public Texture2D Texture { get; set; }
        public int TextureId { get; set; }
        public Rectangle SrcRect { get; set; } // for rendering _from
    
    }

}
