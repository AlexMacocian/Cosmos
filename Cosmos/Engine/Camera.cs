using Cosmos.Structures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    public class Camera
    {
        public static Camera Instance;
        public float Zoom { get { return zoom; } set { zoom = value; UpdateMatrix(); } }
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public Matrix Transform { get; protected set; }
        public CelestialBody Following
        {
            get
            {
                return followBody;
            }
        }

        private float currentMouseWheelValue, previousMouseWheelValue, zoom, previousZoom;
        private bool panning = false, following = false;
        private Vector2 panTarget;
        private CelestialBody followBody;

        public Camera(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Zoom = 1f;
            Position = Vector2.Zero;
            panTarget = Position;
        }

        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void MoveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = new Vector2((int)Math.Round(Position.X + movePosition.X), (int)Math.Round(Position.Y + movePosition.Y));
            Position = newPosition;
            UpdateMatrix();
        }

        public void Tick()
        {
            if (panning)
            {
                Vector2 delta = panTarget - Position;
                delta /= 10;
                if(Math.Abs(delta.X) < 10 && Math.Abs(delta.Y) < 10)
                {
                    panning = false;
                }
                MoveCamera(delta);
            }
            if (following)
            {
                PanCamera(new Vector2((float)followBody.posX, (float)followBody.posY));
            }
        }

        public void PanCamera(Vector2 target)
        {
            panTarget = target;
            panning = true;
        }

        public void Follow(CelestialBody body)
        {
            if (body != null)
            {
                followBody = body;
                following = true;
                PanCamera(new Vector2((float)followBody.posX, (float)followBody.posY));
            }
            else
            {
                followBody = body;
                following = false;
                panning = false;
            }
        }
    }
}
