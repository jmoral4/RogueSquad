using Microsoft.Xna.Framework;

namespace RogueSquadLib.Entities
{
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

}
