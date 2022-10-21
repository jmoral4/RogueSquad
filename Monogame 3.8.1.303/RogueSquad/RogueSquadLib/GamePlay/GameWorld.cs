using RogueSquadLib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RogueSquadLib.Entities;

namespace RogueSquadLib.GamePlay
{
    internal class GameWorld
    {
        public string MapName { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public List<string> TextureNames { get; set; }
        public Tile[,] TileMap { get; set; }

        private GameWorld()
        {
            // TODO: This should perhaps be loaded earlier on and apart form the gameworld client            
            TextureNames = new List<string>();
            TileMap = new Tile[MapHeight, MapWidth];
        }

        // Load from an ini file that contains a "map gen" section
        public static GameWorld LoadFromFile(string iniFile)
        {
            // TODO: add some error handling..

            GameWorld world = new GameWorld();

            IniReader iniReader = new IniReader(iniFile);

            world.MapName = iniReader.GetValue("mapname", "map gen");
            world.MapWidth = Int32.Parse(iniReader.GetValue("map_width", "map gen"));
            world.MapHeight = Int32.Parse(iniReader.GetValue("map_height", "map gen"));
            world.TileWidth = Int32.Parse(iniReader.GetValue("tile_width", "map gen"));
            world.TileHeight = Int32.Parse(iniReader.GetValue("tile_height", "map gen"));

            // load up the tiles that will be used
            string[] textureNames = iniReader.GetValue("textures", "map gen").Split(",");
            world.TextureNames.AddRange(textureNames);
            world.TileMap = new Tile[world.MapHeight, world.MapWidth];
            return world;

        }

        // Create a random map using the current map width, map height, and tile_width/height and using the known textures
        public void GenerateRandomMap()
        {
            Random random = new Random();

            for (int i = 0; i < TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < TileMap.GetLength(1); j++)
                {
                    int selection = random.Next(0, 2);
                    var theTile = new Tile() { Type = selection, MovementCost = 1, X = i, Y = j };

                    TileMap[i, j] = theTile;
                }
            }

            // create the graph
            for (int i = 0; i < TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < TileMap.GetLength(1); j++)
                {

                    var theTile = TileMap[i, j];
                    // add neighbors
                    if (i > 0)
                    {
                        theTile.Neighbors.Add(TileMap[i - 1, j]);
                    }
                    if (i < TileMap.GetLength(0) - 1)
                    {
                        theTile.Neighbors.Add(TileMap[i + 1, j]);
                    }
                    if (j > 0)
                    {
                        theTile.Neighbors.Add(TileMap[i, j - 1]);
                    }
                    if (j < TileMap.GetLength(1) - 1)
                    {
                        theTile.Neighbors.Add(TileMap[i, j + 1]);
                    }

                }
            }



        }

    }
}
