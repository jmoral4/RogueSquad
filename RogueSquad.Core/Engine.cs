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
            _liveEntities = new BitArray(MAX_ENTITIES);            
        }

        const int MAX_ENTITIES = 20000;
        int _lastFreeEntity;
        BitArray _liveEntities;

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
                    _lastFreeEntity = i;
                    break;
                }
            }

            if (_lastFreeEntity == -1)
                throw new Exception("Could not acquire a valid ID for the requested Entity!");

            return _lastFreeEntity;         
        }

        private void RemoveEntity(int id)
        {
            _liveEntities[id] = false;
        }

    }


    public sealed class World
    {      
        public List<IRogueSystem> Systems { get; set; }
        public List<IRogueRenderSystem> RenderSystems { get; set; }
        public List<RogueEntity> Entities { get; set; }

        public World()
        {
           
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

        public void AddEntity(RogueEntity entity)
        {
            
            foreach (var sys in Systems)
            {
                //get matching components and add to system
                var hasTypes = sys.DesiredComponentsTypes.ToDictionary(x => x, y => false);
                
                foreach (var compType in sys.DesiredComponentsTypes)
                {
                    hasTypes[compType] = entity.HasComponent(compType);                                                            
                }

                if (hasTypes.All(x => true))
                    sys.AddEntity(entity);

                //if it has renderable parts, add to renderables                
            }

            if (entity.HasComponent(ComponentTypes.Render) && entity.HasComponent(ComponentTypes.Position))
            {
                foreach (var renderSys in RenderSystems)
                    renderSys.AddEntity(entity);
            }

            // add to world tracking as well
            Entities.Add(entity);
        }

        public void UpdateEntity(RogueEntity entity)
        {
            //update entity's system subscriptions
        }


        public void Update(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var renderSystem in RenderSystems)
            {
                renderSystem.Draw(gameTime);
            }
        }

    }


    public enum ComponentTypes {
        Controller, Position, Movement, Render
    }


    public interface IRogueSystem
    {
        void Update(GameTime gametime);
        void AddEntity(RogueEntity entity);
        ComponentTypes[] DesiredComponentsTypes { get; set; }
    }

    public interface IRogueRenderSystem
    {
        void Draw(GameTime gameTime);
        void AddEntity(RogueEntity entity);

    }

    public interface IRogueComponent
    {
        ComponentTypes ComponentType { get; set; }
    }

    public class RenderNode
    {
        public int Id { get; set; }
        public RenderableComponent Renderalble { get; set; }
        public PositionComponent Position { get; set; }
    }

    public class Render2DSystem : IRogueRenderSystem
    {
        SpriteBatch batchRef;
        public IList<RenderNode> _renderNodes = new List<RenderNode>();
        public Render2DSystem(SpriteBatch batch)
        {
            batchRef = batch;
        }

        public void AddEntity(RogueEntity entity)
        {
            var render = entity.GetComponentByType(ComponentTypes.Render) as RenderableComponent;
            var position = entity.GetComponentByType(ComponentTypes.Position) as PositionComponent;
            _renderNodes.Add(new RenderNode { Position = position, Renderalble =  render, Id = entity.ID });
        }

        public void Draw(GameTime gameTime)
        {
            batchRef.Begin();
            foreach (var entity in _renderNodes)
            {
                batchRef.Draw(entity.Renderalble.Texture, entity.Position.Position, Color.White);                
            }
            batchRef.End();
        }
    }

    public class ControllerNode
    {
        public int Id { get; set; }
        public PositionComponent Position { get; set; }
        public BasicControllerComponent Controller { get; set; }
    }

    public class BasicControllingSystem : IRogueSystem
    {
        public ComponentTypes[] DesiredComponentsTypes { get; set; } = new ComponentTypes[] { ComponentTypes.Controller, ComponentTypes.Position };
        private IList<ControllerNode> _controllerNodes = new List<ControllerNode>();
        public BasicControllingSystem()
        {         
        }

        public void AddEntity(RogueEntity entity)
        {           
            var controller = entity.GetComponentByType(ComponentTypes.Controller) as BasicControllerComponent;
            var position = entity.GetComponentByType(ComponentTypes.Position) as PositionComponent;
            _controllerNodes.Add(new ControllerNode { Position = position, Controller = controller, Id = entity.ID });            
        }

        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            foreach (var controlNode in _controllerNodes)
            {
                controlNode.Controller.ProcessInput(kb);
                if (controlNode.Controller.AnyKeyPressed)
                {
                    Vector2 newPos = controlNode.Position.Position;                    

                    if (controlNode.Controller.KeyRetreat)
                    {
                        controlNode.Position.Position = new Vector2(0, 0);
                    }

                    if (controlNode.Controller.KeyLeft)
                    {
                        newPos.X -= 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X - 1, controlNode.Position.Position.Y);                        
                    }
                    if (controlNode.Controller.KeyRight)
                    {
                        newPos.X += 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X + 1, controlNode.Position.Position.Y);
                    }
                    if (controlNode.Controller.KeyUp)
                    {
                        newPos.Y -= 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X, controlNode.Position.Position.Y - 1);
                    }
                    if (controlNode.Controller.KeyDown)
                    {
                        newPos.Y += 1;
                        //controlNode.Position.Position = new Vector2(controlNode.Position.Position.X, controlNode.Position.Position.Y + 1);
                    }
                    controlNode.Position.Position = newPos;
                }
            }
        }
    }

    public class RandomMovementSystem : IRogueSystem {
        public ComponentTypes[] DesiredComponentsTypes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddEntity(RogueEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
           

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

            AnyKeyPressed = KeyUp || KeyDown || KeyLeft || KeyRight || KeyRetreat;
        }

        private void Reset()
        {
            KeyUp = false;           
            KeyDown = false;          
            KeyLeft = false;           
            KeyRight = false;
            KeyRetreat = false;
            AnyKeyPressed = false;
        }

        public bool KeyRetreat { get; set; }
        public bool KeyLeft { get; set; }
        public bool KeyRight { get; set; }
        public bool KeyUp{ get; set; }
        public bool KeyDown { get; set; }
        public bool AnyKeyPressed { get; set; }
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.Controller;
    }

    public class PositionComponent : IRogueComponent
    {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.Position;
        public Vector2 Position { get; set; } = Vector2.Zero;
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

        public IEnumerable<IRogueComponent> GetComponents()
        {
            return _components;
        }

        public IEnumerable<IRogueComponent> GetComponentsByType(ComponentTypes type)
        {
            foreach (var component in _components)
                if (component.ComponentType == type)
                    yield return component;
        }


        public IRogueComponent GetComponentByType(ComponentTypes type)
        {
            foreach (var component in _components)
                if (component.ComponentType == type)
                    return component;
            return null;
        }

        public static RogueEntity CreateNew()
        {
            return new RogueEntity { ID = Engine.Instance.CreateUniqueEntityId() };
        }
       
    }


    public class RenderableComponent : IRogueComponent {
        public ComponentTypes ComponentType { get; set; } = ComponentTypes.Render;
        public Texture2D Texture { get; set; }

    }

  

}
