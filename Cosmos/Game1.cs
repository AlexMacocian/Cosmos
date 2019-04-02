using Cosmos.Barnes_Hut;
using Cosmos.Content;
using Cosmos.Engine;
using Cosmos.Structures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MonoMenu.Engine;
using static Cosmos.Structures.Galaxy;
using System.Text;
using Cosmos.Effects;

namespace Cosmos
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        BloomFilter bloomFilter;
        Point clickPos;
        Point prevMousePos;
        float prevScrollWheelValue;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Circle circle;
        Texture2D pixel;
        SpriteFont font;
        double passedTime;
        bool pause, leftMousePressed;
        RenderTarget2D rt;
        MonoMenu.Engine.MonoMenu Menu;
        RenderTarget2D monoRenderTarget;
        RenderTarget2D preProcessRenderTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ThreadPool.SetMaxThreads(Environment.ProcessorCount * 2, 0);
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            bloomFilter = new BloomFilter();

            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = Environment.ProcessorCount;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.ApplyChanges();
            Galaxy.Instance = new Galaxy(500000000, 500000000, 1000, 0, 50);
            // TODO: Add your initialization logic here
            MonoMenu.Engine.Monitor.Initialize();
            monoRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            preProcessRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            Menu = new MonoMenu.Engine.MonoMenu(graphics.GraphicsDevice, Content);
            Camera.Instance = new Camera(graphics.GraphicsDevice.Viewport);
            Camera.Instance.Follow(Galaxy.Instance.Bodies[0]);
            MouseInput.LeftMouseButtonDoubleClick += InputLeftMouseDoubleClick;
            MouseInput.LeftMouseButtonClick += InputLeftMouseClick;
            MouseInput.ScrollDown += InputScrollDown;
            MouseInput.ScrollUp += InputScrollUp;
            MouseInput.Dragging += InputDragging;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            circle = new Circle(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            Menu.Load("Overlay1.xaml");
            MonoMenu.Engine.MonoMenu.defaultFont = font;
            bloomFilter.Load(GraphicsDevice, Content, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            bloomFilter.BloomPreset = BloomFilter.BloomPresets.Small;
            bloomFilter.BloomThreshold = 0.65f;
            bloomFilter.BloomStrengthMultiplier = 1.5f;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Galaxy.Instance.Stop();
            bloomFilter.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Menu.Update(gameTime);
            MonoMenu.Engine.Monitor.UpdateCalled();
            MouseInput.Poll(gameTime);
            Camera.Instance.Tick();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                pause = true;
            }
            else
            {
                pause = false;
                passedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (passedTime > 33)
                {
                    Galaxy.Instance.UpdateGalaxyOptimizedThreaded();
                    passedTime = 0;
                }

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                CelestialBody target = Galaxy.Instance.Bodies[1];
                foreach(CelestialBody potential in Galaxy.Instance.Bodies)
                {
                    if (potential is Star)
                    {

                    }
                    else
                    {
                        double targetDistance = Math.Pow(target.posX - Camera.Instance.Position.X, 2) + Math.Pow(target.posY - Camera.Instance.Position.Y, 2);
                        double potentialDistance = Math.Pow(potential.posX - Camera.Instance.Position.X, 2) + Math.Pow(potential.posY - Camera.Instance.Position.Y, 2);
                        if (potentialDistance < targetDistance)
                        {
                            target = potential;
                        }
                    }
                }
                Camera.Instance.Follow(target);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                Constants.TIME_CONSTANT *= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                Constants.TIME_CONSTANT /= 2;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            bool found = false;
            MonoMenu.Engine.Monitor.DrawCalled();

            Matrix cameraTransf = Camera.Instance.Transform;
            Vector3 transl = cameraTransf.Translation;
            Vector2 mClkPos = new Vector2(clickPos.X - GraphicsDevice.PresentationParameters.BackBufferWidth / 2, clickPos.Y - GraphicsDevice.PresentationParameters.BackBufferHeight / 2);
            mClkPos /= Camera.Instance.Zoom;
            mClkPos += Camera.Instance.Position;
            string fps = "FPS: " + (int)MonoMenu.Engine.Monitor.FrameRate;
            string bods = "Bodies: " + Galaxy.Instance.Bodies.Count;
            string markedToRemove = "Marked to be removed: " + Galaxy.Instance.ToRemove;
            string iterations = "Iterations: " + Galaxy.Instance.Iterations;
            string memoryusg = "Memory Usage: " + (MonoMenu.Engine.Monitor.MemoryUsage / 10e5);
            string camerapos = "Camera position: " + (int)Camera.Instance.Position.X + " " + (int)Camera.Instance.Position.Y;
            string mousepos = "Mouse position: " + (int)mClkPos.X + " " + (int)mClkPos.Y;
            string zoomlvl = "Zoom level: " + Camera.Instance.Zoom;
            string following = string.Empty, mass = string.Empty, pxpy = string.Empty, vxvy = string.Empty;
            if (Camera.Instance.Following != null)
            {
                following = "Following: " + Camera.Instance.Following.id;
                if(Camera.Instance.Following is Star)
                {
                    double m = Camera.Instance.Following.mass / Constants.SUN_MASS;
                    mass = "Mass: " + m.ToString("0.##E+0", CultureInfo.InvariantCulture) + " Sun Masses";
                }
                else
                {
                    double m = Camera.Instance.Following.mass / Constants.EARTH_SIZE; 
                    mass = "Mass: " + m.ToString("0.##E+0", CultureInfo.InvariantCulture) + " Earth Masses";
                }
                pxpy = "pX: " + Camera.Instance.Following.posX + " - pY: " + Camera.Instance.Following.posY;
                vxvy = "vX: " + Camera.Instance.Following.vX + " - vY: " + Camera.Instance.Following.vY;
            }
            Menu["fpsRect"].Text = fps;
            Menu["bodsRect"].Text = bods;
            Menu["markedRect"].Text = markedToRemove;
            Menu["itRect"].Text = iterations;
            Menu["memRect"].Text = memoryusg;
            Menu["camRect"].Text = camerapos;
            Menu["mouseRect"].Text = mousepos;
            Menu["zoomRect"].Text = zoomlvl;
            Menu["followRect"].Text = following;
            Menu["massRect"].Text = mass;
            Menu["pxpyRect"].Text = pxpy;
            Menu["vxvyRect"].Text = vxvy;
            Menu.Draw(spriteBatch, monoRenderTarget);


            GraphicsDevice.SetRenderTarget(preProcessRenderTarget);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cameraTransf);

            GraphicsDevice.Clear(Color.Black);

            foreach (CelestialBody body in Galaxy.Instance.Bodies)
            {
                Rectangle destRect = new Microsoft.Xna.Framework.Rectangle((int)Math.Round(body.posX) - (int)Math.Round(body.size / 2), (int)Math.Round(body.posY) - (int)Math.Round(body.size / 2), (int)body.size, (int)body.size);
                if (Camera.Instance.VisibleArea.Contains(destRect) || Camera.Instance.VisibleArea.Intersects(destRect))
                {            
                    if (destRect.Contains(mClkPos) && leftMousePressed)
                    {
                        found = true;
                        Camera.Instance.Follow(body);
                    }
                    Color color = Color.Green;
                    if(body is Star)
                    {
                        color = GetColor(body as Star);
                    }
                    spriteBatch.Draw(circle.CircleText, destRect, color);
                    //spriteBatch.Draw(circle.CircleText, new Microsoft.Xna.Framework.Rectangle((int)(body.Position.X + graphics.PreferredBackBufferWidth / 2 - 2.5), (int)(body.Position.Y + graphics.PreferredBackBufferHeight / 2 - 2.5), 5, 5), Color.Red);
                }
            }

            if(leftMousePressed && !found)
            {
                Camera.Instance.Follow(null);
            }

            #region Draw quad outline
            if (pause)
            {
                while (!System.Threading.Monitor.TryEnter(Galaxy.Instance.quadLock))
                {

                }
                Galaxy.Instance.root.DrawOutline(spriteBatch, pixel, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, Camera.Instance.VisibleArea);
                System.Threading.Monitor.Exit(Galaxy.Instance.quadLock);
            }
            #endregion

            spriteBatch.End();

            Texture2D bloom = bloomFilter.Draw(preProcessRenderTarget, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.Draw(preProcessRenderTarget, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bloom, new Vector2(-3f, -3f), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(new Point(MouseInput.MousePosition.X - 2, MouseInput.MousePosition.Y - 2), new Point(4, 4)), Color.Red);
            spriteBatch.Draw(monoRenderTarget, new Rectangle(0, 0, monoRenderTarget.Width, monoRenderTarget.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
            leftMousePressed = false;
        }

        private Color GenerateColor(CelestialBody body)
        {
            double speed = body.vX * body.vX + body.vY * body.vY;

            float val = (float)(speed / 50);

            float l = 0.5f * val;
            float h = 0.63f - (0.63f * val);
            h = MathHelper.Clamp(h, 0, 0.63f);
            l = MathHelper.Clamp(l, 0.4f, 1f);
            return Helper.HSLtoRGB(h, 0, l);
        }

        private Color GetColor(Star star)
        {
            switch (star.starClass)
            {
                case Star.Class.O:
                    return Color.FromNonPremultiplied(150, 180, 200, 255);
                case Star.Class.B:
                    return Color.Cyan;
                case Star.Class.A:
                    return Color.LightBlue;
                case Star.Class.F:
                    return Color.Wheat;
                case Star.Class.G:
                    return Color.Yellow;
                case Star.Class.K:
                    return Color.Orange;
                case Star.Class.M:
                    return Color.OrangeRed;
                default:
                    return Color.Red;
            }
        }

        private void InputLeftMouseClick(object sender, EventArgs e)
        {

        }

        private void InputLeftMouseDoubleClick(object sender, EventArgs e)
        {
            clickPos = MouseInput.MousePosition;
            leftMousePressed = true;
        }

        private void InputScrollUp(object sender, double delta)
        {
            Camera.Instance.Zoom += Camera.Instance.Zoom / 10;
        }

        private void InputScrollDown(object sender, double delta)
        {
            Camera.Instance.Zoom -= Camera.Instance.Zoom / 10;
        }

        private void InputDragging(object sender, Point delta)
        {
            Camera.Instance.MoveCamera(new Vector2((float)-delta.X / Camera.Instance.Zoom, (float)-delta.Y / Camera.Instance.Zoom));
            Camera.Instance.Follow(null);
        }
    }
}
