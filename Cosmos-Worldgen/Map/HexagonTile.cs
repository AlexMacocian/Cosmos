using Cosmos.WorldGen.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Map
{
    class HexagonTile
    {
        private Hexagon hexagon;
        private Point coords;

        public Hexagon Hexagon { get => hexagon; }
        public Color Color { get; set; }
        public float Height { get; set; }
        public float Temperature { get; set; }
        public Point Coords { get => coords; }
        public Biome Biome { get; set; }

        public HexagonTile(Vector2 position, float size, Point coords)
        {
            hexagon = new Hexagon(position, size);
            this.coords = coords;
        }

    }
}
