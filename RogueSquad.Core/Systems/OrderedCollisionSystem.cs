using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RogueSquad.Core.Components;
using RogueSquad.Core.Nodes;
using System;
using System.Collections.Generic;
using RogueSquad.Core.Algorithms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RogueSquad.Core.Systems
{
    public class OrderedCollisionSystem : IRogueSystem
    {
        private int _width;
        private int _height;
        public OrderedCollisionSystem(int width, int height)
        {
            _width = width;
            _height = height;
            //insert all primary nodes
            for (int i = 0; i < 16; i++)
            {
                _collisionNodes.Insert(i, new List<CollisionNode>());
            }
        }

        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.CollidableComponent };
        private BTree<int, IList<CollisionNode>> _collisionNodes = new BTree<int, IList<CollisionNode>>(16);
        private IList<CollisionNode> _allNodes = new List<CollisionNode>();

        public void AddEntity(RogueEntity entity)
        {
            var collision = entity.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;

            if (collision == null || position == null)
                throw new Exception("Entity did not have the required components to add to Collision System!");

            var quadrant = GetTreeLocation(position.Position);
            var node = new CollisionNode { Position = position, CollisionData = collision, Id = entity.ID };

            _collisionNodes.Search(quadrant).Pointer.Add(node);
            _allNodes.Add(node);
        }
        public bool HasEntity(RogueEntity entity)
        {
           
            return false;
        }


        public int GetTreeLocation(Vector2 position)
        {
            //divide screen into 16 segments via x/y divided by 4
            int wNode = _width / 4;
            int hNode = _height / 4;

            int x = (int)position.X / wNode;
            int y = (int)position.Y / hNode;

            int quadrant = (y * (8)) + (x - (y * 8));
            return quadrant;
        }

        public void Update(GameTime gametime)
        {
            UpdateAllCollisionRectangles();
            UpdateCollissionTree();
            // Note: Any collisions can only happen once per object per evaluation... unless we chance 'collided with' to be an array or evrything colliding that update()
            // This does boost performance by only performing 1 check per node on both ends reducing n^2
            // in the present system, collision with projectile could be avoided by touching a wall perhaps...                

            foreach (var node in _allNodes)
            {
                //check against anything in the local tree
                var currentLoc = GetTreeLocation(node.Position.Position);
                var quadrant = _collisionNodes.Search(currentLoc).Pointer;
                

                //check all collissions and...?
                foreach (var n in quadrant)
                {
                    if (node.Id != n.Id)
                    {
                        //have to create a new collision rect based on the current location

                        var hasCollision = node.CollisionData.BoundingRectangle.Intersects(n.CollisionData.BoundingRectangle);
                        if (hasCollision)
                        {
                            node.CollisionData.Collided();
                            node.CollidedWith = n.Id;
                        }
                        else
                        {
                            node.CollisionData.ResetCollision();
                            node.CollidedWith = -1;
                        }

                    }
                }
            }

        }

        private void UpdateCollissionTree()
        {
            //clear the tree
            for (int i = 0; i < 16; i++)
            {
                _collisionNodes.Search(i).Pointer.Clear();
            }

            foreach (var node in _allNodes)
            {
                var quad = GetTreeLocation(node.Position.Position);
                _collisionNodes.Search(quad).Pointer.Add(node);
            }

        }


        private void UpdateAllCollisionRectangles()
        {
            foreach (var node in _allNodes)
            {
                //update rect for the correct object only. 
                node.CollisionData.UpdateBoundingRectangle(new RectangleF(node.Position.Position.X, node.Position.Position.Y, node.CollisionData.BoundingRectangle.Width, node.CollisionData.BoundingRectangle.Height));
            }
        }

    }
}

