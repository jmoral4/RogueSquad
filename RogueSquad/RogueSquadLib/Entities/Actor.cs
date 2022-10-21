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

}
