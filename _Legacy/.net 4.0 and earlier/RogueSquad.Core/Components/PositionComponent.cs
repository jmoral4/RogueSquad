using Microsoft.Xna.Framework;

namespace RogueSquad.Core.Components
{
    public class PositionComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.PositionComponent;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Speed { get; set; } = 0f;


    }

  

}
