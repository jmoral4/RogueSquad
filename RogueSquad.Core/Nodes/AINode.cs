using RogueSquad.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Nodes
{
    class AINode : INode
    {
        public int Id { get; set; }
        public AIComponent AIData { get; set; }
        public PositionComponent Position { get; set; }

    }
}
