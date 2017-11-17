using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Systems;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;

namespace RogueSquad.Core
{


    public sealed class World
    {      
        public List<IRogueSystem> Systems { get; set; }
        public List<IRogueRenderSystem> RenderSystems { get; set; }
        public List<RogueEntity> Entities { get; set; }


        GraphicsDevice _graphicsDeviceRef;
        SpriteBatch _renderBatcher;
        public ContentManager Content { get; set; }
        int w = 1920;
        int h = 1080;

        public World(GraphicsDevice graphicsDeviceRef, ContentManager contentManager, int resolutionW, int resolutionH)
        {
            w = resolutionW;
            h = resolutionH;
            Content = contentManager;
            _graphicsDeviceRef = graphicsDeviceRef;
            _renderBatcher = new SpriteBatch(graphicsDeviceRef);
            Systems = new List<IRogueSystem>();
            RenderSystems = new List<IRogueRenderSystem>();
            Entities = new List<RogueEntity>();

        }

        public int EntityCount => Entities.Count;

        public void RegisterSystem(IRogueSystem system)
        {
            Systems.Add(system);
        }

        public void SetScene(string sceneName)
        {
            if (sceneName == "Testbed")
            {                
                CreateDemoScene();
            }
        }


        private void CreateDemoScene()
        {
            //setup player
            RogueEntity player = RogueEntity.CreateNew();
            Vector2 startLocation = new Vector2(100, 100);
            var playerTex = Content.Load<Texture2D>("Assets/robit");
            player.AddComponent(new PositionComponent { Position = startLocation });
            player.AddComponent(new SpriteComponent { Texture = playerTex});
            player.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(startLocation.X, startLocation.Y, playerTex.Bounds.Width,playerTex.Bounds.Height) });
            player.AddComponent(new BasicControllerComponent());

            AddEntity(player);

            AddRandomEnemies();

           
        }

        

        public void AddRandomEnemies()
        {
            EntityGenerator eg = new EntityGenerator(Content);
            Random r = new Random();
           
            for (int i = 0; i < 100; i++)
            {
               
                var pos = new Vector2(r.Next(0, w - 20), r.Next(0, h - 20));
                var enemy = eg.CreateOnScreenEntity("basic_enemy", pos);             
                this.AddEntity(enemy);
            }
        }

        
        public void EnableBasicSystems()
        {
            RegisterSystem(new GameplaySystem(this));
            RegisterSystem(new BasicControllingSystem(this));
            RegisterSystem(new CollisionSystem());
            RegisterRenderSystem(new RenderingSystem(_renderBatcher));            
        }

        public void EnableDebugRendering()
        {
            RegisterRenderSystem(new DebugRenderSystem(_renderBatcher));
        }

        public void RegisterRenderSystem(IRogueRenderSystem system)
        {
            RenderSystems.Add(system);
        }

        public void AddEntity(RogueEntity entity)
        {

            AddEntityToSystems(entity);
            AddEntityToRenderSystems(entity);

            // add to world tracking as well - probably not needed in the future
            Entities.Add(entity);
        }

        public void RemoveEntity(RogueEntity entity)
        {
            // remove from each system

        }


        private void AddEntityToSystems(RogueEntity entity)
        {
            foreach (var sys in Systems)
            {
                //get matching components and add to system
                var hasTypes = sys.RequiredComponents.ToDictionary(x => x, y => false);

                foreach (var compType in sys.RequiredComponents)
                {
                    hasTypes[compType] = entity.HasComponent(compType);
                }

                if (hasTypes.All(x => x.Value))
                    sys.AddEntity(entity);
            }
            
        }

        private void AddEntityToRenderSystems(RogueEntity entity)
        {
            foreach (var sys in RenderSystems)
            {
                var hasTypes = sys.RequiredComponents.ToDictionary(x => x, y => false);
                foreach (var compType in sys.RequiredComponents)
                {
                    hasTypes[compType] = entity.HasComponent(compType);
                }

                if (hasTypes.All(x => x.Value))
                    sys.AddEntity(entity);
            }
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

}