using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core.Nodes;
using RogueSquad.Core.Components;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Sprites;

namespace RogueSquad.Core.Systems
{

    public class RenderingSystem : IRogueRenderSystem
    {
        RenderNode _playerNodeRef;
        SpriteBatch batchRef;
        Camera2D _camera;
        ViewportAdapter _viewportAdapter;
        public IList<RenderNode> _renderNodes = new List<RenderNode>();
        public IEnumerable<ComponentTypes> Required { get; set; } = new ComponentTypes[] { ComponentTypes.SpriteComponent, ComponentTypes.PositionComponent };
        public RenderingSystem(SpriteBatch batch, Camera2D camera, ViewportAdapter viewportAdapter)
        {
            batchRef = batch;
            _camera = camera;
            _viewportAdapter = viewportAdapter;
        }

        public void AddEntity(RogueEntity entity)
        {
            var render = entity.GetComponentByType(ComponentTypes.SpriteComponent) as SpriteComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            var anim = entity.GetComponentByType(ComponentTypes.SpriteComponent) as AnimatedSpriteComponent;
            var animationStateInfo = entity.GetComponentByType(ComponentTypes.AnimationStateInfoComponent) as AnimationStateInfoComponent;
            var renderNode = new RenderNode { Position = position, StaticSpriteData = render, Id = entity.ID, AnimatedSpriteData = anim, AnimationState = animationStateInfo };
            _renderNodes.Add(renderNode);
            if (entity.HasComponent(ComponentTypes.BasicControllerComponent))
            {
                anim.AnimatedSprite.Play(anim.CurrentAnimation);
                _playerNodeRef = renderNode;
            }
                
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
            var sourceRectangle = new Rectangle(0, 0, _viewportAdapter.VirtualWidth, _viewportAdapter.VirtualHeight);
            sourceRectangle.Offset(_camera.Position * new Vector2(0.1f));

            batchRef.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            foreach (var entity in _renderNodes)
            {
                if (entity.AnimatedSpriteData == null)
                {
                    if (entity.StaticSpriteData.Size != Point.Zero)
                    {
                        batchRef.Draw(entity.StaticSpriteData.Texture, new Rectangle((int)entity.Position.Position.X, (int)entity.Position.Position.Y, entity.StaticSpriteData.Size.X, entity.StaticSpriteData.Size.Y), null, Color.White);
                    }
                    else
                    {
                        batchRef.Draw(entity.StaticSpriteData.Texture, entity.Position.Position, Color.White);
                    }
                }
                else
                {                    
                    batchRef.Draw(entity.AnimatedSpriteData.AnimatedSprite);
                }
                
                
            }
            batchRef.End();

        }


        public void Update(GameTime gameTime) {
            _camera.LookAt(_playerNodeRef.Position.Position);

            //update any animations
            foreach (var entity in _renderNodes)
            {
                if (entity.AnimatedSpriteData != null)
                {
                    // make sure we're playing the latest animation
                    if (entity.AnimatedSpriteData.CurrentAnimation != entity.AnimationState.CurrentAnimationName)
                    {
                        //we've switched animations -- toggle and play
                        entity.AnimatedSpriteData.CurrentAnimation = entity.AnimationState.CurrentAnimationName;
                        entity.AnimatedSpriteData.AnimatedSprite.Play(entity.AnimatedSpriteData.CurrentAnimation);
                    }

                    entity.AnimatedSpriteData.AnimatedSprite.Position = entity.Position.Position;
                    entity.AnimatedSpriteData.AnimatedSprite.Update(gameTime);

                }

            }


           // _camera.Zoom = 1.0f;
        }
    }

}