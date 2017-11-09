using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Systems
{
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
