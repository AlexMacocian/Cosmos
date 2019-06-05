using Cosmos.WorldGen.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Map
{
    class Biome
    {
        public enum Biomes
        {
            Ocean,
            Beach,
            Land,
            Mountain,
            Desert,
            Tundra,
            MountainPeak,
            FrozenOcean
        }
        /// <summary>
        /// Map of colors representing the region.
        /// Used to give colors to tiles in the current biome.
        /// </summary>
        public ColorMap ColorMap = new ColorMap();
        /// <summary>
        /// Get color.
        /// </summary>
        /// <param name="value">Value to return color from map.</param>
        /// <returns>Color from color map.</returns>
        public Color GetColor(float value)
        {
            return ColorMap.GetColor(value);
        }
    }
}
