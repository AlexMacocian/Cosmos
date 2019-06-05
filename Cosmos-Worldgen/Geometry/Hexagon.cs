using Cosmos.WorldGen.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Geometry
{
    class Hexagon
    {
        private static double SQRT3 = 1.73205080757;
        private Vector2 position;
        private float size;
        private Vector2[] corners;
        private Polygon polygon;

        public float Size
        {
            get => size;
        }
        public Vector2 Position
        {
            get => position; set => position = value;
        }
        public float Width
        {
            get
            {
                return size * 2;
            }
        }
        public float Height
        {
            get
            {
                return (float)(size * SQRT3);
            }
        }
        public Polygon Polygon { get => polygon; }

        public Hexagon(Vector2 position, float size)
        {
            this.position = position;
            this.size = size;
            corners = new Vector2[6];
            for(int i = 0; i < 6; i++)
            {
                corners[i] = Hex_Corner(position, size, i);
            }
            polygon = new Polygon(corners);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color, Color borderColor, float borderThickness = 1)
        {
            spriteBatch.DrawFilledPolygon(polygon, color);
            for(int i = 0; i < 6; i++)
            {
                spriteBatch.DrawLine(corners[i % 6], corners[(i + 1) % 6], borderColor, borderThickness);
            }
        }

        private static Vector2 Hex_Corner(Vector2 position, float size, int i)
        {
            var angle_deg = 60 * i;
            var angle_rad = Math.PI / 180 * angle_deg;
            return new Vector2(position.X + (float)(size * Math.Cos(angle_rad)), position.Y + (float)(size * Math.Sin(angle_rad)));
        }
    }
}
