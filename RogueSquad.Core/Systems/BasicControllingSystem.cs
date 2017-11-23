using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core.Components;
using RogueSquad.Core.Nodes;

namespace RogueSquad.Core.Systems
{
    // tanslate keyboard/controller input into game control commands (stored in the controller component/node)
    public class BasicControllingSystem : IRogueSystem
    {
        public ComponentTypes[] RequiredComponents { get; set; } = new ComponentTypes[] { ComponentTypes.BasicControllerComponent, ComponentTypes.PositionComponent };
        private IList<ControllerNode> _controllerNodes = new List<ControllerNode>();
        World _worldRef;
        public BasicControllingSystem(World world)
        {
            _worldRef = world;
        }

        public void AddEntity(RogueEntity entity)
        {           
            var controller = entity.GetComponentByType(ComponentTypes.BasicControllerComponent) as BasicControllerComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            _controllerNodes.Add(new ControllerNode { Position = position, Controller = controller, Id = entity.ID });            
        }

        public bool HasEntity(RogueEntity entity)
        {
            foreach (var node in _controllerNodes)
            {
                if (node.Id == entity.ID)
                    return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            foreach (var controlNode in _controllerNodes)
            {
                ProcessInput(kb, controlNode);                               
            }
        }

        public void ProcessInput(KeyboardState kb, ControllerNode controllerNode)
        {
            Reset(controllerNode);
            if (kb.IsKeyDown(Keys.W))
                controllerNode.Controller.KeyUp = true;
            if (kb.IsKeyDown(Keys.S))
                controllerNode.Controller.KeyDown = true;
            if (kb.IsKeyDown(Keys.A))
                controllerNode.Controller.KeyLeft = true;
            if (kb.IsKeyDown(Keys.D))
                controllerNode.Controller.KeyRight = true;
            if (kb.IsKeyDown(Keys.F1))
                controllerNode.Controller.KeyCreateRandomEntities = true;

            if (kb.IsKeyDown(Keys.Q))
                controllerNode.Controller.KeyTarget = true;

            if (kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.E))
                controllerNode.Controller.KeyFire = true;

            controllerNode.Controller.AnyKeyPressed = controllerNode.Controller.KeyUp || controllerNode.Controller.KeyDown || controllerNode.Controller.KeyLeft || controllerNode.Controller.KeyRight || controllerNode.Controller.KeyRetreat || controllerNode.Controller.KeyCreateRandomEntities || controllerNode.Controller.KeyFire || controllerNode.Controller.KeyTarget;
        }

        private void Reset(ControllerNode controllerNode)
        {
            controllerNode.Controller.KeyUp = false;
            controllerNode.Controller.KeyDown = false;
            controllerNode.Controller.KeyLeft = false;
            controllerNode.Controller.KeyRight = false;
            controllerNode.Controller.KeyRetreat = false;
            controllerNode.Controller.KeyTarget = false;
            controllerNode.Controller.KeyFire = false;
            controllerNode.Controller.KeyCreateRandomEntities = false;
            controllerNode.Controller.AnyKeyPressed = false;
        }

    }

  

}
