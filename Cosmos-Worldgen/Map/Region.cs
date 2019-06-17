using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Map
{
    class Region
    {
        List<HexagonTile> tiles;
        string name;
        Color color;
        /// <summary>
        /// List of tiles in the region.
        /// </summary>
        public List<HexagonTile> Tiles { get => tiles; set => tiles = value; }
        /// <summary>
        /// Name of the region.
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Biome of the region.
        /// </summary>
        public Biome Biome { get; set; }
        public Color Color { get => color; }

        public Region(Color color, string name = "")
        {
            tiles = new List<HexagonTile>();
            this.name = name;
            this.color = color;
        }
    }
}
