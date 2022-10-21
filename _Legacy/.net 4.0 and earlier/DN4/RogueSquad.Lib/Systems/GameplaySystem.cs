using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RogueSquad.Core.Nodes;

namespace RogueSquad.Core.Systems
{
    public class GameplayNode
    {
        public int Id { get; set; }
        public PositionComponent PositionData { get; set; }
        public BasicControllerComponent ControllerData { get; set; }
        public AIComponent AIData { get; set; }
        public CollidableComponent CollisionData { get; set; }
        public AnimationStateInfoComponent PlayerStateData { get; set; }
        public bool IsPlayer { get; set; }
        
        
    }    

    public class GameplaySystem : IRogueSystem
    {
        public IEnumerable<ComponentTypes> Required { get; set; } = Enumerable.Empty<ComponentTypes>();

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
            var playerState = entity.GetComponentByType(ComponentTypes.AnimationStateInfoComponent) as AnimationStateInfoComponent;

            var gameplayNode = new GameplayNode { PositionData = position, ControllerData = controller, Id = entity.ID, AIData = ai, CollisionData = collisionData, IsPlayer = false , PlayerStateData = playerState };

            if (controller != null)
            {
                //this is a player node
                _player = gameplayNode;
                _player.IsPlayer = true;

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

        public void Update(GameTime gametime)
        {

            HandleInput(_player.ControllerData, _player.PositionData, _player.PlayerStateData, gametime);

            //check if player is in an AI trigger boundary
            foreach (var gamePlayNode in _nodes)
            {
                //run gameplay checks based on final AI state

            }                       

        }


        /// <summary>
        /// This method takes gameplay action on the controller commands which have been translated from their input devices (gamepad or keyboard)
        /// </summary>
        /// <param name="controller">Translated Controller Commands</param>
        /// <param name="positionData">Positional Data</param>
        /// <param name="gameTime">Elapsed Game Time</param>
        private void HandleInput(BasicControllerComponent controller, PositionComponent positionData, AnimationStateInfoComponent playerState, GameTime gameTime)
        {
            // this should be the central place where all input from the controller is handled as long as the game is running (not in a menu)
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
                    playerState.Facing = CharacterFacing.WEST;
                    playerState.IsMoving = true;
                    playerState.CurrentAnimationName = "left";
                    //also we update our players directional info
                }
                if (controller.KeyRight)
                {
                    newPos.X += positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
                    playerState.Facing = CharacterFacing.EAST;
                    playerState.IsMoving = true;
                    playerState.CurrentAnimationName = "right";
                }
                if (controller.KeyUp)
                {
                    newPos.Y -= positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
                    playerState.Facing = CharacterFacing.NORTH;
                    playerState.IsMoving = true;
                    playerState.CurrentAnimationName = "up";

                }
                if (controller.KeyDown)
                {
                    newPos.Y += positionData.Speed * gameTime.ElapsedGameTime.Milliseconds;
                    playerState.Facing = CharacterFacing.SOUTH;
                    playerState.IsMoving = true;
                    playerState.CurrentAnimationName = "down";
                }

                if (controller.KeyCreateRandomEntities)
                {
                    _worldRef.AddRandomEnemies(100, 100);
                }

                if (controller.KeyTarget)
                {
                    //let's select a target 

                }

                if (controller.KeyEnableDebug)
                {
                    if (_worldRef.IsDebugEnabled)
                        _worldRef.DisableDebugRendering();
                    else
                        _worldRef.EnableDebugRendering();
                }

                positionData.Position = newPos;
            }
            else
            {
                //select the correct idle direction
                string anim = "idle_down";
                switch (playerState.Facing)
                {
                    case CharacterFacing.EAST: anim="idle_right";break;
                    case CharacterFacing.WEST: anim = "idle_left"; break;
                    case CharacterFacing.SOUTH: anim = "idle_down"; break;
                    case CharacterFacing.NORTH: anim = "idle_up"; break;
                }
                playerState.CurrentAnimationName = anim;
                playerState.IsMoving = false;                
            }
        }
    }
}
