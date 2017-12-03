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
        int offset = -100;
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
            _xOffset = tileDimensions.X / 2;
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
                            _map[x, y].TileStack[0].Texture,
                            //Map[x, y] == 0 ? _tiles[0].Texture : _tiles[1].Texture,
                           v,
                            Color.White
                            );
                        // _spriteBatch.DrawString(_font, $"{x},{y}", v, Color.Red);
                    }
                    else
                    {
                        var v = new Vector2((x * _tileDimensions.X) + offset - _tileDimensions.Y, (y * _yOffset) + offset);
                        _spriteBatch.Draw(
                            _map[x, y].TileStack[0].Texture,
                                                    //Map[x, y] == 0 ? _tiles[0].Texture : _tiles[1].Texture,
                                                     v,
                                                    Color.White
                                                    );
                        //_spriteBatch.DrawString(_font, $"{x},{y}-A", v, Color.Red);
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


    public class TileRenderingSystem 
    {
        public int [,]   Map { get; set; }

        private SpriteBatch _spriteBatch;

        Texture2D _grass;
        Texture2D _stone;
        SpriteFont _font;

        int tileW = 256;
        int tileH = 169;
        int offset = 100;
        int xOff;
        int yOff;

        public TileRenderingSystem(SpriteBatch spriteBatch, Texture2D grass, Texture2D stone, SpriteFont font)
        {
            _spriteBatch = spriteBatch;
            _grass = grass;
            _stone = stone;
             xOff = tileW / 2;
            yOff = tileH / 2;
            _font = font;
        }


        public void CreateMap(int w, int h)
        {
            Map = new int[w, h];
            Random rand = new Random();
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (rand.Next(0, 100) > 70)
                    {
                        Map[i, j] = 1;
                    }
                    else
                    {
                        Map[i, j] = 0;
                    }
                }
            }

        }

        public void Draw(GameTime gameTime)
        {
            bool alt = false;
            _spriteBatch.Begin();
            for (int y = 0; y < Map.GetLength(1); y++)
                
            {

                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    if (alt)
                    {
                        var v = new Vector2((x * tileW) + offset, (y * yOff) + offset);
                        _spriteBatch.Draw(
                            Map[x, y] == 0 ? _grass : _stone,
                           v,
                            Color.White
                            );
                       // _spriteBatch.DrawString(_font, $"{x},{y}", v, Color.Red);
                    }
                    else
                    {
                        var v = new Vector2((x * tileW) + offset - 128 , (y * yOff) + offset);
                        _spriteBatch.Draw(
                                                    Map[x, y] == 0 ? _grass : _stone,
                                                     v,
                                                    Color.White
                                                    );
                        //_spriteBatch.DrawString(_font, $"{x},{y}-A", v, Color.Red);
                    }
                  
                }
                alt = !alt;
            }



            _spriteBatch.End();



        }
        


    }
}
