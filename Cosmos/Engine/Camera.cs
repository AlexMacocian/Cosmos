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
        public Vector2D Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        public RectangleD VisibleArea { get; protected set; }
        public MatrixD Transform { get; protected set; }
        public static double OffsetX, OffsetY;
        public CelestialBody Following
        {
            get
            {
                return followBody;
            }
        }

        private float currentMouseWheelValue, previousMouseWheelValue, zoom, previousZoom;
        private bool panning = false, following = false;
        private Vector2D panTarget;
        private CelestialBody followBody;

        public Camera(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Zoom = 1f;
            Position = Vector2D.Zero;
            panTarget = Position;
        }

        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = MatrixD.Invert(Transform);

            var tl = Vector2D.Transform(Vector2D.Zero, inverseViewMatrix);
            var tr = Vector2D.Transform(new Vector2D(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2D.Transform(new Vector2D(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2D.Transform(new Vector2D(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2D(
                MathHelperD.Min(tl.X, MathHelperD.Min(tr.X, MathHelperD.Min(bl.X, br.X))),
                MathHelperD.Min(tl.Y, MathHelperD.Min(tr.Y, MathHelperD.Min(bl.Y, br.Y))));
            var max = new Vector2D(
                MathHelperD.Max(tl.X, MathHelperD.Max(tr.X, MathHelperD.Max(bl.X, br.X))),
                MathHelperD.Max(tl.Y, MathHelperD.Max(tr.Y, MathHelperD.Max(bl.Y, br.Y))));
            VisibleArea = new RectangleD(min.X, min.Y, (max.X - min.X), (max.Y - min.Y));
        }

        private void UpdateMatrix()
        {
            Transform = MatrixD.CreateTranslation(new Vector3D(-Position.X, -Position.Y, 0)) *
                    MatrixD.CreateScale(Zoom) *
                    MatrixD.CreateTranslation(new Vector3D(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void MoveCamera(Vector2D movePosition)
        {
            double posX = Position.X;
            double posY = Position.Y;
            double offX = movePosition.X;
            double offY = movePosition.Y;
            posX += Math.Ceiling(offX);
            posY += Math.Ceiling(offY);
            Vector2D newPosition = new Vector2D((float)Math.Round(posX), (float)Math.Round(posY));
            Position = newPosition;
            UpdateMatrix();
        }

        public void Tick()
        {
            if (panning)
            {
                Vector2D delta = panTarget - Position;
                delta /= 10;
                if(Math.Abs(delta.X) < 10 && Math.Abs(delta.Y) < 10)
                {
                    panning = false;
                }
                MoveCamera(delta);
            }
            if (following)
            {
                PanCamera(new Vector2D((float)followBody.posX, (float)followBody.posY));
            }
        }

        public void PanCamera(Vector2D target)
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
                PanCamera(new Vector2D((float)followBody.posX, (float)followBody.posY));
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
