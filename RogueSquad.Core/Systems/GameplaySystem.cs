using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace RogueSquad.Core.Systems
{
    public class GameplayNode
    {
        public int Id { get; set; }
        public PositionComponent PositionData { get; set; }
        public BasicControllerComponent ControllerData { get; set; }
        public AIComponent AIData { get; set; }
        public CollidableComponent CollisionData { get; set; }
        public bool IsPlayer { get; set; }
        
        
    }
    public class GameplaySystem : IRogueSystem
    {
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { }; // we get every component

        List<GameplayNode> _nodes = new List<GameplayNode>();
        World _worldRef;
        GameplayNode _player;

        public GameplaySystem(World world)
        {
            _worldRef = world;
        }

        public void AddEntity(RogueEntity entity)
        {
            var controller = entity.GetComponentByType(ComponentTypes.BasicControllerComponent) as BasicControllerComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            var ai = entity.GetComponentByType(ComponentTypes.AIComponent) as AIComponent;
            var collisionData = entity.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;

            var gameplayNode = new GameplayNode { PositionData = position, ControllerData = controller, Id = entity.ID, AIData = ai, CollisionData = collisionData, IsPlayer = false };

            if (controller != null)
            {
                //this is a player node
                _player = gameplayNode;

            }
            else {
                _nodes.Add(gameplayNode);
            }
            
           
        }
        public bool HasEntity(RogueEntity entity)
        {
            foreach (var node in _nodes)
            {
                if (node.Id == entity.ID)
                    return true;
            }
            return false;
        }

        float aiSpeed = .14f; //move to position
        public void Update(GameTime gametime)
        {

            HandleInput(_player.ControllerData, _player.PositionData, gametime);

            //check if player is in an AI trigger boundary
            foreach (var gamePlayNode in _nodes)
            {
                if (_player.CollisionData.BoundingRectangle.Intersects(gamePlayNode.AIData.DetectionRadius))
                {
                    gamePlayNode.AIData.DetectedPlayer = true;
                }
                else
                {
                    gamePlayNode.AIData.DetectedPlayer = false;
                }


                //chase player if detected
                if (gamePlayNode.AIData.DetectedPlayer)
                {
                    Vector2 newPos = gamePlayNode.PositionData.Position;
                    if (_player.PositionData.Position.X > gamePlayNode.PositionData.Position.X)
                    {
                        //move player 
                        newPos.X += aiSpeed * gametime.ElapsedGameTime.Milliseconds;
                    }
                    if (_player.PositionData.Position.X < gamePlayNode.PositionData.Position.X)
                    {
                        //move player 
                        newPos.X -= aiSpeed * gametime.ElapsedGameTime.Milliseconds;
                    }
                    if (_player.PositionData.Position.Y > gamePlayNode.PositionData.Position.Y)
                    {
                        //move player 
                        newPos.Y += aiSpeed * gametime.ElapsedGameTime.Milliseconds;
                    }
                    if (_player.PositionData.Position.Y < gamePlayNode.PositionData.Position.Y)
                    {
                        //move player 
                        newPos.Y -= aiSpeed * gametime.ElapsedGameTime.Milliseconds;
                    }

                    gamePlayNode.PositionData.Position = newPos;
                }

                //move allies towards player if needed
                if (gamePlayNode.AIData.IsAlly)
                {
                    //update follow target to match player location
                    var newLocation = _player.PositionData.Position + gamePlayNode.AIData.FollowDistance;
                    gamePlayNode.AIData.FollowTarget = new RectangleF(newLocation.X, newLocation.Y, 2,2);

                    //if we're outside of our follow distance
                    if (!gamePlayNode.AIData.FollowTarget.Intersects(gamePlayNode.CollisionData.BoundingRectangle))
                    {
                        MoveTowardsTarget(gamePlayNode, gamePlayNode.AIData.FollowTarget, gametime);

                    }

                }
            }           
            


        }

        private void MoveTowardsTarget(GameplayNode gamePlayNode, RectangleF target, GameTime gametime)
        {
            Vector2 newPos = gamePlayNode.PositionData.Position;
            if (target.Position.X > gamePlayNode.PositionData.Position.X)
            {
                //move player 
                newPos.X += aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (target.Position.X < gamePlayNode.PositionData.Position.X)
            {
                //move player 
                newPos.X -= aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (target.Position.Y > gamePlayNode.PositionData.Position.Y)
            {
                //move player 
                newPos.Y += aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (target.Position.Y < gamePlayNode.PositionData.Position.Y)
            {
                //move player 
                newPos.Y -= aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }

            gamePlayNode.PositionData.Position = newPos;
        }

        private void MoveTowardsPlayer(GameplayNode gamePlayNode, GameTime gametime)
        {
            Vector2 newPos = gamePlayNode.PositionData.Position;
            if (_player.PositionData.Position.X > gamePlayNode.PositionData.Position.X)
            {
                //move player 
                newPos.X += aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (_player.PositionData.Position.X < gamePlayNode.PositionData.Position.X)
            {
                //move player 
                newPos.X -= aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (_player.PositionData.Position.Y > gamePlayNode.PositionData.Position.Y)
            {
                //move player 
                newPos.Y += aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (_player.PositionData.Position.Y < gamePlayNode.PositionData.Position.Y)
            {
                //move player 
                newPos.Y -= aiSpeed * gametime.ElapsedGameTime.Milliseconds;
            }

            gamePlayNode.PositionData.Position = newPos;

        }

        private void HandleInput(BasicControllerComponent controller, PositionComponent positionData, GameTime gameTime)
        {
            if (controller.AnyKeyPressed)
            {
                Vector2 newPos = positionData.Position;

                if (controller.KeyRetreat)
                {
                    positionData.Position = new Vector2(0, 0);
                }

                if (controller.KeyLeft)
                {
                    newPos.X -= positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;

                }
                if (controller.KeyRight)
                {
                    newPos.X += positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;

                }
                if (controller.KeyUp)
                {
                    newPos.Y -= positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;

                }
                if (controller.KeyDown)
                {
                    newPos.Y += positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
                }

                if (controller.KeyCreateRandomEntities)
                {
                    _worldRef.AddRandomEnemies(100,100);
                }

                if (controller.KeyTarget)
                {
                    //let's select a target 

                }


                positionData.Position = newPos;
            }
        }
    }
}
