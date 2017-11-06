using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core.Components;
using RogueSquad.Core.Nodes;

namespace RogueSquad.Core.Systems
{
    public class BasicControllingSystem : IRogueSystem
    {
        public ComponentTypes[] DesiredComponentsTypes { get; set; } = new ComponentTypes[] { ComponentTypes.BasicControllerComponent, ComponentTypes.PositionComponent };
        private IList<ControllerNode> _controllerNodes = new List<ControllerNode>();
        public BasicControllingSystem()
        {         
        }

        public void AddEntity(RogueEntity entity)
        {           
            var controller = entity.GetComponentByType(ComponentTypes.BasicControllerComponent) as BasicControllerComponent;
            var position = entity.GetComponentByType(ComponentTypes.PositionComponent) as PositionComponent;
            _controllerNodes.Add(new ControllerNode { Position = position, Controller = controller, Id = entity.ID });            
        }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            foreach (var controlNode in _controllerNodes)
            {
                controlNode.Controller.ProcessInput(kb);
                if (controlNode.Controller.AnyKeyPressed)
                {
                    Vector2 newPos = controlNode.Position.Position;                    

                    if (controlNode.Controller.KeyRetreat)
                    {
                        controlNode.Position.Position = new Vector2(0, 0);
                    }

                    if (controlNode.Controller.KeyLeft)
                    {
                        newPos.X -= 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X - 1, controlNode.Position.Position.Y);                        
                    }
                    if (controlNode.Controller.KeyRight)
                    {
                        newPos.X += 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X + 1, controlNode.Position.Position.Y);
                    }
                    if (controlNode.Controller.KeyUp)
                    {
                        newPos.Y -= 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X, controlNode.Position.Position.Y - 1);
                    }
                    if (controlNode.Controller.KeyDown)
                    {
                        newPos.Y += 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X, controlNode.Position.Position.Y + 1);
                    }
                    controlNode.Position.Position = newPos;
                }
            }
        }
    }

  

}
