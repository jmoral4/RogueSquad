using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquadLib.Entities
{
    // represents a basic thing in the world that can be acted upon in some way 
    internal class Actor
    {
        public string Name { get; set; }
        public string Faction { get; set; }
       
        // Query Variables used by systems for filtering quickly
        public bool IsCollideable { get; set; }
        public bool IsDrawable { get; set; }
        public bool CanMove { get; set; }

        public MovementComponent MovementInfo { get; set; }
        public DrawComponent DrawComponent { get; set; }

        public Actor()
        { 
            MovementInfo = new MovementComponent() { Id = Guid.NewGuid().ToString()};
            DrawComponent = new DrawComponent();

            IsCollideable = true;
            IsDrawable = true;
            CanMove = true;
        }

    }    

    class MovementComponent
    {
        // TODO: Change this to an integer to keep the movecomponent small for cache coherence, requires central 'repo' giving out ints. 
        public string Id { get; set; }
        public Vector2 Speed { get; set; }
        public Rectangle BoundingBox { get; set; }
        public void UpdateBoundingBox(Rectangle rect)
        {
            this.BoundingBox = rect;
        }
        public bool Collided { get; set; }
        public bool ResetCollision() => Collided = false;
    }

    class DrawComponent
    { 
        public Vector2 Scale { get; set; }
        public Color Tint { get; set; }
        public Texture2D Texture { get; set; }
        public int TextureId { get; set; }
        public Rectangle SrcRect { get; set; } // for rendering _from
    
    }

}
