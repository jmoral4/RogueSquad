using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Components
{
    public class AIComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.AIComponent;

        public bool DetectedPlayer { get; set; }
        public bool IsHostile { get; set; }     
        public bool IsAttacking { get; set; }
        public RectangleF DetectionRadius { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsAlly { get; set; }
        public string Faction { get; set; }
        public bool IsSameFaction(string s)
        {
            return s == Faction;
        }
        public bool HasPatrolArea { get; set; }

        public RectangleF PatrolArea { get; set; }

        public RectangleF FollowFrame { get; set; }
        public RectangleF FollowTarget { get; set; }
        public Vector2 FollowDistance { get; set; }
        

    }
}
