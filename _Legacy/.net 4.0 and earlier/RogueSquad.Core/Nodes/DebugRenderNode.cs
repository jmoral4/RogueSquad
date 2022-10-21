using RogueSquad.Core.Nodes;
using RogueSquad.Core.Components;

namespace RogueSquad.Core.Nodes
{
    public class DebugRenderNode : INode
    {
        public int Id { get; set; }
        public SpriteComponent Renderable { get; set; }
        public PositionComponent Position { get; set; }
        public CollidableComponent Collision { get; set; }
        public AIComponent AIData { get; set; }

        public PatrolComponent PatrolData { get; set; }
        public FollowComponent FollowData { get; set; }
    }

}
