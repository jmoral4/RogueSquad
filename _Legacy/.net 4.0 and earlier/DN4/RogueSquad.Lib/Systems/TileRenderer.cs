using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSquad.Core.Components;
using MonoGame.Extended.ViewportAdapters;

namespace RogueSquad.Core.Systems
{

    public enum MapTileTypes
    {
        Ground, Object, Detail, Other
    }

    public class Tile
    {
        public string Name { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Source { get; set; }
        public bool IsWalkable { get; set; }
        public bool IsDetail { get; set; } //rendered on top of tiles        
        public int Width { get; set; }
        public int Height { get; set; }
        public MapTileTypes TileType { get; set; }
    }

    public class Node
    {
        public List<Tile> TileStack { get; set; } = new List<Tile>();
    }

    public class MapRenderingSystem : IRogueRenderSystem
    {
        readonly List<Tile> _tiles;
        readonly SpriteFont _font;
        readonly SpriteBatch _spriteBatch;
        readonly Point _tileDimensions;
        readonly int _xOffset;
        readonly int _yOffset;
        Point _mapDimensions;
        int offset = -100;    //overall offset for debug purposes
        Camera2D _camera;
        ViewportAdapter _viewportAdapter;
        int[,] Map { get; set; }

        Node[,] _map { get; set; }
        public IEnumerable<ComponentTypes> Required { get; set; } = new ComponentTypes[] { ComponentTypes.TileComponent };
        public IEnumerable<ComponentTypes> Any { get; set; } = Enumerable.Empty<ComponentTypes>();

        public MapRenderingSystem(List<Tile> tiles, Point tileDimensions, SpriteFont debugFont, SpriteBatch spriteBatch, Camera2D camera, ViewportAdapter viewportAdapter )
        {                        
            _tiles = tiles;
            _font = debugFont;
            _spriteBatch = spriteBatch;
            _tileDimensions = tileDimensions;
            _xOffset = tileDimensions.X / 2;  //tile offset assuming a 2:1 ratio
            _yOffset = tileDimensions.Y / 2;
            _camera = camera;
            _viewportAdapter = viewportAdapter;
        }

        public void CreateMap(int w, int h)
        {
            _mapDimensions = new Point(w, h);
            Map = new int[w, h];

            _map = new Node[w, h];

            Random rand = new Random();
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    var r = rand.Next(0, 100);
                    _map[i,j] = new Node();

                    if (r > 80)
                    {
                        Map[i, j] = 3;
                        _map[i, j].TileStack.Add(_tiles[3]);
                    }
                    else if (r > 60)
                    {
                        Map[i, j] = 2;
                        _map[i, j].TileStack.Add(_tiles[2]);
                    }
                    else if (r > 40)
                    {
                        Map[i, j] = 1;
                        _map[i, j].TileStack.Add(_tiles[1]);
                    }
                    else
                    {
                        Map[i, j] = 0;
                        _map[i, j].TileStack.Add(_tiles[0]);
                    }


                   
                }
            }

        }

        public void Draw(GameTime gameTime)
        {
            bool alt = false;
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: _camera.GetViewMatrix());
            for (int y = 0; y < Map.GetLength(1); y++)

            {

                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    if (alt)
                    {
                        var v = new Vector2((x * _tileDimensions.X) + offset, (y * _yOffset) + offset);
                        _spriteBatch.Draw(
                            _map[x, y].TileStack[0].Texture, new Rectangle((int)v.X, (int)v.Y, _tileDimensions.X, _tileDimensions.Y),
                            _map[x, y].TileStack[0].Source, 
                            Color.White
                            );
                        // _spriteBatch.DrawString(_font, $"{x},{y}", v, Color.Red);
                    }
                    else
                    {
                        var v = new Vector2((x * _tileDimensions.X) + offset - _tileDimensions.Y, (y * _yOffset) + offset);
                        _spriteBatch.Draw(
                            _map[x, y].TileStack[0].Texture,
                                                   new Rectangle((int)v.X, (int)v.Y, _tileDimensions.X, _tileDimensions.Y),
                                                   _map[x, y].TileStack[0].Source,
                                                    Color.White
                                                    );
                       // _spriteBatch.DrawString(_font, $"{x},{y}-A", v, Color.Red);
                    }

                }
                alt = !alt;
            }



            _spriteBatch.End();



        }

        public void Update(GameTime gametime)
        {
            //throw new NotImplementedException();
        }

        public void AddEntity(RogueEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool HasEntity(RogueEntity entity)
        {

            return false;
        }
    }

    
}
