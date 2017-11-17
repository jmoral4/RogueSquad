using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Input;

namespace RogueSquad.Core.Systems
{
    public class GameplayNode
    {
        public int Id { get; set; }
        public PositionComponent PositionData { get; set; }
        public BasicControllerComponent ControllerData { get; set; }
        
    }
    public class GameplaySystem : IRogueSystem
    {
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.BasicControllerComponent, ComponentTypes.PositionComponent };

        List<GameplayNode> _nodes = new List<GameplayNode>();
        World _worldRef;

        public GameplaySystem(World world)
        {
            _worldRef = world;
        }

        public void AddEntity(RogueEntity entity)
        {
            var controller = entity.GetComponentByType(ComponentTypes.BasicControllerComponent) as BasicControllerComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            _nodes.Add(new GameplayNode { PositionData = position, ControllerData = controller, Id = entity.ID });
        }

        public void Update(GameTime gametime)
        {
            foreach (var gamePlayNode in _nodes)
            {
                HandleInput(gamePlayNode.ControllerData, gamePlayNode.PositionData);
            }
        }

        private void HandleInput(BasicControllerComponent controller, PositionComponent positionData)
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
                    newPos.X -= 1;

                }
                if (controller.KeyRight)
                {
                    newPos.X += 1;

                }
                if (controller.KeyUp)
                {
                    newPos.Y -= 1;

                }
                if (controller.KeyDown)
                {
                    newPos.Y += 1;
                }

                if (controller.KeyCreateRandomEntities)
                {
                    _worldRef.AddRandomEnemies();
                }

                positionData.Position = newPos;
            }
        }
    }
}
