using Cosmos.WorldGen.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Map
{
    class HexagonAtmosphericTile
    {
        private Hexagon hexagon;
        private Point coords;

        public Hexagon Hexagon { get => hexagon; }
        public Color Color { get; set; }
        public Point Coords { get => coords; }

        public HexagonAtmosphericTile(Vector2 position, float size, Point coords)
        {
            hexagon = new Hexagon(position, size);
            this.coords = coords;
        }
    }
}
