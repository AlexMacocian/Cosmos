using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cosmos.Engine
{
    public struct RectangleD
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private double _x2;
        private double _y2;

        public Rectangle toRectangle()
        {
            Rectangle myReturn = new Rectangle((int)_x, (int)_y, (int)_width, (int)_height);
            return myReturn;
        }

        public RectangleD(double pX, double pY, double pWidth, double pHeight)
        {
            _x = pX;
            _y = pY;
            _width = pWidth;
            _height = pHeight;
            _x2 = pX + pWidth;
            _y2 = pY + pHeight;
        }

        public bool Contains(Vector2D pPoint)
        {
            if ((pPoint.X > this._x) && (pPoint.X < this._x2) && (pPoint.Y > this._y) && (pPoint.Y < this._y2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RectangleD Union(RectangleD rect1, RectangleD rect2)
        {
            RectangleD tempRect = new RectangleD();

            if (rect1._x < rect2._x)
            {
                tempRect._x = rect1._x;
            }
            else
            {
                tempRect._x = rect2._x;
            }

            if (rect1._x2 > rect2._x2)
            {
                tempRect._x2 = rect1._x2;
            }
            else
            {
                tempRect._x2 = rect2._x2;
            }

            tempRect._width = tempRect._x2 - tempRect._x;


            if (rect1._y < rect2._y)
            {
                tempRect._y = rect1._y;
            }
            else
            {
                tempRect._y = rect2._y;
            }

            if (rect1._y2 > rect2._y2)
            {
                tempRect._y2 = rect1._y2;
            }
            else
            {
                tempRect._y2 = rect2._y2;
            }

            tempRect._height = tempRect._y2 - tempRect._y;
            return tempRect;
        }
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                _x2 = _x + _width;
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                _y2 = _y + _height;
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                _x2 = _x + _width;
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                _y2 = _y + _height;
            }
        }

        public double X2
        {
            get { return _x2; }
        }

        public double Y2
        {
            get { return _y2; }
        }

        public RectangleD Duplicate()
        {
            return new RectangleD(X, Y, Width, Height);
        }

        public bool Intersects(Rectangle r)
        {
            return (X + Width >= r.X && Y + Height >= r.Y && X <= r.X + r.Width && Y <= r.Y + r.Height);
        }

        public bool Intersects(RectangleD r)
        {
            return (X + Width >= r.X && Y + Height >= r.Y && X <= r.X + r.Width && Y <= r.Y + r.Height);
        }

        public bool Contains(RectangleD r)
        {
            return (r.X + r.Width) < (X + Width) && (r.X) > (X) && (r.Y) > (Y) && (r.Y + r.Height) < (Y + Height);
        }

        public bool Contains(Rectangle r)
        {
            return (r.X + r.Width) < (X + Width) && (r.X) > (X) && (r.Y) > (Y) && (r.Y + r.Height) < (Y + Height);
        }
    }
}