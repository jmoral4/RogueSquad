using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Systems;
using RogueSquad.Core.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

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
        Camera2D _camera;
        ViewportAdapter _viewPort;

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
                CreateDemoScene(100,100);
            }
        }

        public void AttachCamera(Camera2D camera, ViewportAdapter viewport)
        {
            _camera = camera;
            _viewPort = viewport;
        }


        private void CreateDemoScene(int worldWidth, int worldHeight)
        {
            //setup player
            RogueEntity player = RogueEntity.CreateNew();
            Vector2 startLocation = new Vector2(1000, 500);
            var playerTex = Content.Load<Texture2D>("Assets/robit");
            player.AddComponent(new PositionComponent { Position = startLocation, Speed = .25f });
            player.AddComponent(new SpriteComponent { Texture = playerTex, Size = new Point(64, 96) });
            player.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(startLocation.X, startLocation.Y, 64, 96) });
            player.AddComponent(new BasicControllerComponent());
            player.AddComponent(new AIComponent { IsPlayer = true });

            AddEntity(player);

            AddRandomEnemies(worldWidth, worldHeight);

           
        }



        public void AddRandomEnemies(int worldWidth, int worldHeight)
        {
            EntityGenerator eg = new EntityGenerator(Content);
            Random r = new Random();
           
            for (int i = 0; i < 20; i++)
            {
                var pos = new Vector2(r.Next(0, (64 * worldWidth) - 20), r.Next(0, (worldHeight * 32) - 20));
                var enemy = eg.CreateOnScreenEntity("basic_enemy", pos, r.Next(1,10) / 100);
                CollidableComponent collider = enemy.GetComponentByType(ComponentTypes.CollidableComponent) as CollidableComponent;
                //add some AI
                var inf = r.Next(50, 200);
                AIComponent ai = new AIComponent
                {
                    DetectedPlayer = false,
                    IsAttacking = false,                   
                    IsHostile = r.Next(0, 100) > 70,
                    IsPlayer = false,
                    
                };
                var rect = collider.BoundingRectangle.ToRectangle();
                rect.Inflate(inf, inf);
                ai.DetectionRadius = rect;

                ai.HasPatrolArea = r.Next(0, 100) > 40;
                if (ai.HasPatrolArea)
                {
                    var patrolRect = ai.DetectionRadius.ToRectangle();
                    patrolRect.Inflate(inf/2, inf/2);
                    ai.PatrolArea = patrolRect;
                }
                enemy.AddComponent(ai);
                this.AddEntity(enemy);
            }
        }

        
        public void EnableBasicSystems()
        {
            RegisterSystem(new GameplaySystem(this));
            RegisterSystem(new BasicControllingSystem(this));
            RegisterSystem(new CollisionSystem());
            RegisterRenderSystem(CreateMapRenderingSystem(100,100));
            RegisterRenderSystem(new RenderingSystem(_renderBatcher, _camera, _viewPort));
            
        }

        private IRogueRenderSystem CreateMapRenderingSystem(int mapWidth, int mapHeight)
        {

            var mrs = new MapRenderingSystem(
                    new List<Tile> {
                        new Tile
                        {
                            Name= "Sand1",
                            Texture = Content.Load<Texture2D>("Assets/sandtile1"),
                            IsWalkable = true,
                            TileType = MapTileTypes.Ground
                        },
                                                new Tile
                        {
                            Name= "Sand2",
                            Texture = Content.Load<Texture2D>("Assets/sandtile2"),
                            IsWalkable = true,
                            TileType = MapTileTypes.Ground
                        },
                                                                        new Tile
                        {
                            Name= "Sand3",
                            Texture = Content.Load<Texture2D>("Assets/sandtile3"),
                            IsWalkable = true,
                            TileType = MapTileTypes.Ground
                        },
                                                                                                new Tile
                        {
                            Name= "Sand4",
                            Texture = Content.Load<Texture2D>("Assets/sandtile4"),
                            IsWalkable = true,
                            TileType = MapTileTypes.Ground
                        }

                    },
                    new Point(64, 32),
                    Content.Load<SpriteFont>("Fonts/gamefont"),
                    _renderBatcher,
                    _camera,
                    _viewPort
                );

            mrs.CreateMap(mapWidth, mapHeight);

            return mrs; 

        }

        bool _debugEnabled = false;

        public void EnableDebugRendering()
        {
            if (_debugEnabled)
                return;
            var dbg = new DebugRenderSystem(_renderBatcher, _camera, _viewPort);
            RegisterRenderSystem(dbg);
            AddEntitiesToNewSystem(dbg);
            _debugEnabled = true;
        }
        public void DisableDebugRendering()
        {
            if (!_debugEnabled)
                return;
            lock (RenderSystems)
            {
                //delete
                for (int i = 0; i < RenderSystems.Count; i++)
                {
                    if (RenderSystems[i] is DebugRenderSystem)
                        RenderSystems[i] = null;
                }

                RenderSystems.RemoveAll(x => x == null);
                _debugEnabled = false;
            }
            
        }
        public void RegisterRenderSystem(IRogueRenderSystem system)
        {
            RenderSystems.Add(system);
        }

        bool _isPaused;
        public void Pause()
        {
            _isPaused = true;
        }

        public void UnPause()
        {
            _isPaused = false;
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

                if (hasTypes.All(x => x.Value) || sys is GameplaySystem)
                {
                    if( !sys.HasEntity(entity))
                        sys.AddEntity(entity);  
                }
            }
            
        }


        private void AddEntitiesToNewSystem(IRogueSystem sys)
        {
            foreach (var entity in Entities)
            {
                //get matching components and add to system
                var hasTypes = sys.RequiredComponents.ToDictionary(x => x, y => false);

                foreach (var compType in sys.RequiredComponents)
                {
                    hasTypes[compType] = entity.HasComponent(compType);
                }

                if (hasTypes.All(x => x.Value))
                    if (!sys.HasEntity(entity))
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
                    if (!sys.HasEntity(entity))
                        sys.AddEntity(entity);
            }
        }


        public void UpdateEntity(RogueEntity entity)
        {
            //update entity's system subscriptions
        }


        public void Update(GameTime gameTime)
        {
            if (_isPaused)
                return;
            foreach (var system in Systems)
            {
                system.Update(gameTime);
                
            }

            foreach (var renderSys in RenderSystems)
            {
                renderSys.Update(gameTime);
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