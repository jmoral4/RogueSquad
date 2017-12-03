using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Components
{
    public enum CharacterFacing { NORTH, SOUTH, EAST, WEST}
    public class AnimationStateInfoComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.AnimationStateInfoComponent;

        public CharacterFacing Facing { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsMoving { get; set; }
        public string CurrentAnimationName { get; set; }

    }
}
