
using RogueSquad.Core.Components;

namespace RogueSquad.Core.Nodes
{

    public class RenderNode : INode
    {
        public int Id { get; set; }
        public RenderableComponent Renderalble { get; set; }
        public PositionComponent Position { get; set; }
    }

}