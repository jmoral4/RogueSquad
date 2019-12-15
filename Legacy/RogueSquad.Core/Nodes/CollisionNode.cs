using RogueSquad.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Nodes
{
    public class CollisionNode : INode
    {
        public int Id { get; set; }
        public int CollidedWith { get; set; }
        public bool Checked { get; set; }
        public bool HadCollision => CollisionData.HasCollision;
        public PositionComponent Position { get; set; }
        public CollidableComponent CollisionData { get; set; }


    }
}
