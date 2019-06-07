using Cosmos.WorldGen.Drawing;
using Cosmos.WorldGen.Geometry;
using LibNoise;
using LibNoise.Builder;
using LibNoise.Filter;
using LibNoise.Primitive;
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
        private NoiseMap heightMap, atmosphereMap;
        private Vector2 origin, lightPosition;
        private static double SQRT3 = 1.73205080757;
        private int width, height, tileSize, seed;
        private LODProvider lodPrivider;
        HexagonTile[,] tiles;
        private float hexagonWidth, hexagonHeight, rotatingSpeed;
        private Dictionary<Biome.Biomes, Biome> biomes;
        private ColorMap colorMap;
        private Random random;
        private int updateCount = 0;
        private Sun sun = new Sun();
        private bool invalidNoise = true;
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
        public float FeatureDensity
        {
            get => featureDensity;
            set
            {
                featureDensity = MathHelper.Clamp(value, 0.01f, 10);
                invalidNoise = true;
            }
        }
        /// <summary>
        /// The amount of atmosphere gases on the map. Between 0 and 1.
        /// </summary>
        public float AtmospherePresence { get => atmospherePresence; set => atmospherePresence = MathHelper.Clamp(value, 0, 1f); }
        /// <summary>
        /// The density of atmospheric features. Between 0 and 25.
        /// </summary>
        public float AtmosphereDensity
        {
            get => atmosphereDensity;

            set
            {
                atmosphereDensity = MathHelper.Clamp(value, 0, 25);
                invalidNoise = true;
            }
        }
        /// <summary>
        /// Rotating speed of the planet.
        /// </summary>
        public float RotatingSpeed { get => rotatingSpeed; set => rotatingSpeed = MathHelper.Clamp(value, 0, 10); }
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
            heightMap = new NoiseMap(width, height);
            atmosphereMap = new NoiseMap(width, height);
            lodPrivider = new LODProvider();
            this.origin = origin;
            this.lightPosition = new Vector2((int)(width / 2), (int)(height / 2));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float oddCol = j % 2;
                    tiles[i, j] = new HexagonTile(new Vector2((oddCol * 1.5f * tileSize) + origin.X + (i * 3 * tileSize), 
                        origin.Y + (float)(j * SQRT3 * tileSize / 2)), 
                        tileSize,
                        new Point(i, j));
                    hexagonHeight = tiles[i, j].Hexagon.Height;
                    hexagonWidth = tiles[i, j].Hexagon.Width;
                }
            }
            GenerateBiomes();
            GenerateLODs();
            GenerateMap();
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Called each update frame to update the map.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, RectangleF visibleArea, Camera2D camera)
        {
            updateCount++;
            if(updateCount % 5 == 0)
            {
                lightPosition.X += rotatingSpeed;
            }
        }
        /// <summary>
        /// Generate the map with the currently set properties.
        /// </summary>
        public void GenerateMap()
        {
            random = new Random(seed);
            for (int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    tiles[i, j].Biome = null;
                }
            }
            if (invalidNoise)
            {
                GenerateNoise();
            }
            GenerateHeightMap();
            GenerateColors();
            GenerateLightMask();
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
        /// <summary>
        /// Draws the map onto the spritebatch.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to be used for drawing.</param>
        /// <param name="camera">Camera object.</param>
        /// <param name="hexagonTexture">Texture of a hexagon.</param>
        public void DrawMap(SpriteBatch spriteBatch, Camera2D camera, Texture2D hexagonTexture)
        {
            DrawLOD(spriteBatch, hexagonTexture, camera.BoundingRectangle, lodPrivider.GetMultiplier(camera.Zoom));
        }
        #endregion
        #region Private Methods
        private void GenerateLODs()
        {
            lodPrivider.AddLOD(0.4f, 1);
            lodPrivider.AddLOD(0.2f, 3);
            lodPrivider.AddLOD(0.1f, 5);
            lodPrivider.AddLOD(0.08f, 9);
            lodPrivider.AddLOD(0.02f, 11);
            lodPrivider.AddLOD(0.008f, 25);
        }
        /// <summary>
        /// Generate the noise values from current seed and settings.
        /// </summary>
        private void GenerateNoise()
        {
            NoiseMapBuilderSphere noiseMapBuilderSphere = new NoiseMapBuilderSphere();
            ImprovedPerlin improvedPerlin = new ImprovedPerlin();
            FilterModule filterModule = new SumFractal();
            filterModule.Frequency = featureDensity;
            filterModule.Primitive3D = improvedPerlin;
            improvedPerlin.Quality = NoiseQuality.Best;
            improvedPerlin.Seed = seed;
            noiseMapBuilderSphere.SetSize(width, height);
            noiseMapBuilderSphere.SourceModule = filterModule;

            noiseMapBuilderSphere.NoiseMap = heightMap;
            noiseMapBuilderSphere.Build();

            FilterModule filterModule2 = new LibNoise.Filter.Billow();

            filterModule2.Frequency = atmosphereDensity;
            filterModule2.Primitive3D = improvedPerlin;
            noiseMapBuilderSphere.SourceModule = filterModule2;

            noiseMapBuilderSphere.NoiseMap = atmosphereMap;
            noiseMapBuilderSphere.Build();
        }
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

            Biome hillsBiome = new Biome();
            hillsBiome.ColorMap.AddColorValue(1f, Color.Green);
            biomes.Add(Biome.Biomes.Hills, hillsBiome);

            Biome forestBiome = new Biome();
            forestBiome.ColorMap.AddColorValue(1f, Color.ForestGreen);
            biomes.Add(Biome.Biomes.Forest, forestBiome);

            Biome mountainBiome = new Biome();
            mountainBiome.ColorMap.AddColorValue(1f, Color.FromNonPremultiplied(210, 105, 30, 255));
            biomes.Add(Biome.Biomes.Mountain, mountainBiome);

            Biome desertBiome = new Biome();
            desertBiome.ColorMap.AddColorValue(1f, Color.FromNonPremultiplied(230, 142, 13, 255));
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
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    HexagonTile tile = tiles[i, j];

                    float heightcoef = heightMap.GetValue(i, j);
                    float tempcoef = heightcoef;
                    float vegetationcoef = heightcoef;

                    var v = ((float)Math.Abs((height / 2) - j) / (height / 2) * 2) - 1;

                    tile.Height = avgElevation + heightcoef * elevationVar;
                    tile.Temperature = (avgTemp - tempcoef * tempVar - v * tempVar);
                    tile.Vegetation = vegetationcoef;
                    GenerateTileBiome(tile);
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
        /// Generates clouds and shadows on tiles
        /// </summary>
        public void GenerateAtmosphere()
        {
            cloudPosition += windDirection;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    HexagonTile tile = tiles[i, j];
                    float coef = atmosphereMap.GetValue(i + (int)cloudPosition.X, j + (int)cloudPosition.X);

                    coef = (coef + 1) / 2;
                    tile.AtmosphericColor = Color.White * coef * atmospherePresence;
                    tile.Light = sun.LightValues[((int)(i + lightPosition.X) % width + width) % width, j];
                    tile.AtmosphericLight = tile.Light;
                    tile.Light -= (coef * atmospherePresence) * 0.7f;
                }
            }
        }
        /// <summary>
        /// Generate clouds and shadows on given visible area.
        /// </summary>
        /// <param name="visibleArea">Delimiter for the update location.</param>
        public void GenerateAtmosphere(Rectangle visibleArea, Camera2D camera)
        {
            GenerateAtmosphere(new RectangleF(visibleArea.Center, visibleArea.Size), camera);
        }
        /// <summary>
        /// Generate clouds and shadows on given visible area.
        /// </summary>
        /// <param name="visibleArea">Delimiter for the update location.</param>
        public void GenerateAtmosphere(RectangleF visibleArea, Camera2D camera)
        {
            GenerateAtmosphereLOD(visibleArea, lodPrivider.GetMultiplier(camera.Zoom));
        }
        /// <summary>
        /// Generates the atmospheric and light values.
        /// </summary>
        /// <param name="visibleArea">Renctangle delimiting the visible area.</param>
        /// <param name="lodMultiplier">Multiplier used to skip operations.</param>
        private void GenerateAtmosphereLOD(RectangleF visibleArea, float lodMultiplier)
        {
            Vector2 startPos = new Vector2();
            Vector2 endPos = new Vector2();
            GetDrawingCoordinates(visibleArea, out startPos, out endPos);
            startPos.X = (int)(startPos.X / lodMultiplier) * (int)lodMultiplier;
            startPos.Y = (int)(startPos.Y / lodMultiplier) * (int)lodMultiplier;
            cloudPosition += windDirection;
            for (int i = (int)startPos.X; i < (int)endPos.X; i+=(int)lodMultiplier)
            {
                for (int j = (int)startPos.Y; j < (int)endPos.Y; j+=(int)lodMultiplier)
                {
                    HexagonTile tile = tiles[i, j];

                    float coef = atmosphereMap.GetValue(((i + (int)cloudPosition.X) % width + width) % width,
                        ((j + (int)cloudPosition.Y) % height + height) % height);

                    coef = (coef + 1) / 2;
                    tile.AtmosphericColor = Color.White * coef * atmospherePresence;
                    tile.AtmosphericLight = sun.LightValues[((int)(i + lightPosition.X) % width + width) % width, j];
                    tile.Light = tile.AtmosphericLight - (coef * atmospherePresence);
                }
            }
        }
        /// <summary>
        /// Generate light on tile at given coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the tile.</param>
        /// <param name="y">Y coordinate of the tile.</param>
        /// <param name="lightCenterX">X coordinate of light.</param>
        /// <param name="lightCenterY">Y coordinate of light.</param>
        /// <param name="radiusSquared">Squared radius of light.</param>
        /// <returns></returns>
        private float GenerateLight(int x, int y, float lightCenterX, float lightCenterY, float radiusSquared)
        {
            Vector2 position = new Vector2(tiles[x, y].Hexagon.Position.X, tiles[x, y].Hexagon.Position.Y);
            position.X += hexagonWidth / 2;
            position.Y += hexagonHeight / 2;
            float dx = position.X - lightCenterX;
            float dy = position.Y - lightCenterY;
            float distanceSquared = dx * dx + dy * dy;
            float coef = (radiusSquared - distanceSquared) / radiusSquared;
            return coef;
        }
        /// <summary>
        /// Generates the light mask to be used to draw light.
        /// </summary>
        private void GenerateLightMask()
        {
            sun.LightValues = new float[width, height];

            float lightCenterX = tiles[width / 2, height / 2].Hexagon.Position.X + hexagonWidth / 2;
            float lightCenterY = tiles[width / 2, height / 2].Hexagon.Position.Y + hexagonHeight / 2;
            float radius = height * hexagonHeight / 3f;
            float radiusSquared = radius * radius;
            float halfMaxWidth = width * hexagonWidth;
            float maxDivergence = (halfMaxWidth * halfMaxWidth) - radiusSquared;

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    float factor = (float)Math.Abs(j - height / 2) / (height / 2);
                    float divergence = (float)(Math.Pow(138, factor) - 1) / 138;
                    sun.LightValues[i, j] = GenerateLight(i, j, lightCenterX, lightCenterY, radiusSquared + maxDivergence * divergence);
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
            return Math.Atan(Math.Exp(
                y / 180 * Math.PI
            )) / Math.PI * 360 - 90;
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
        /// <summary>
        /// Generate the tile biome based on the currently saved tile values.
        /// </summary>
        /// <param name="tile">Tile to be generated.</param>
        private void GenerateTileBiome(HexagonTile tile)
        {
            if (tile.Height < 0.3f)
            {
                if (tile.Temperature < 0.2f)
                {
                    tile.Biome = biomes[Biome.Biomes.FrozenOcean];
                }
                else if (tile.Temperature < 0.8f)
                {
                    tile.Biome = biomes[Biome.Biomes.Ocean];
                }
                else
                {
                    tile.Biome = biomes[Biome.Biomes.Desert];
                }
            }
            else if (tile.Height < 0.35f)
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
            else if (tile.Height < 0.6f)
            {
                if (tile.Temperature < 0.35f)
                {
                    tile.Biome = biomes[Biome.Biomes.Tundra];
                }
                else if (tile.Temperature < 0.7f)
                {
                    if (tile.Vegetation < 0.5f)
                    {
                        tile.Biome = biomes[Biome.Biomes.Land];
                    }
                    else
                    {
                        tile.Biome = biomes[Biome.Biomes.Forest];
                    }
                }
                else
                {
                    tile.Biome = biomes[Biome.Biomes.Desert];
                }
            }
            else if (tile.Height < 0.75f)
            {
                if (tile.Temperature < 0.35f)
                {
                    tile.Biome = biomes[Biome.Biomes.Tundra];
                }
                else if (tile.Temperature < 0.7f)
                {
                    if (tile.Vegetation < 0.5f)
                    {
                        tile.Biome = biomes[Biome.Biomes.Hills];
                    }
                    else
                    {
                        tile.Biome = biomes[Biome.Biomes.Forest];
                    }
                }
                else
                {
                    tile.Biome = biomes[Biome.Biomes.Desert];
                }
            }
            else if (tile.Height < 0.98f)
            {
                if (tile.Temperature < 0.7f)
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
        /// <summary>
        /// Draws the map onto the spritebatch, skipping operations based on the LOD multiplier.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="hexagonTexture">Hexagonal texture.</param>
        /// <param name="boundingRectangle">Bounding rectangle defining the visible area.</param>
        /// <param name="lodMultiplier">Multiplier used to skip drawing operations.</param>
        private void DrawLOD(SpriteBatch spriteBatch, Texture2D hexagonTexture, RectangleF boundingRectangle, float lodMultiplier)
        {
            int lodTileSize = TileSize * (int)lodMultiplier;
            Vector2 startPos, endPos;
            GetDrawingCoordinates(boundingRectangle, out startPos, out endPos);
            startPos.X = (int)(startPos.X / lodMultiplier) * (int)lodMultiplier;
            startPos.Y = (int)(startPos.Y / lodMultiplier) * (int)lodMultiplier;
            for (int i = (int)startPos.X; i < (int)endPos.X; i += (int)lodMultiplier)
            {
                for (int j = (int)Math.Floor(startPos.Y); j < (int)Math.Ceiling(endPos.Y); j += (int)lodMultiplier)
                {
                    float oddCol = j % 2;
                    float posX = Tiles[i, j].Hexagon.Position.X + (oddCol * 1.5f * lodTileSize) - (oddCol * 1.5f * tileSize);
                    Color color = Tiles[i, j].Color * Tiles[i, j].Light;
                    color.A = 255;
                    Color atmosphereColor = Tiles[i, j].AtmosphericColor * Tiles[i, j].AtmosphericLight;
                    atmosphereColor.A = 255;
                    spriteBatch.Draw(hexagonTexture, new Rectangle((int)posX, (int)Tiles[i, j].Hexagon.Position.Y, 2 * lodTileSize, (int)(SQRT3 * lodTileSize)), color);
                    spriteBatch.Draw(hexagonTexture, new Rectangle((int)posX, (int)Tiles[i, j].Hexagon.Position.Y, 2 * lodTileSize, (int)(SQRT3 * lodTileSize)), atmosphereColor);
                }
            }
        }
        #endregion
    }
}
