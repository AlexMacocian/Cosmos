using Cosmos.WorldGen.Geometry;
using Cosmos.WorldGen.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos.WorldGen
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float zoom = 1;
        float previousScrollWheelValue = 0;
        MouseState prevMouseState;
        HexagonTilemap hexagonTilemap;
        RenderTarget2D hexagonTexture;
        RenderTarget2D hexagonBorderTexture;
        Camera2D camera;
        double SQRT3 = 1.73205080757;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            hexagonTilemap = new HexagonTilemap(new Vector2(0, 0), 200, 200, 50, 1337, 0.5f, 0.5f, 0.3f, 0.8f);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            prevMouseState = Mouse.GetState();
            camera = new Camera2D(GraphicsDevice);

            this.Window.AllowUserResizing = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if(Mouse.GetState().ScrollWheelValue - previousScrollWheelValue < 0)
            {
                camera.ZoomOut(camera.Zoom * 0.1f);
            }
            else if(Mouse.GetState().ScrollWheelValue - previousScrollWheelValue > 0)
            {
                camera.ZoomIn(camera.Zoom * 0.1f);
            }
            previousScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            float dx = 0, dy = 0;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                MouseState mouseState = Mouse.GetState();
                dx = mouseState.X - prevMouseState.X;
                dy = mouseState.Y - prevMouseState.Y;
            }
            prevMouseState = Mouse.GetState();
            camera.Move(new Vector2(-dx / camera.Zoom, -dy / camera.Zoom));

            hexagonTilemap.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                hexagonTilemap.WindDirection += new Vector2(0, 0.01f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                hexagonTilemap.WindDirection += new Vector2(0, -0.01f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                hexagonTilemap.WindDirection += new Vector2(0.01f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                hexagonTilemap.WindDirection += new Vector2(-0.01f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                hexagonTilemap.Seed = new Random().Next();
                hexagonTilemap.GenerateMap();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    hexagonTilemap.AverageElevation += 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    hexagonTilemap.AverageTemperature += 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    hexagonTilemap.ElevationVariation += 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    hexagonTilemap.TemperatureVariation += 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                {
                    hexagonTilemap.FeatureDensity += 0.1f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    hexagonTilemap.AtmospherePresence += 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.U))
                {
                    hexagonTilemap.AtmosphereDensity += 0.1f;
                    hexagonTilemap.GenerateMap();
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    hexagonTilemap.AverageElevation -= 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    hexagonTilemap.AverageTemperature -= 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    hexagonTilemap.ElevationVariation -= 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    hexagonTilemap.TemperatureVariation -= 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                {
                    hexagonTilemap.FeatureDensity -= 0.1f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    hexagonTilemap.AtmospherePresence -= 0.05f;
                    hexagonTilemap.GenerateMap();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.U))
                {
                    hexagonTilemap.AtmosphereDensity -= 0.1f;
                    hexagonTilemap.GenerateMap();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (hexagonTexture == null)
            {
                hexagonTexture = new RenderTarget2D(GraphicsDevice, 2 * hexagonTilemap.TileSize, (int)(SQRT3 * hexagonTilemap.TileSize));
                hexagonBorderTexture = new RenderTarget2D(GraphicsDevice, 2 * hexagonTilemap.TileSize, (int)(SQRT3 * hexagonTilemap.TileSize));
                GraphicsDevice.SetRenderTarget((RenderTarget2D)hexagonTexture);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                Hexagon hexagon = new Hexagon(new Vector2(hexagonTilemap.Tiles[0, 0].Hexagon.Width / 2, hexagonTilemap.Tiles[0, 0].Hexagon.Height / 2), hexagonTilemap.TileSize);
                hexagon.Draw(gameTime, spriteBatch, Color.White, Color.Black);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget((RenderTarget2D)hexagonBorderTexture);
                spriteBatch.Begin();
                hexagon.Draw(gameTime, spriteBatch, Color.Transparent, Color.Red, 3);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);
            }
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, null, null, null, null, camera.GetViewMatrix());
            Vector2 startPos, endPos;
            hexagonTilemap.GetDrawingCoordinates(camera.BoundingRectangle, out startPos, out endPos);
            for (int i = (int)startPos.X; i < (int)endPos.X; i++)
            {
                for (int j = (int)Math.Floor(startPos.Y); j < (int)Math.Ceiling(endPos.Y); j++)
                {
                    Vector2 position = new Vector2(hexagonTilemap.Tiles[i, j].Hexagon.Position.X - hexagonTilemap.TileSize,
                        hexagonTilemap.Tiles[i, j].Hexagon.Position.Y - hexagonTilemap.TileSize);
                    spriteBatch.Draw(hexagonTexture, position, hexagonTilemap.Tiles[i, j].Color);
                }
            }
            for (int i = (int)startPos.X; i < (int)endPos.X; i++)
            {
                for (int j = (int)Math.Floor(startPos.Y); j < (int)Math.Ceiling(endPos.Y); j++)
                {
                    Vector2 position = new Vector2(hexagonTilemap.AtmosphericTiles[i, j].Hexagon.Position.X - hexagonTilemap.TileSize,
                        hexagonTilemap.Tiles[i, j].Hexagon.Position.Y - hexagonTilemap.TileSize);
                    spriteBatch.Draw(hexagonTexture, position, hexagonTilemap.AtmosphericTiles[i, j].Color);
                }
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
                mousePosition = camera.ScreenToWorld(mousePosition.X, mousePosition.Y);
                HexagonTile tile = hexagonTilemap.GetTileAtCoordinates(mousePosition);
                Vector2 position = new Vector2(tile.Hexagon.Position.X - hexagonTilemap.TileSize,
                        tile.Hexagon.Position.Y - hexagonTilemap.TileSize);
                spriteBatch.Draw(hexagonBorderTexture, position, Color.Red);
                foreach(HexagonTile nt in hexagonTilemap.GetAllNeighbors(tile))
                {
                    Vector2 pos = new Vector2(nt.Hexagon.Position.X - hexagonTilemap.TileSize,
                            nt.Hexagon.Position.Y - hexagonTilemap.TileSize);
                    spriteBatch.Draw(hexagonBorderTexture, pos, Color.Red);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(hexagonTexture, new Rectangle(Mouse.GetState().Position, new Point(8, 8)), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
