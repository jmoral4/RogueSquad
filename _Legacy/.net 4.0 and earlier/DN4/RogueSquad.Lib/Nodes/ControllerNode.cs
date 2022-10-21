using RogueSquad.Core.Components;

namespace RogueSquad.Core.Nodes
{
    public class ControllerNode : INode
    {
        public int Id { get; set; }
        public PositionComponent Position { get; set; }
        public BasicControllerComponent Controller { get; set; }
    }

  

}
