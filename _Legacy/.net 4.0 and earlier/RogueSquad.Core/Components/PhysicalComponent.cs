using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Components
{
    public class PhysicalComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.PhysicalComponent;

        public Vector2 Position { get; set; }
        public RectangleF BoundingRectangle { get; set; }
        public Vector2 Movement { get; set; }
        public bool IsStatic { get; set; }
        public bool HadCollisionLastUpdate { get; set; }

    }
}
