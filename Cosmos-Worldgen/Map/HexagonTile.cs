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
        private float light;
        private float atmosphericLight;
        /// <summary>
        /// Hexagon shape.
        /// </summary>
        public Hexagon Hexagon { get => hexagon; }
        /// <summary>
        /// Color of the tile.
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Color of the atmosphere.
        /// </summary>
        public Color AtmosphericColor { get; set; }
        /// <summary>
        /// Intensity of light. Between 0 and 1.
        /// </summary>
        public float Light { get => light; set => light = MathHelper.Clamp(value, 0.15f, 1f); }
        /// <summary>
        /// Intensity of light on top of the atmosphere. Unafected by atmospheric shadow.
        /// </summary>
        public float AtmosphericLight { get => atmosphericLight; set => atmosphericLight = MathHelper.Clamp(value, 0.15f, 1f); }
        /// <summary>
        /// Height of the tile onto the map.
        /// </summary>
        public float Height { get; set; }
        /// <summary>
        /// Temperature of the tile.
        /// </summary>
        public float Temperature { get; set; }
        /// <summary>
        /// Vegetation of the tile.
        /// </summary>
        public float Vegetation { get; set; }
        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Point Coords { get => coords; }
        /// <summary>
        /// Biome information of the tile.
        /// </summary>
        public Biome Biome { get; set; }
        /// <summary>
        /// Region of the tile.
        /// </summary>
        public Region Region { get; set; }


        public HexagonTile(Vector2 position, float size, Point coords)
        {
            hexagon = new Hexagon(position, size);
            this.coords = coords;
        }

    }
}
