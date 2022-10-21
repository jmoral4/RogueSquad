using RogueSquad.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Editors
{

    public class EditableNpc
    {
        public string Name { get; set; }
        public string Faction { get; set; }
        public int HitPoints { get; set; }
        public int Speed { get; set; }
        public AIComponent AIData { get; set; }
        public SpriteComponent SpriteData { get; set; }
        public bool IsRecruitable { get; set; }
        public bool IsVital { get; set; } //can't die - always reduced to 1hp
        public CollidableComponent CollisionData { get; set; }
        public string PreferredMap { get; set; }
        public bool IsSaved { get; set; }

    }


    public class NpcEditor
    {       
        //select texture
        //Assign name
        //Assign Properties
        //Assign Default AI behavior(can be overriden by the game)
        //Assign 'preferred map' (used by map editor when generating random maps)
    }
}
