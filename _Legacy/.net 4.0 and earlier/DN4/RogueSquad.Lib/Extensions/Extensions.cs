using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Extensions
{
    public static class Extensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, SpriteEffects spriteEffect)
        {
            if (sprite == null) throw new ArgumentNullException(nameof(sprite));

            if (sprite.IsVisible)
            {
                var texture = sprite.TextureRegion.Texture;
                var sourceRectangle = sprite.TextureRegion.Bounds;

                spriteBatch.Draw(texture, sprite.Position, sourceRectangle, sprite.Color * sprite.Alpha, sprite.Rotation,
                    sprite.Origin,
                    sprite.Scale, spriteEffect, sprite.Depth);
            }
        }

        public static void DrawFlipped(this SpriteBatch spriteBatch, Sprite sprite)
        {
            if (sprite == null) throw new ArgumentNullException(nameof(sprite));

            if (sprite.IsVisible)
            {
                var texture = sprite.TextureRegion.Texture;
                var sourceRectangle = sprite.TextureRegion.Bounds;

                spriteBatch.Draw(texture, sprite.Position, sourceRectangle, sprite.Color * sprite.Alpha, sprite.Rotation,
                    sprite.Origin,
                    sprite.Scale, SpriteEffects.FlipVertically, sprite.Depth);
            }
        }
    }
}
