using Microsoft.Xna.Framework.Graphics;

namespace RogueSquad.Core.Components
{
    public class RenderableComponent : IRogueComponent {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.RenderableComponent;
        public Texture2D Texture { get; set; }

    }

  

}
