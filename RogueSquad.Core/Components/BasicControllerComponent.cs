using Microsoft.Xna.Framework.Input;
using System;

namespace RogueSquad.Core.Components
{
    public class KeyMapping
    {
        public Keys Key { get; set; }
        public Action OnKeyPress { get; set; }
    }

    public class BasicControllerComponent : IRogueComponent
    {
        KeyMapping[] _keyMapping;
        
        public BasicControllerComponent(KeyMapping[] keymapping = null)
        {
            _keyMapping = keymapping;
        }



        public bool KeyRetreat { get; set; }
        public bool KeyFire { get; set; }
        public bool KeyLeft { get; set; }
        public bool KeyRight { get; set; }
        public bool KeyUp{ get; set; }
        public bool KeyDown { get; set; }
        public bool AnyKeyPressed { get; set; }
        public bool KeyCreateRandomEntities { get; set; }
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.BasicControllerComponent;
    }

  

}
