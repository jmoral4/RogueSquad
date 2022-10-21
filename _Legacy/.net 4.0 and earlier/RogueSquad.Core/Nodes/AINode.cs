using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace RogueSquad.Core.Nodes
{

    public class AINode : INode {
        public int Id { get; set; }
        public AIComponent AIData { get; set; }
        public PositionComponent PositionData { get; set; }
        public CollidableComponent CollisionData { get; set; }
        public FollowComponent FollowData { get; set; }
        public PatrolComponent PatrolData { get; set; }
    }



}
