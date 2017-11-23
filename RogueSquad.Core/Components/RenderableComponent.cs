using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquad.Core.Components
{
    public class SpriteComponent : IRogueComponent {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.SpriteComponent;
        public Texture2D Texture { get; set; }
        public Point Size { get; set; }

        public Rectangle Source { get; set; }

    }

  

}
