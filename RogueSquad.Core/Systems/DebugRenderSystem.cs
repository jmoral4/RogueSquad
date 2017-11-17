using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSquad.Core.Components;
using RogueSquad.Core.Primitives;
using RogueSquad.Core.Nodes;

namespace RogueSquad.Core.Systems
{
    public class DebugRenderSystem : IRogueRenderSystem
    {
        SpriteBatch batchRef;
        public IList<DebugRenderNode> _renderNodes = new List<DebugRenderNode>();
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.SpriteComponent, ComponentTypes.PositionComponent, ComponentTypes.CollidableComponent };
        public DebugRenderSystem(SpriteBatch batch)
        {
            batchRef = batch;
        }

        public void AddEntity(RogueEntity entity)
        {
            var render = entity.GetComponentByType(ComponentTypes.SpriteComponent) as SpriteComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            var collision = entity.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;
            _renderNodes.Add(new DebugRenderNode { Position = position, Renderable = render, Collision=collision, Id = entity.ID });
        }

        public void Draw(GameTime gameTime)
        {
            batchRef.Begin();
            foreach (var entity in _renderNodes)
            {
                var color = entity.Collision.HasCollision ? Color.Red : Color.Green;

                batchRef.DrawRectangleF(entity.Collision.BoundingRectangle, color);

            }
            batchRef.End();
        }
        public void Update(GameTime gameTime) { }
    }

}
