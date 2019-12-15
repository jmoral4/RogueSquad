using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLite
{
    class AnimatedSprite
    {
        public Rectangle TextureStrip { get; set; }
        public Texture2D Texture { get; set; }
        protected int CurrentFrame { get; set;}
        public double FrameRate { get; set; }
        public Rectangle CurrentFrameRectangle { get; set; }
        public Vector2 FrameDimensions { get; set; }
        public readonly int TotalFrames;

        double _elapsedMS = 0;

        public AnimatedSprite(Texture2D texture, Rectangle textureStrip, Vector2 dimensions, double frameRate)
        {
            Texture = texture;
            TextureStrip = textureStrip;
            FrameDimensions = dimensions;
            CurrentFrameRectangle = new Rectangle(textureStrip.X, textureStrip.Y, (int)dimensions.X, (int)dimensions.Y);
            FrameRate = frameRate;
            TotalFrames = TextureStrip.Width / CurrentFrameRectangle.Width;
        }

        public void Draw(Rectangle destination, SpriteBatch batch)
        {
            //get textyre
            batch.Draw(Texture, destination, CurrentFrameRectangle, Color.White);
        }

        private void UpdateFrameRectangle()
        {
            CurrentFrameRectangle = new Rectangle((int)(CurrentFrame * FrameDimensions.X), (int)TextureStrip.Y, (int)FrameDimensions.X, (int)FrameDimensions.Y);
        }

        public void Update(GameTime gameTime)
        {

            // check if elapsed is our target framerate
            if ((_elapsedMS) > (1000.0d / FrameRate))
            {
                CurrentFrame++;
                if (CurrentFrame > TotalFrames)
                {
                    CurrentFrame = 0;
                }
                UpdateFrameRectangle();
                _elapsedMS = 0;
            }
            else
            {
                _elapsedMS += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

        }
        
    }
}
