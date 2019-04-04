using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Content
{
    class Circle
    {
        Texture2D circleText;

        public Circle(GraphicsDevice dev)
        {
            this.CircleText = createCircleText(100, dev);
        }

        public int Radius
        {
            get
            {
                return 50;
            }
        }

        public int Size
        {
            get
            {
                return 100;
            }
        }

        public Texture2D CircleText
        {
            get
            {
                return circleText;
            }

            set
            {
                circleText = value;
            }
        }

        Texture2D createCircleText(int radius, GraphicsDevice GraphicsDevice)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
