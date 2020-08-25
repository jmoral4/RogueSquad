using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RogueSquad.Core.Systems;
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

        public bool IsStatic { get; set; }
        public bool DetectedPlayer { get; set; }        
        public bool IsHostile { get; set; }     
        public bool IsAttacking { get; set; }
        public RectangleF DetectionArea { get; set; }
        public Vector2 DetectionRange { get; set; }
        public bool IsPlayerControlled { get; set; }        
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


    public class FollowComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.FollowComponent;        
        public Vector2 FollowDistance { get; set; }        
        public int FollowTarget { get; set; }
        public RectangleF FollowTargetLocation { get; set; }
        public bool ReachedTargetDistance { get; set; }
        public bool AlwaysFollow { get; set; } // if badguys are following each other or whatever, do they break formation?
        
        
    }


    public class PatrolComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.PatrolComponent;
        public BoundingRectangle PatrolArea { get; set; }
        public List<string> FriendlyFactions { get; set; } = new List<string>();
        public bool IsReset { get; set; }

    }


}
