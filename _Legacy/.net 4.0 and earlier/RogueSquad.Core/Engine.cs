using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RogueSquad.Core.Components;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core
{
    //http://csharpindepth.com/Articles/General/Singleton.aspx
    public sealed class Engine
    {

        private static readonly Lazy<Engine> lazy =
            new Lazy<Engine>(() => new Engine());

        public static Engine Instance { get { return lazy.Value; } }

        private Engine()
        {
        }

        private GraphicsDevice _device;
        private ContentManager _content;

        public void Init(GraphicsDevice device, ContentManager content) {
            _device = device;
            _content = content;
            _liveEntities = new BitArray(MAX_ENTITIES);            
        }

        const int MAX_ENTITIES = 20000;
        int _lastFreeEntity;
        BitArray _liveEntities;

        public string VersionString { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int DesiredFPS { get; set; }

        public int CreateUniqueEntityId()
        {
            return GetNextFreeEntity();
        }

        private int GetNextFreeEntity()
        {
            if (_lastFreeEntity >= MAX_ENTITIES  )
            {
                _lastFreeEntity = -1;
            }

            for (int i = 0; i < MAX_ENTITIES; i++)
            {
                if (!_liveEntities[i])
                {
                    _liveEntities[i] = true;
                    _lastFreeEntity = i;
                    break;
                }
            }

            if (_lastFreeEntity == -1)
                throw new Exception("Could not acquire a valid ID for the requested Entity!");

            return _lastFreeEntity;         
        }

        public Texture2D GetTexture(string fullNameAndPath)
        {
            return _content.Load<Texture2D>(fullNameAndPath);
        }

        private void RemoveEntity(int id)
        {
            _liveEntities[id] = false;
        }

    }


    public class EntityGenerator
    {
        private ContentManager _content;
        public EntityGenerator(ContentManager content)
        {
            _content = content;
        }
        public static RogueEntity CreateOnScreenEntity(string entityType, Vector2 position, float speed)
        {
            if (entityType == "basic_enemy" )
            {
                var texture = Engine.Instance.GetTexture("Textures/ro");
                RogueEntity enemy = RogueEntity.CreateNew();                
                enemy.AddComponent(new PositionComponent { Position = position, Speed = speed });
                enemy.AddComponent(new SpriteComponent { Texture = texture});
                enemy.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(position.X, position.Y, texture.Bounds.Width, texture.Bounds.Height) });
                return enemy;
            }

            return null;
        }
    }
  

}
