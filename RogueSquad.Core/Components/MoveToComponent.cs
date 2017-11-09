using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Components
{
    public class MoveToComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.MoveToComponent;

        public Vector2 Destination { get; set; }

        public bool Arrived { get; set; }
        
    }
}
