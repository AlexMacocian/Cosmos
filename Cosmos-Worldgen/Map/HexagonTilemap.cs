using Cosmos.WorldGen.Drawing;
using Cosmos.WorldGen.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Map
{
    class HexagonTilemap
    {
        #region Fields
        private Vector2 origin;
        private NoiseGenerator noiseGenerator;
        private static double SQRT3 = 1.73205080757;
        private int width, height, tileSize, seed;
        HexagonTile[,] tiles;
        HexagonAtmosphericTile[,] atmosphericTiles;
        private float hexagonWidth, hexagonHeight;
        private Dictionary<Biome.Biomes, Biome> biomes;
        private ColorMap colorMap;
        private Random random;
        private int updateCount = 0; 
        #region Terrain-gen values
        float avgTemp, tempVar, avgElevation, elevationVar, featureDensity, atmospherePresence, atmosphereDensity;
        Vector2 windDirection = new Vector2();
        Vector2 cloudPosition = new Vector2();
        #endregion
        #endregion
        #region Properties
        /// <summary>
        /// An enum of possible neighbor directions.
        /// </summary>
        public enum NeighborDirections
        {
            NorthWest,
            North,
            NorthEast,
            SouthEast,
            South,
            SouthWest
        }
        /// <summary>
        /// Array storage of atmospheric tiles.
        /// </summary>
        public HexagonAtmosphericTile[,] AtmosphericTiles {
            get => atmosphericTiles;
        }
        /// <summary>
        /// Array storage of map tiles.
        /// </summary>
        public HexagonTile[,] Tiles
        {
            get => tiles;
        }
        /// <summary>
        /// Size of one tile.
        /// </summary>
        public int TileSize { get => tileSize; }
        /// <summary>
        /// Width of the map, in voxels.
        /// </summary>
        public int Width { get => width; }
        /// <summary>
        /// Height of the map, in voxels.
        /// </summary>
        public int Height { get => height; }
        /// <summary>
        /// Average temperature of the map. Between 0 and 1.
        /// </summary>
        public float AverageTemperature { get => avgTemp; set => avgTemp = MathHelper.Clamp(value, 0, 1); }
        /// <summary>
        /// Maximum variation of the temperature. Between 0 and 1.
        /// </summary>
        public float TemperatureVariation { get => tempVar; set => tempVar = MathHelper.Clamp(value, 0, 1); }
        /// <summary>
        /// Average elevation of the map. Between 0 and 1.
        /// </summary>
        public float AverageElevation { get => avgElevation; set => avgElevation = MathHelper.Clamp(value, 0, 1); }
        /// <summary>
        /// Maximum variation of the elevation. Between 0 and 1.
        /// </summary>
        public float ElevationVariation { get => elevationVar; set => elevationVar = MathHelper.Clamp(value, 0, 1); }
        /// <summary>
        /// Density of terrain features. Between 0.01 and 10.
        /// </summary>
        public float FeatureDensity { get => featureDensity; set => featureDensity = MathHelper.Clamp(value, 0.01f, 10); }
        /// <summary>
        /// The amount of atmosphere gases on the map. Between 0 and 1.
        /// </summary>
        public float AtmospherePresence { get => atmospherePresence; set => atmospherePresence = MathHelper.Clamp(value, 0, 1); }
        /// <summary>
        /// The density of atmospheric features. Between 0 and 25.
        /// </summary>
        public float AtmosphereDensity { get => atmosphereDensity; set => atmosphereDensity = MathHelper.Clamp(value, 0, 25); }
        /// <summary>
        /// Map seed.
        /// </summary>
        public int Seed { get => seed; set => seed = value; }
        /// <summary>
        /// Current direction of wind in 2D space. Higher values means higher speed.
        /// </summary>
        public Vector2 WindDirection { get => windDirection; set => windDirection = value; }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor for hexagon tilemap.
        /// </summary>
        /// <param name="origin">Origin position.</param>
        /// <param name="width">Width of the map, in voxels.</param>
        /// <param name="height">Height of the map, in voxels.</param>
        /// <param name="tileSize">Size of one tile.</param>
        /// <param name="seed">Seed used for terrain generation.</param>
        /// <param name="avgTemp">Average temperature of the map. Between 0 and 1.</param>
        /// <param name="tempVar">Maximum variation of the temperature. Between 0 and 1.</param>
        /// <param name="avgElevation">Average elevation of the map. Between 0 and 1.</param>
        /// <param name="elevationVar">Maximum variation of the elevation. Between 0 and 1.</param>
        /// <param name="featureDensity">Density of terrain features. Between 0.01 and 10.</param>
        public HexagonTilemap(Vector2 origin, int width, int height, int tileSize, 
            int seed = 1337, float avgTemp = 0.3f, float tempVar = 0.2f, float avgElevation = 0.4f, float elevationVar = 0.4f,
            float featureDensity = 1.5f, float atmospherePresence = 0.2f, float atmosphereDensity = 2f)
        {
            AtmospherePresence = atmospherePresence;
            AtmosphereDensity = atmosphereDensity;
            AverageTemperature = avgTemp;
            TemperatureVariation = tempVar;
            AverageElevation = avgElevation;
            ElevationVariation = elevationVar;
            FeatureDensity = featureDensity;
            biomes = new Dictionary<Biome.Biomes, Biome>();
            Seed = seed;
            this.tileSize = tileSize;
            this.width = width;
            this.height = height;
            tiles = new HexagonTile[width, height];
            atmosphericTiles = new HexagonAtmosphericTile[width, height];
            this.origin = origin;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float oddCol = j % 2;
                    tiles[i, j] = new HexagonTile(new Vector2((oddCol * 1.5f * tileSize) + origin.X + (i * 3 * tileSize), 
                        origin.Y + (float)(j * SQRT3 * tileSize / 2)), 
                        tileSize,
                        new Point(i, j));
                    atmosphericTiles[i, j] = new HexagonAtmosphericTile(new Vector2((oddCol * 1.5f * tileSize) + origin.X + (i * 3 * tileSize),
                        origin.Y + (float)(j * SQRT3 * tileSize / 2)),
                        tileSize,
                        new Point(i, j));
                    hexagonHeight = tiles[i, j].Hexagon.Height;
                    hexagonWidth = tiles[i, j].Hexagon.Width;
                }
            }
            GenerateBiomes();
            GenerateMap();
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Called each update frame to update the map.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            updateCount++;
            if(updateCount % 5 == 0)
            {
                GenerateAtmosphere();
            }
        }
        /// <summary>
        /// Generate the map with the currently set properties.
        /// </summary>
        public void GenerateMap()
        {
            noiseGenerator = new NoiseGenerator(seed);
            random = new Random(seed);
            for (int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    tiles[i, j].Biome = null;
                }
            }
            GenerateHeightMap();
            GenerateColors();
            GenerateAtmosphere();
        }
        /// <summary>
        /// Method to get the tile at the specified coordinates.
        /// </summary>
        /// <param name="coords">Provided coordinates.</param>
        /// <returns>Hexagon tile at provided coordinates.</returns>
        public HexagonTile GetTileAtCoordinates(Vector2 coords)
        {
            float x = (float)coords.X / (float)(3 * tileSize);
            float y = (float)coords.Y / (float)(SQRT3 * tileSize / 2);
            HexagonTile tile = null;
            int indexX = (int)MathHelper.Clamp(x, 1, width - 2);
            int indexY = (int)MathHelper.Clamp(y, 1, height - 2);
            double distance = double.MaxValue;
            for(int i = indexX - 1; i <= indexX + 1; i++)
            {
                for(int j = indexY - 1; j <= indexY + 1; j++)
                {
                    double d = (coords - tiles[i, j].Hexagon.Position).LengthSquared();
                    if(d < distance)
                    {
                        distance = d;
                        tile = tiles[i, j];
                    }
                }
            }
            return tile;
        }
        /// <summary>
        /// Method to generate the starting and ending coordinates of the tiles to be drawn.
        /// </summary>
        /// <param name="visibleArea">Rectangle delimiting the visible area.</param>
        /// <param name="startCoord">Starting indices in the tile array.</param>
        /// <param name="endCoords">Ending indices in the tile array.</param>
        public void GetDrawingCoordinates(Rectangle visibleArea, out Vector2 startCoord, out Vector2 endCoords)
        {
            float startX = (float)visibleArea.Left / (float)(3 * tileSize);
            float endX = (float)visibleArea.Right / (float)(3 * tileSize);
            float startY = (float)visibleArea.Top / (float)(SQRT3 * tileSize / 2);
            float endY = (float)visibleArea.Bottom / (float)(SQRT3 * tileSize / 2);
            startX -= 1; endX += 1; startY -= 1; endY += 1;
            startCoord = new Vector2(MathHelper.Clamp(startX, 0, width), MathHelper.Clamp(startY, 0, height));
            endCoords = new Vector2(MathHelper.Clamp(endX, 0, width), MathHelper.Clamp(endY, 0, height));
        }
        /// <summary>
        /// Method to generate the starting and ending coordinates of the tiles to be drawn.
        /// </summary>
        /// <param name="visibleArea">Rectangle delimiting the visible area.</param>
        /// <param name="startCoord">Starting indices in the tile array.</param>
        /// <param name="endCoords">Ending indices in the tile array.</param>
        public void GetDrawingCoordinates(RectangleF visibleArea, out Vector2 startCoord, out Vector2 endCoords)
        {
            float startX = (float)visibleArea.Left / (float)(3 * tileSize);
            float endX = (float)visibleArea.Right / (float)(3 * tileSize);
            float startY = (float)visibleArea.Top / (float)(SQRT3 * tileSize / 2);
            float endY = (float)visibleArea.Bottom / (float)(SQRT3 * tileSize / 2);
            startX -= 1; endX += 1; startY -= 1; endY += 1;
            startCoord = new Vector2(MathHelper.Clamp(startX, 0, width), MathHelper.Clamp(startY, 0, height));
            endCoords = new Vector2(MathHelper.Clamp(endX, 0, width), MathHelper.Clamp(endY, 0, height));
        }
        /// <summary>
        /// Returns the specified neighbor of the provided tile.
        /// </summary>
        /// <param name="tile">Current tile.</param>
        /// <param name="neighboringTile">Direction of the neighboring tile.</param>
        /// <returns>Neighboring tile.</returns>
        public HexagonTile GetNeighboringTile(HexagonTile tile, NeighborDirections neighboringTile)
        {
            Point coords = tile.Coords;
            Point newCoords = new Point();
            if (neighboringTile == NeighborDirections.North)
            {
                newCoords.X = coords.X;
                newCoords.Y = coords.Y - 2;
            }
            else if (neighboringTile == NeighborDirections.South)
            {
                newCoords.X = coords.X;
                newCoords.Y = coords.Y + 2;
            }
            else if (neighboringTile == NeighborDirections.NorthWest)
            {
                if (coords.Y % 2 == 1)
                {
                    //Odd numbered row
                    newCoords.X = coords.X;
                    newCoords.Y = coords.Y - 1;
                }
                else
                {
                    //Even numbered row
                    newCoords.X = coords.X - 1;
                    newCoords.Y = coords.Y - 1;
                }
            }
            else if (neighboringTile == NeighborDirections.NorthEast)
            {
                if (coords.Y % 2 == 1)
                {
                    //Odd numbered row
                    newCoords.X = coords.X + 1;
                    newCoords.Y = coords.Y - 1;
                }
                else
                {
                    //Even numbered row
                    newCoords.X = coords.X;
                    newCoords.Y = coords.Y - 1;
                }
            }
            else if (neighboringTile == NeighborDirections.SouthEast)
            {
                if (coords.Y % 2 == 1)
                {
                    //Odd numbered row
                    newCoords.X = coords.X + 1;
                    newCoords.Y = coords.Y + 1;
                }
                else
                {
                    //Even numbered row
                    newCoords.X = coords.X;
                    newCoords.Y = coords.Y + 1;
                }
            }
            else if (neighboringTile == NeighborDirections.SouthWest)
            {
                if (coords.Y % 2 == 1)
                {
                    //Odd numbered row
                    newCoords.X = coords.X;
                    newCoords.Y = coords.Y + 1;
                }
                else
                {
                    //Even numbered row
                    newCoords.X = coords.X - 1;
                    newCoords.Y = coords.Y + 1;
                }
            }
            newCoords.X = (newCoords.X % width + width) % width;
            newCoords.Y = (newCoords.Y % height + height) % height;
            return tiles[newCoords.X, newCoords.Y];
        }
        /// <summary>
        /// Method to get all neighbors of current tile.
        /// </summary>
        /// <param name="tile">Tile to return neighbors from.</param>
        /// <returns>All neighbors of provided tile.</returns>
        public HexagonTile[] GetAllNeighbors(HexagonTile tile)
        {
            HexagonTile[] hexagonTiles = new HexagonTile[6];
            hexagonTiles[0] = GetNeighboringTile(tile, NeighborDirections.NorthWest);
            hexagonTiles[1] = GetNeighboringTile(tile, NeighborDirections.North);
            hexagonTiles[2] = GetNeighboringTile(tile, NeighborDirections.NorthEast);
            hexagonTiles[3] = GetNeighboringTile(tile, NeighborDirections.SouthEast);
            hexagonTiles[4] = GetNeighboringTile(tile, NeighborDirections.South);
            hexagonTiles[5] = GetNeighboringTile(tile, NeighborDirections.SouthWest);
            return hexagonTiles;
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Generates biomes to be used in terraforming.
        /// </summary>
        private void GenerateBiomes()
        {
            Biome oceanBiome = new Biome();
            oceanBiome.ColorMap.AddColorValue(1f, Color.Blue);
            biomes.Add(Biome.Biomes.Ocean, oceanBiome);

            Biome beachBiome = new Biome();
            beachBiome.ColorMap.AddColorValue(1f, Color.SandyBrown);
            biomes.Add(Biome.Biomes.Beach, beachBiome);

            Biome landBiome = new Biome();
            landBiome.ColorMap.AddColorValue(1f, Color.LawnGreen);
            biomes.Add(Biome.Biomes.Land, landBiome);

            Biome mountainBiome = new Biome();
            mountainBiome.ColorMap.AddColorValue(1f, Color.FromNonPremultiplied(210, 105, 30, 255));
            biomes.Add(Biome.Biomes.Mountain, mountainBiome);

            Biome desertBiome = new Biome();
            desertBiome.ColorMap.AddColorValue(1f, Color.LightGoldenrodYellow);
            biomes.Add(Biome.Biomes.Desert, desertBiome);

            Biome tundraBiome = new Biome();
            tundraBiome.ColorMap.AddColorValue(1f, Color.White);
            biomes.Add(Biome.Biomes.Tundra, tundraBiome);

            Biome frozenOceanBiome = new Biome();
            frozenOceanBiome.ColorMap.AddColorValue(1f, Color.White);
            biomes.Add(Biome.Biomes.FrozenOcean, frozenOceanBiome);

            Biome mountainPeakBiome = new Biome();
            mountainPeakBiome.ColorMap.AddColorValue(1f, Color.White);
            biomes.Add(Biome.Biomes.MountainPeak, mountainPeakBiome);
        }
        /// <summary>
        /// Generates a rough heightmap to be further manipulated.
        /// </summary>
        private void GenerateHeightMap()
        {
            noiseGenerator.SetFrequency(featureDensity);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    HexagonTile tile = tiles[i, j];

                    float x1 = -1, x2 = 1, y1 = -1, y2 = 1;
                    float s = (float)(i) / width;
                    float t = (float)(j) / height;
                    float dx = x2 - x1, dy = y2 - y1;

                    float nx = (float)(x1 + Math.Cos(s * 2 * Math.PI) * dx / (2 * Math.PI));
                    float ny = (float)(y1 + Math.Cos(t * 2 * Math.PI) * dy / (2 * Math.PI));
                    float nz = (float)(x1 + Math.Sin(s * 2 * Math.PI) * dx / (2 * Math.PI));
                    float nw = (float)(y1 + Math.Sin(t * 2 * Math.PI) * dy / (2 * Math.PI));

                    float coef = noiseGenerator.GetSimplex(nx, ny, nz, nw);

                    var v = 1f - ((float)Math.Abs((height / 2) - j) / (height / 2));

                    tile.Height = avgElevation + coef * elevationVar;
                    tile.Temperature = (avgTemp - coef * tempVar) * Math.Min(v * 4, 1);
                    if (tile.Height < 0.3f)
                    {
                        if(tile.Temperature < 0.2f)
                        {
                            tile.Biome = biomes[Biome.Biomes.FrozenOcean];
                        }
                        else if(tile.Temperature < 0.8f)
                        {
                            tile.Biome = biomes[Biome.Biomes.Ocean];
                        }
                        else
                        {
                            tile.Biome = biomes[Biome.Biomes.Desert];
                        }                          
                    }
                    else if(tile.Height < 0.35f)
                    {
                        if (tile.Temperature < 0.8f)
                        {
                            tile.Biome = biomes[Biome.Biomes.Beach];
                        }
                        else
                        {
                            tile.Biome = biomes[Biome.Biomes.Desert];
                        }
                    }
                    else if(tile.Height < 0.7f)
                    {
                        if(tile.Temperature < 0.35f)
                        {
                            tile.Biome = biomes[Biome.Biomes.Tundra];
                        }
                        else if(tile.Temperature < 0.7f)
                        {
                            tile.Biome = biomes[Biome.Biomes.Land];
                        }
                        else
                        {
                            tile.Biome = biomes[Biome.Biomes.Desert];
                        }                       
                    }
                    else if(tile.Height < 0.9f)
                    {
                        if (tile.Temperature < 0.35f)
                        {
                            tile.Biome = biomes[Biome.Biomes.Tundra];
                        }
                        else if (tile.Temperature < 0.7f)
                        {
                            tile.Biome = biomes[Biome.Biomes.Mountain];
                        }
                        else
                        {
                            tile.Biome = biomes[Biome.Biomes.Desert];
                        }
                    }
                    else
                    {
                        if (tile.Temperature < 0.7f)
                        {
                            tile.Biome = biomes[Biome.Biomes.MountainPeak];
                        }
                        else
                        {
                            tile.Biome = biomes[Biome.Biomes.Desert];
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Generates tile colors from calculated values.
        /// </summary>
        private void GenerateColors()
        {
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    tiles[i, j].Color = tiles[i, j].Biome.GetColor(tiles[i, j].Temperature);
                }
            }
        }
        /// <summary>
        /// Generates clouds on tiles
        /// </summary>
        private void GenerateAtmosphere()
        {
            noiseGenerator.SetFrequency(atmosphereDensity);
            cloudPosition += windDirection;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    HexagonAtmosphericTile tile = atmosphericTiles[i, j];

                    float x1 = -1, x2 = 1, y1 = -1, y2 = 1;
                    float s = (float)(i + cloudPosition.X) / width;
                    float t = (float)(j + cloudPosition.Y) / height;
                    float dx = x2 - x1, dy = y2 - y1;

                    float nx = (float)(x1 + Math.Cos(s * 2 * Math.PI) * dx / (2 * Math.PI));
                    float ny = (float)(y1 + Math.Cos(t * 2 * Math.PI) * dy / (2 * Math.PI));
                    float nz = (float)(x1 + Math.Sin(s * 2 * Math.PI) * dx / (2 * Math.PI));
                    float nw = (float)(y1 + Math.Sin(t * 2 * Math.PI) * dy / (2 * Math.PI));

                    float coef = noiseGenerator.GetSimplex(nx, ny, nz, nw);

                    coef = (coef + 1) / 2;
                    tile.Color = Color.White * coef * atmospherePresence;
                }
            }
        }
        /// <summary>
        /// Convert plane Y coordinate into a mercator projection latitude.
        /// </summary>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Latitude respective to provided Y.</returns>
        private double YToLatitude(double y)
        {
            return System.Math.Atan(System.Math.Exp(
                y / 180 * System.Math.PI
            )) / System.Math.PI * 360 - 90;
        }
        /// <summary>
        /// Convert latitude to Y coordinate on plane.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <returns>Y coordinate respective to the provided latitude.</returns>
        private double LatitudeToY(double latitude)
        {
            return System.Math.Log(System.Math.Tan(
                (latitude + 90) / 360 * System.Math.PI
            )) / System.Math.PI * 180;
        }
        #endregion
    }
}
