using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Components
{
    public class AIComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; }

        public bool MoveToRequested { get; set; }
    }
}
