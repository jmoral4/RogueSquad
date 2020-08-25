using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueSquad.Core.Systems
{
    public static class PSBlendState
    {
        public static BlendState Multiply = new BlendState
        {
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState Screen = new BlendState
        {
            ColorSourceBlend = Blend.InverseDestinationColor,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState Darken = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Min,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState Lighten = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Max,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState LinearDodge = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState LinearBurn = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
    }

    public class CanvasRenderSystem : IRogueRenderSystem
    {
        public IEnumerable<ComponentTypes> Required { get; set; } = new ComponentTypes[] { ComponentTypes.TileComponent };

        ContentManager _content;
        Point _mapSize;
        Texture2D _waterColorPaper;
        Texture2D _background;
        SpriteBatch _renderBatch;
        BlendState _multiplyBlend;

        public CanvasRenderSystem( ContentManager content, Point mapSize, SpriteBatch batch)
        {
            _content = content;
            _mapSize = mapSize;
            _renderBatch = batch;
            LoadContent();
        }

        public void LoadContent()
        {
            //sumi-parts_0001_water-color-paper-mulitiply
            //sumi-parts_0018_background

            _waterColorPaper = _content.Load<Texture2D>("Sumi/sumi-parts_0001_water-color-paper-mulitiply");
            _background = _content.Load<Texture2D>("Sumi/sumi-parts_0018_background");

            _multiplyBlend = new BlendState
            {
                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.DestinationColor,
                ColorDestinationBlend = Blend.Zero 
            };
            

        }

        private Rectangle Screen = new Rectangle(0,0,1920, 1080);

    public void AddEntity(RogueEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {


            _renderBatch.Begin(blendState: PSBlendState.Multiply);
            
            _renderBatch.Draw(_background, Screen, Color.White);
            _renderBatch.Draw(_waterColorPaper,Screen, Color.White);

            _renderBatch.End();

            
        }

        public bool HasEntity(RogueEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            //throw new NotImplementedException();
        }
    }
}
