using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquadLib.Entities.Breakout;

public class Brick :BreakoutEntity
{
   public Brick(Texture2D texture, Rectangle boundingBox, Color color)
   {
      this.Texture = texture;
      this.Color = color;
      this.BoundingBox = boundingBox;
   }
}

public class BreakoutEntity
{
    public Rectangle BoundingBox { get; set; }
    public Texture2D Texture { get; set; }
    public Color Color { get; set; }
    public Vector2 Motion { get; set; }
    public bool IsAlive { get; set; }
}