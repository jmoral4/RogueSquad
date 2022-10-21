using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;

namespace RogueSquad.Core.Components
{
    public class SpriteComponent : IRogueComponent {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.SpriteComponent;
        public Texture2D Texture { get; set; }
        public Point Size { get; set; }

        public Rectangle Source { get; set; }

    }

    public class AnimatedSpriteComponent : IRogueComponent {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.SpriteComponent;        
        public AnimatedSprite AnimatedSprite { get; set; }            
        public string CurrentAnimation { get; set; }        
        public bool IsPlaying { get; set; }        
        public AnimatedSpriteComponent(SpriteSheetAnimationFactory spriteData)
        {
            AnimatedSprite = new AnimatedSprite(spriteData);
        }

    }


    public class ToggleAnimatedSprite : AnimatedSprite
    {
        public ToggleAnimatedSprite(SpriteSheetAnimationFactory spriteData) : base(spriteData)
        {
        }
        
    }

}
