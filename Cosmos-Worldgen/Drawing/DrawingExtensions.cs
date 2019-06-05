using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.WorldGen.Drawing
{
    public static class DrawingExtensions
    {
        public static void DrawFilledTriangle(this SpriteBatch spriteBatch, Vector2[] points, Color color, float thickness = 1)
        {
            points = points.OrderBy(v => v.Y).ToArray<Vector2>();

            Vector2 pos1 = points[0];
            Vector2 pos2 = points[1];
            Vector2 pos3 = points[2];

            float dx1, dx2, dx3;
            if (pos2.Y - pos1.Y > 0)
                dx1 = (pos2.X - pos1.X) / (pos2.Y - pos1.Y);
            else
                dx1 = 0;
            if (pos3.Y - pos1.Y > 0)
                dx2 = (pos3.X - pos1.X) / (pos3.Y - pos1.Y);
            else
                dx2 = 0;
            if (pos3.Y - pos2.Y > 0)
                dx3 = (pos3.X - pos2.X) / (pos3.Y - pos2.Y);
            else
                dx3 = 0;

            Vector2 e = pos1;
            Vector2 s = pos1;
            if (dx1 > dx2)
            {
                for (; s.Y <= pos2.Y; s.Y++, e.Y++, s.X += dx2, e.X += dx1)
                    spriteBatch.DrawLine(new Vector2(s.X, s.Y), new Vector2(e.X, s.Y), color, thickness);
                e = pos2;
                for (; s.Y <= pos3.Y; s.Y++, e.Y++, s.X += dx2, e.X += dx3)
                    spriteBatch.DrawLine(new Vector2(s.X, s.Y), new Vector2(e.X, s.Y), color, thickness);
            }
            else
            {
                for (; s.Y <= pos2.Y; s.Y++, e.Y++, s.X += dx1, e.X += dx2)
                    spriteBatch.DrawLine(new Vector2(s.X, s.Y), new Vector2(e.X, s.Y), color, thickness);
                s = pos2;
                for (; s.Y <= pos3.Y; s.Y++, e.Y++, s.X += dx3, e.X += dx2)
                    spriteBatch.DrawLine(new Vector2(s.X, s.Y), new Vector2(e.X, s.Y), color, thickness);
            }

        }

        public static void DrawFilledTriangle(this SpriteBatch spriteBatch, Triangle triangle, Color color, float thickness = 1)
        {
            DrawFilledTriangle(spriteBatch, triangle.Points, color, thickness);
        }

        public static void DrawFilledPolygon(this SpriteBatch spriteBatch, Polygon polygon, Color color, float thickness = 1)
        {
            Triangle[] triangles = polygon.Triangulate();
            foreach(Triangle triangle in triangles)
            {
                DrawFilledTriangle(spriteBatch, triangle, color, thickness);
            }
        }

        public static Triangle[] Triangulate(this Polygon polygon)
        {
            List<Tuple<int, int, int>> triangleIndices = new List<Tuple<int, int, int>>();
            double[,] table = new double[polygon.Vertices.Length, polygon.Vertices.Length];
            int[,] s = new int[polygon.Vertices.Length, polygon.Vertices.Length];
            for (int i = polygon.Vertices.Length - 1; i >= 0; i--)
            {
                for (int j = i; j < polygon.Vertices.Length; j++)
                {
                    if (j < i + 2)
                    {
                        table[i,j] = 0;
                    }
                    else
                    {
                        table[i,j] = Double.MaxValue;
                        for (int k = i + 1; k < j; k++)
                        {
                            double val = table[i,k] + table[k,j] + Cost(polygon.Vertices, i, j, k);
                            if (val < table[i,j])
                            {
                                table[i,j] = val;
                                s[i,j] = k;
                            }
                        }
                    }
                }
            }
            RecursivelyBuildTriangles(s, 0, polygon.Vertices.Length - 1, ref triangleIndices);
            Triangle[] triangles = new Triangle[triangleIndices.Count];
            for(int i = 0; i < triangleIndices.Count; i++)
            {
                triangles[i] = new Triangle(polygon.Vertices[triangleIndices[i].Item1], polygon.Vertices[triangleIndices[i].Item2], polygon.Vertices[triangleIndices[i].Item3]);
            }
            return triangles;
        } 

        private static double Cost(Vector2[] points, int i, int j, int k)
        {
            Vector2 p1 = points[i], p2 = points[j], p3 = points[k];
            return Vector2.Distance(p1, p2) + Vector2.Distance(p2, p3) + Vector2.Distance(p3, p1);
        }

        private static void RecursivelyBuildTriangles(int[,] s, int i, int j, ref List<Tuple<int, int, int>> triangles)
        {
            if (j - i < 2)
                return;

            RecursivelyBuildTriangles(s, i, s[i, j], ref triangles);        
            Tuple<int, int, int> triangle = new Tuple<int, int, int>(i, s[i, j], j);
            triangles.Add(triangle);
            RecursivelyBuildTriangles(s, s[i, j], j, ref triangles);
        }
    }
}
