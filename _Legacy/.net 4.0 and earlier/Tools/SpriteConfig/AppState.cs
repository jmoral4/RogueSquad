namespace SpriteConfig
{
    public class AppState
    {
        public bool ParentSpriteLoaded { get; set; }
        public bool IsSaved { get; set; }
        public bool ShouldPrompt => IsSaved && ParentSpriteLoaded;
    }
}
