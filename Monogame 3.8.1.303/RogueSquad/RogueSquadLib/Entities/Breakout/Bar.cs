using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquadLib.Entities.Breakout;

public class Bar:BreakoutEntity
{
    public Bar(Texture2D texture, Rectangle boundingBox, Color color)
    {
        this.Texture = texture;
        this.Color = color;
        this.BoundingBox = boundingBox;
    }
}