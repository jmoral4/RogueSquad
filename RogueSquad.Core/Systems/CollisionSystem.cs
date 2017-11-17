﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using RogueSquad.Core.Nodes;
using MonoGame.Extended;

namespace RogueSquad.Core.Systems
{
    public class CollisionSystem : IRogueSystem
    {
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.CollidableComponent };
        private IList<CollisionNode> _collisionNodes = new List<CollisionNode>();

        public void AddEntity(RogueEntity entity)
        {
            var collision = entity.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;

            if (collision == null || position == null)
                throw new Exception("Entity did not have the required components to add to Collision System!");

            _collisionNodes.Add(new CollisionNode { Position = position, CollisionData = collision, Id = entity.ID });
        }

        public void Update(GameTime gametime)
        {
            UpdateAllCollisionRectangles();
            // Note: Any collisions can only happen once per object per evaluation... unless we chance 'collided with' to be an array or evrything colliding that update()
            // This does boost performance by only performing 1 check per node on both ends reducing n^2
            // in the present system, collision with projectile could be avoided by touching a wall perhaps...
            foreach (var node in _collisionNodes)
            {                
                //check all collissions and...?
                foreach (var n in _collisionNodes)
                {
                    if (node.Id != n.Id )
                    {
                       //have to create a new collision rect based on the current location
                       
                       var hasCollision = node.CollisionData.BoundingRectangle.Intersects(n.CollisionData.BoundingRectangle);
                        if (hasCollision)
                        {
                            node.CollisionData.Collided();
                            node.CollidedWith = n.Id;                            
                        }
                    }
                }
            }

        }


        private void UpdateAllCollisionRectangles()
        {
            foreach (var node in _collisionNodes)
            {
                //update rect for the correct object only. 
                node.CollisionData.UpdateBoundingRectangle(new RectangleF(node.Position.Position.X, node.Position.Position.Y, node.CollisionData.BoundingRectangle.Width, node.CollisionData.BoundingRectangle.Height));
                node.CollisionData.ResetCollision();
                node.CollidedWith = 0;
            }
        }

    }
}
