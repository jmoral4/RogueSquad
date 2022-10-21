
using RogueSquad.Core.Components;

namespace RogueSquad.Core.Nodes
{

    public class RenderNode : INode
    {
        public int Id { get; set; }
        public SpriteComponent StaticSpriteData { get; set; }
        public PositionComponent Position { get; set; }

        public AnimatedSpriteComponent AnimatedSpriteData { get; set; }
        public AnimationStateInfoComponent AnimationState { get; set; }
    }

}