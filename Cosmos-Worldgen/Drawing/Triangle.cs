using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Drawing
{
    public struct Triangle
    {
        Vector2[] positions;
        public Vector2 A {
            get
            {
                return positions[0];
            }
        }
        public Vector2 B
        {
            get
            {
                return positions[1];
            }
        }
        public Vector2 C
        {
            get
            {
                return positions[2];
            }
        }
        public Vector2[] Points
        {
            get
            {
                return positions;
            }
        }

        public Triangle(Vector2[] positions)
        {
            this.positions = new Vector2[3];
            this.positions[0] = positions[0];
            this.positions[1] = positions[1];
            this.positions[2] = positions[2];
        }

        public Triangle(Vector2 A, Vector2 B, Vector2 C)
        {
            this.positions = new Vector2[3];
            this.positions[0] = A;
            this.positions[1] = B;
            this.positions[2] = C;
        }
    }
}
