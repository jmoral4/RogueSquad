using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core.Nodes;
using RogueSquad.Core.Components;

namespace RogueSquad.Core.Systems
{

    public class RenderingSystem : IRogueRenderSystem
    {
        SpriteBatch batchRef;
        public IList<RenderNode> _renderNodes = new List<RenderNode>();
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.SpriteComponent, ComponentTypes.PositionComponent };
        public RenderingSystem(SpriteBatch batch )
        {
            batchRef = batch;
        }

        public void AddEntity(RogueEntity entity)
        {
            var render = entity.GetComponentByType(ComponentTypes.SpriteComponent) as SpriteComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            _renderNodes.Add(new RenderNode { Position = position, Renderalble =  render, Id = entity.ID });
        }

        public void Draw(GameTime gameTime)
        {
            batchRef.Begin();
            foreach (var entity in _renderNodes)
            {
                batchRef.Draw(entity.Renderalble.Texture, entity.Position.Position, Color.White);                
            }
            batchRef.End();
        }

        public void Update(GameTime gameTime) { }
    }

}