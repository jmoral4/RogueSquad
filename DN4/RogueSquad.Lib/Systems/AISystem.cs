using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;

using MonoGame.Extended;
using RogueSquad.Core.Nodes;
using System.Diagnostics;

namespace RogueSquad.Core.Systems
{
    public class AISystem : IRogueSystem
    {
        public IEnumerable<ComponentTypes> Required { get; set; } = new ComponentTypes[] { ComponentTypes.AIComponent};

        AINode _player;
        List<AINode> _aiNodes = new List<AINode>();
        //TODO: we need a playerinfocomponent which has stuff like the playernumber, etc, so we can pass that to systems as needed
        public void AddEntity(RogueEntity entity)
        {
            var ai = entity.GetComponentByType(ComponentTypes.AIComponent) as AIComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            var collisionData = entity.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;
            var patrolData = entity.GetComponentByType(ComponentTypes.PatrolComponent) as PatrolComponent;
            var followData = entity.GetComponentByType(ComponentTypes.FollowComponent) as FollowComponent;
            var aiNode = new AINode { AIData = ai, PositionData = position, CollisionData = collisionData, Id = entity.ID, FollowData = followData, PatrolData = patrolData };            

            if (ai.IsPlayerControlled)
            {
                _player = aiNode;
            }
            else
            {
                _aiNodes.Add(aiNode);
            }
        }

        private static bool CanPatrol(AINode ai) => ai.PatrolData != null;
        private static bool CanFollow(AINode ai) => ai.FollowData != null;        
        private static bool ExitedPatrolArea(AINode ai) => !ai.PatrolData.PatrolArea.Contains(ai.CollisionData.BoundingRectangle.Center);
        private static bool IsAtPatrolStart(AINode ai) => ai.PatrolData.PatrolArea.Intersects(ai.CollisionData.BoundingRectangle);

        public void Update(GameTime gameTime)
        {
            // check for any triggers on the AI component which need toggling and set as needed
            foreach (var ai in _aiNodes)
            {
                //match the ai's detection area to its current position if it's not static
                if (!ai.AIData.IsStatic)
                {
                    var pos = ai.PositionData.Position + ai.AIData.DetectionRange;
                    var size = ai.AIData.DetectionArea.Size;
                                                            
                    ai.AIData.DetectionArea = new RectangleF(pos, size);
                }

                //update all ai to see if player has triggered them -- we can perform behaviors as needed afterwards
                if (_player.CollisionData.BoundingRectangle.Intersects(ai.AIData.DetectionArea))
                {
                    ai.AIData.DetectedPlayer = true;
                }
                else
                {
                    ai.AIData.DetectedPlayer = false;
                }


                if (CanPatrol(ai))
                {                    
                    //move towards the detected player
                    if (ai.AIData.DetectedPlayer) {
                        if (!ai.PatrolData.FriendlyFactions.Contains(_player.AIData.Faction))
                        {
                            if (!ExitedPatrolArea(ai))
                            {
                                MoveTowardsPlayer(ai, gameTime);
                            }
                            else
                            {
                                //we can see the player but we've hit our patrol boundary!

                                                                
                            }
                        }
                        else {
                            // player is a friend faction
                        }
                    }
                    else
                    {
                        //player undetected..
                        ReturnToPatrolStart(ai, gameTime);                        
                     
                        //follow a patrol route, or create a new one if we're done with the current one (data stored in patrol data 
                    }
                }
              

                if ( CanFollow(ai))
                {
                    if (ai.AIData.IsAlly) //shortcut for AI followers
                    {                       
                        Follow(ai, _player, gameTime);
                    }
                    else
                    {
                        //get out follow target
                        var followEntity = LookupEntityById(ai.FollowData.FollowTarget);
                        Follow(ai, followEntity, gameTime);                        
                    }

                }
               
            }
        }

        private void Follow(AINode self, AINode target, GameTime gameTime)
        {
            if (self == target)
                return;

            var loc = target.PositionData.Position + self.FollowData.FollowDistance;
            self.FollowData.FollowTargetLocation = new RectangleF(loc.X, loc.Y, 2, 2);

            //have we reached our target location?
            if (!self.FollowData.FollowTargetLocation.Intersects(self.CollisionData.BoundingRectangle))
            {
                MoveTowardsTarget(self, self.FollowData.FollowTargetLocation, gameTime);

            }
        }

        private AINode LookupEntityById(int id)
        {
            foreach (var ai in _aiNodes)
            {
                if (ai.Id == id)
                    return ai;
            }
            return null;
        }

        private Vector2 CreateFollowVector(AINode self, int followTarget)
        {
            foreach (var ai in _aiNodes)
            {
                if (followTarget == ai.Id)
                {
                    Debug.Assert(ai.PositionData != null, "Entity being followed did not have a position component!");
                    return ai.PositionData.Position + self.FollowData.FollowDistance;
                }
            }

            //if we got here the target was missing
            return self.PositionData.Position; //follow self...
        }


     
        private void ReturnToPatrolStart(AINode ai, GameTime gameTime)
        {
            var pCenter = new RectangleF(ai.PatrolData.PatrolArea.Center, new Size2(2, 2));

            if (!ai.CollisionData.BoundingRectangle.Intersects(pCenter))
            {
                MoveTowardsTarget(ai, pCenter, gameTime);
                ai.PatrolData.IsReset = false;
            }
            else
            {
                //arrived
                ai.PatrolData.IsReset = true;
            }

        }




        private void MoveTowardsPlayer(AINode ai, GameTime gameTime)
        {
            Vector2 newPos = ai.PositionData.Position;
            if (_player.PositionData.Position.X > ai.PositionData.Position.X)
            {
                newPos.X +=  ai.PositionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (_player.PositionData.Position.X < ai.PositionData.Position.X)
            {
                newPos.X -= ai.PositionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (_player.PositionData.Position.Y > ai.PositionData.Position.Y)
            {
                newPos.Y += ai.PositionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (_player.PositionData.Position.Y < ai.PositionData.Position.Y)
            {
                newPos.Y -= ai.PositionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
            }

            ai.PositionData.Position = newPos;

        }

        private void MoveTowardsTarget(AINode ai, RectangleF target, GameTime gametime)
        {
            Vector2 newPos = ai.PositionData.Position;
            if (target.Position.X > ai.PositionData.Position.X)
            {
                //move player 
                newPos.X += ai.PositionData.Speed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (target.Position.X < ai.PositionData.Position.X)
            {
                //move player 
                newPos.X -= ai.PositionData.Speed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (target.Position.Y > ai.PositionData.Position.Y)
            {
                //move player 
                newPos.Y += ai.PositionData.Speed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (target.Position.Y < ai.PositionData.Position.Y)
            {
                //move player 
                newPos.Y -= ai.PositionData.Speed * gametime.ElapsedGameTime.Milliseconds;
            }

            ai.PositionData.Position = newPos;
        }

        public bool HasEntity(RogueEntity entity)
        {
            foreach (var node in _aiNodes)
            {
                if (node.Id == entity.ID)
                    return true;
            }
            return false;
        }
    }
}
