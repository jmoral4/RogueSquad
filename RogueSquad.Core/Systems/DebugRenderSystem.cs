using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSquad.Core.Nodes;
using RogueSquad.Core.Components;
using RogueSquad.Core.Primitives;

namespace RogueSquad.Core.Systems
{
    public class DebugRenderSystem : IRogueRenderSystem
    {
        SpriteBatch batchRef;
        public IList<DebugRenderNode> _renderNodes = new List<DebugRenderNode>();
        public DebugRenderSystem(SpriteBatch batch)
        {
            batchRef = batch;
        }

        public void AddEntity(RogueEntity entity)
        {
            var render = entity.GetComponentByType(ComponentTypes.RenderableComponent) as RenderableComponent;
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
    }


    public class DebugRenderNode : INode
    {
        public int Id { get; set; }
        public RenderableComponent Renderable { get; set; }
        public PositionComponent Position { get; set; }
        public CollidableComponent Collision { get; set; }


    }

}
