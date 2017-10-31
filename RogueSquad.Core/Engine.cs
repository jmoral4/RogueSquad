using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

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

        private GraphicsDeviceManager _device;
        private ContentManager _content;

        public void Init(GraphicsDeviceManager device, ContentManager content) {
            _device = device;
            _content = content;
        }

    }


    public sealed class World
    {
        int _lastEntityCounter = 0;
        const int MAX_ENTITIES = 20000;
        public List<IRogueSystem> Systems { get; set; }
        public List<IRogueRenderSystem> RenderSystems { get; set; }
        BitArray KnownEntities;
        public List<RogueEntity> Entities { get; set; }

        public World()
        {
            KnownEntities = new BitArray(MAX_ENTITIES);
            Systems = new List<IRogueSystem>();
            RenderSystems = new List<IRogueRenderSystem>();
            Entities = new List<RogueEntity>();
        }

        public int EntityCount => Entities.Count;

        public void RegisterSystem(IRogueSystem system)
        {
            Systems.Add(system);
        }

        public void RegisterRenderSystem(IRogueRenderSystem system)
        {
            RenderSystems.Add(system);
        }

        public RogueEntity CreateEntity()
        {
            int freeId = -1;
            for (int i = 0; i < KnownEntities.Length; i++)
            {
                if (!KnownEntities[i])
                {
                    freeId = i;
                    break;
                }
            }

            if (freeId != -1)
            {
                var entity = new RogueEntity { ID = freeId };
                Entities.Add(entity);
                return entity;
            }
            else
                throw new Exception("No Ids available for creation of new entities!");            
        }

        public void DestroyEntity(int id)
        {
            KnownEntities[id] = false;            
        }

        public void Update(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var renderSystem in RenderSystems)
            {
                renderSystem.Draw(gameTime, Entities);
            }
        }

    }

    public enum SystemTypes {
            BasicControllerSystem, MovementSystem, CameraSystem
    }

    public enum ComponentTypes {
        Controller, Position, Movement, Render
    }

    public enum RenderSystemTypes {
        Render2DSystem
    }

    public interface IRogueSystem
    {
        void Update(GameTime gametime, IList<RogueEntity> entities);

    }

    public interface IRogueRenderSystem
    {
        void Draw(GameTime gameTime, IList<RogueEntity> entities);
        
    }

    public interface IRogueComponent
    {
        ComponentTypes ComponentType { get; set; }
    }

    public class Render2DSystem : IRogueRenderSystem
    {
        SpriteBatch batchRef;
        public Render2DSystem(SpriteBatch batch)
        {
            batchRef = batch;
        }

        public void Draw(GameTime gameTime, IList<RogueEntity> entities)
        {
            batchRef.Begin();
            foreach (var entity in entities)
            {
                if (entity.HasComponent(ComponentTypes.Render))
                {
                    //get position
                    var position = entity.GetComponentsByType(ComponentTypes.Position).First() as PositionComponent;
                    var render = entity.GetComponentsByType(ComponentTypes.Render).First() as RenderableComponent;
                    batchRef.Draw(render.Texture, position.Position, Color.White);                    
                }
            }
            batchRef.End();
        }
    }


    public class BasicControllingSystem : IRogueSystem
    {       

        public void Update(GameTime gameTime, IList<RogueEntity> entities )
        {
            var kb = Keyboard.GetState();
            foreach (var entity in entities)
            {
                if (entity.HasComponent(ComponentTypes.Controller))
                {                    
                    var controller = entity.GetComponentsByType(ComponentTypes.Controller).First() as BasicControllerComponent;
                    controller.ProcessInput(kb);

                    var position = entity.GetComponentsByType(ComponentTypes.Position).First() as PositionComponent;
                    //position.Position = new Vector2(0, 0);
                    if (controller.KeyRetreat)
                    {
                        //var position = entity.GetComponentsByType(ComponentTypes.Position).First() as PositionComponent;
                        position.Position = new Vector2(0, 0);
                    }

                    if (controller.KeyLeft)
                    {
                        position.Position = new Vector2(position.Position.X - 1, position.Position.Y);
                    }
                    if (controller.KeyRight)
                    {
                        position.Position = new Vector2(position.Position.X + 1, position.Position.Y);
                    }
                    if (controller.KeyUp)
                    {
                        position.Position = new Vector2(position.Position.X , position.Position.Y -1);
                    }
                    if (controller.KeyDown)
                    {
                        position.Position = new Vector2(position.Position.X , position.Position.Y+1);
                    }

                    //do something
                }
            }

        }
    }

    public class RandomMovementSystem : IRogueSystem {
        public void Update(GameTime gameTime, IList<RogueEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent(ComponentTypes.Movement))
                {
                    var position = entity.GetComponentsByType(ComponentTypes.Position).First() as PositionComponent;
                    
                    //do something
                }
            }

        }

    }



    public class SpriteComponent
    {
        public string Texture { get; set; }
        public SpriteComponent(string textureName)
        {
            Texture = textureName;
        }
    }

    public class BasicControllerComponent : IRogueComponent
    {

        public BasicControllerComponent()
        {

        }

        public void ProcessInput(KeyboardState kb)
        {
            Reset();
            if (kb.IsKeyDown(Keys.W))
                KeyUp = true;
            if (kb.IsKeyDown(Keys.S))
                KeyDown = true;
            if (kb.IsKeyDown(Keys.A))
                KeyLeft = true;
            if (kb.IsKeyDown(Keys.D))
                KeyRight = true;
        }

        private void Reset()
        {
            KeyUp = false;
           
            KeyDown = false;
          
            KeyLeft = false;
           
            KeyRight = false;
        }

        public bool KeyRetreat { get; set; }
        public bool KeyLeft { get; set; }
        public bool KeyRight { get; set; }
        public bool KeyUp{ get; set; }
        public bool KeyDown { get; set; }
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.Controller;
    }

    public class PositionComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.Position;
        public Vector2 Position { get; set; } = Vector2.Zero;
    }
    public class MovementComponent
    {
        public double Speed { get; set; } = 0.0;
    }
    


    public class RogueEntity {
        public int ID { get; set; }               
        List<IRogueComponent> _components;

        public RogueEntity()
        {
            _components = new List<IRogueComponent>();
        }

        public void AddComponent(IRogueComponent component)
        {
            _components.Add(component);
        }

        public bool HasComponent(ComponentTypes type)
        {
            foreach (var component in _components)
            {
                if (component.ComponentType == type)
                    return true;
            }
            return false;
        }
     

        public IEnumerable<IRogueComponent> GetComponentsByType(ComponentTypes type)
        {
            foreach (var component in _components)
                if (component.ComponentType == type)
                    yield return component;
        }


        //public Vector2 Position { get; set; }
        ////ublic List<RogueEntity>

        //InputComponent _input;
        //RenderableComponent _render;
        //PhysicsComponent _physics;

        //void Update()

        //public RogueEntity(InputComponent input, PhysicsComponent physics, RenderableComponent render)
        //{
        //    _input = input;
        //    _physics = physics;
        //    _render = render;
        //}
    }

    public class InputComponent
    { }

    public class IComponent {

    }


    public class MoveableComponent {
        public Vector2 Position { get; set; }
        public Rectangle BoundingRectangle { get; set; }        
    }

    public class PhysicsComponent {
        public Rectangle BoundingRectangle { get; set; }        
    }

    public class RenderableComponent : IRogueComponent {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.Render;
        public Texture2D Texture { get; set; }

    }

  

}
