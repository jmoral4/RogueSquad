using Microsoft.Xna.Framework.Input;

namespace RogueSquad.Core.Components
{
    public class BasicControllerComponent : IRogueComponent
    {

        public BasicControllerComponent()
        {

        }

        public void ProcessInput(KeyboardState kb)
        {
            Reset();
            if (kb.IsKeyDown(Keys.W))
                KeyUp = true;
            if (kb.IsKeyDown(Keys.S))
                KeyDown = true;
            if (kb.IsKeyDown(Keys.A))
                KeyLeft = true;
            if (kb.IsKeyDown(Keys.D))
                KeyRight = true;

            AnyKeyPressed = KeyUp || KeyDown || KeyLeft || KeyRight || KeyRetreat;
        }

        private void Reset()
        {
            KeyUp = false;           
            KeyDown = false;          
            KeyLeft = false;           
            KeyRight = false;
            KeyRetreat = false;
            AnyKeyPressed = false;
        }

        public bool KeyRetreat { get; set; }
        public bool KeyLeft { get; set; }
        public bool KeyRight { get; set; }
        public bool KeyUp{ get; set; }
        public bool KeyDown { get; set; }
        public bool AnyKeyPressed { get; set; }
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.BasicControllerComponent;
    }

  

}
