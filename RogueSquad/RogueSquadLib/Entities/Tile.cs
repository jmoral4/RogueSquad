using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquadLib.Entities
{
    /// <summary>
    /// Represents the logical representation of a tile on-screen. This is different than the
    /// visual representation which would include dimensions/etc. This representation is primarily for usage in pathfinding and I related lookups.
    /// Note: The Texture is provided for simplicity and may be removed
    /// </summary>
    internal class Tile
    {
        public int Type { get; set; }
        public string Texture { get; set; }
        public int MovementCost { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Tile> Neighbors { get; set; }
        public bool Selected { get; set; }

        public Tile()
        { 
            Neighbors = new List<Tile>();
        }

    }
}
