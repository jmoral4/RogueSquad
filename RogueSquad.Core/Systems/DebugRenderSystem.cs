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
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace RogueSquad.Core.Systems
{
    public class DebugRenderSystem : IRogueRenderSystem
    {
        SpriteBatch batchRef;
        Camera2D _camera;
        ViewportAdapter _viewportAdapter;
        public IList<DebugRenderNode> _renderNodes = new List<DebugRenderNode>();
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.SpriteComponent, ComponentTypes.PositionComponent, ComponentTypes.CollidableComponent, ComponentTypes.AIComponent};
        public DebugRenderSystem(SpriteBatch batch, Camera2D camera, ViewportAdapter viewportAdapter)
        {
            batchRef = batch;
            _camera = camera;
            _viewportAdapter = viewportAdapter;
        }

        public void AddEntity(RogueEntity entity)
        {
            var render = entity.GetComponentByType(ComponentTypes.SpriteComponent) as SpriteComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            var collision = entity.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;
            var ai = entity.GetComponentByType(ComponentTypes.AIComponent) as AIComponent;
            _renderNodes.Add(new DebugRenderNode { Position = position, Renderable = render, Collision=collision, Id = entity.ID , AIData = ai});
        }
        public bool HasEntity(RogueEntity entity)
        {
            foreach (var node in _renderNodes)
            {
                if (node.Id == entity.ID)
                    return true;
            }
            return false;
        }
        public void Draw(GameTime gameTime)
        {
            batchRef.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            foreach (var entity in _renderNodes)
            {
                var color = entity.Collision.HasCollision ? Color.Red : Color.Green;

                batchRef.DrawRectangleF(entity.Collision.BoundingRectangle, color);

                if (!entity.AIData.IsPlayer)
                {
                    if (entity.AIData.DetectedPlayer)
                    {
                        batchRef.DrawRectangle(entity.AIData.DetectionRadius, Color.Orange, 1f);
                    }
                    else
                    {
                        batchRef.DrawRectangle(entity.AIData.DetectionRadius, Color.Blue, 1f);
                    }
                    if (entity.AIData.HasPatrolArea)
                    {
                        batchRef.DrawRectangle(entity.AIData.PatrolArea, Color.Pink);
                    }
                }

            }
            batchRef.End();
        }
        public void Update(GameTime gameTime) { }
    }

}
