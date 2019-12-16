using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace RogueSquad.Core.Components
{
    public class CollidableComponent : IRogueComponent
    {
        public RectangleF BoundingRectangle { get; set; }
        public bool HasCollision { get; set; }    
        public bool IsStatic { get; set; }
        public void Collided()
        {
            this.HasCollision = true;
        }
        public void ResetCollision()
        {
            this.HasCollision = false;
        }
        public void UpdateBoundingRectangle(RectangleF rect)
        {
            this.BoundingRectangle = rect;
        }
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.CollidableComponent;
    }

}
