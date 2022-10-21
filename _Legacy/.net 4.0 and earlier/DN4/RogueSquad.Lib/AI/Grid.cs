using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.AI
{
    public class Grid
    {
        public class CollisionBox
        {
            public int EntityId { get; set; }
            public Rectangle Rectangle { get; set; }
        }
        public class Cell
        {
            public Cell()
            {

            }

            public RectangleF Bounds { get; private set; }
            private List<CollisionBox> children = new List<CollisionBox>();
            public void Add(CollisionBox box)
            {
                this.children.Add(box);
            }

            public bool Contains(CollisionBox box)
            {
                return this.children.Any(x => x.EntityId == box.EntityId);                
            }

            public bool Remove(CollisionBox box)
            {
                return this.children.RemoveAll(x => x.EntityId == box.EntityId) > 0;                                
            }

            public bool Remove(int entityId)
            {
                return this.children.RemoveAll(x => x.EntityId == entityId) > 0;
            }

            public int Count()
            {
                return this.children.Count;
            }
        }

        public int CellSize { get; set; }
        public Cell[,] Cells { get; private set; }

        public float Width => this.Columns * CellSize;
        public float Height => this.Rows * CellSize;
        public int Columns => this.Cells.GetLength(0);
        public int Rows => this.Cells.GetLength(1);

        public Grid(int width, int height, int cellSize)
        {
            Cells = new Cell[width, height];
            CellSize = cellSize;
        }

        public IEnumerable<Cell> QueryCells(float x, float y, float w, float h)
        {
            var minX = (int)(x / this.CellSize);
            var minY = (int)(y / this.CellSize);
            var maxX = (int)((x + w - 1) / this.CellSize) + 1;
            var maxY = (int)((y + h - 1) / this.CellSize) + 1;

            minX = Math.Max(0, minX);
            minY = Math.Max(0, minY);
            maxX = Math.Min(this.Columns - 1, maxX);
            maxY = Math.Min(this.Rows - 1, maxY);

            List<Cell> result = new List<Cell>();

            for (int ix = minX; ix <= maxX; ix++)
            {
                for (int iy = minY; iy <= maxY; iy++)
                {
                    var cell = Cells[ix, iy];

                    if (cell == null)
                    {
                        cell = new Cell();
                        Cells[ix, iy] = cell;
                    }

                    result.Add(cell);
                }
            }

            return result;

        }


        public void Add(CollisionBox box)
        {
            var cells = this.QueryCells(box.Rectangle.X, box.Rectangle.Y, box.Rectangle.Width, box.Rectangle.Height);
            foreach (var cell in cells)
            {
                if (!cell.Contains(box))
                    cell.Add(box);
            }
        }

        public void Update(CollisionBox box, RectangleF from)
        {
            var fromCells = this.QueryCells(from.X, from.Y, from.Width, from.Height);
            var removed = false;
            foreach (var cell in fromCells)
            {
                removed |= cell.Remove(box);
            }
            if (removed)
                this.Add(box);          
        }

        public bool Remove(CollisionBox box)
        {
            var cells = this.QueryCells(box.Rectangle.X, box.Rectangle.Y, box.Rectangle.Width, box.Rectangle.Height);
            var removed = false;
            foreach (var cell in cells)
            {
                removed |= cell.Remove(box);
            }
            return removed;
        }



    }
}
