using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquadLib.Entities.Breakout;

public class Ball:BreakoutEntity
{
    public float rotation = 0;
    public Ball(Texture2D texture, Rectangle boundingBox, Color color)
    {
        this.Texture = texture;
        this.Color = color;
        this.BoundingBox = boundingBox;
    }
}