using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    public struct RayD : IEquatable<RayD>
    {
        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    "Pos( ", Position.DebugDisplayString, " ) \r\n",
                    "Dir( ", Direction.DebugDisplayString, " )"
                );
            }
        }

        #endregion

        #region Public Fields

        public Vector3D Position;
        public Vector3D Direction;

        #endregion


        #region Public Constructors

        public RayD(Vector3D position, Vector3D direction)
        {
            Position = position;
            Direction = direction;
        }

        #endregion


        #region Public Methods

        public override bool Equals(object obj)
        {
            return (obj is RayD) && Equals((RayD)obj);
        }


        public bool Equals(RayD other)
        {
            return (this.Position.Equals(other.Position) &&
                    this.Direction.Equals(other.Direction));
        }


        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        // Adapted from http://www.scratchapixel.com/lessons/3d-basic-lessons/lesson-7-intersecting-simple-shapes/ray-box-intersection/
        public double? Intersects(BoundingBoxD box)
        {
            double? tMin = null, tMax = null;

            if (MathHelperD.WithinEpsilon(Direction.X, 0.0f))
            {
                if (Position.X < box.Min.X || Position.X > box.Max.X)
                {
                    return null;
                }
            }
            else
            {
                tMin = (box.Min.X - Position.X) / Direction.X;
                tMax = (box.Max.X - Position.X) / Direction.X;

                if (tMin > tMax)
                {
                    double? temp = tMin;
                    tMin = tMax;
                    tMax = temp;
                }
            }

            if (MathHelperD.WithinEpsilon(Direction.Y, 0.0f))
            {
                if (Position.Y < box.Min.Y || Position.Y > box.Max.Y)
                {
                    return null;
                }
            }
            else
            {
                double tMinY = (box.Min.Y - Position.Y) / Direction.Y;
                double tMaxY = (box.Max.Y - Position.Y) / Direction.Y;

                if (tMinY > tMaxY)
                {
                    double temp = tMinY;
                    tMinY = tMaxY;
                    tMaxY = temp;
                }

                if ((tMin.HasValue && tMin > tMaxY) ||
                    (tMax.HasValue && tMinY > tMax))
                {
                    return null;
                }

                if (!tMin.HasValue || tMinY > tMin) tMin = tMinY;
                if (!tMax.HasValue || tMaxY < tMax) tMax = tMaxY;
            }

            if (MathHelperD.WithinEpsilon(Direction.Z, 0.0f))
            {
                if (Position.Z < box.Min.Z || Position.Z > box.Max.Z)
                {
                    return null;
                }
            }
            else
            {
                double tMinZ = (box.Min.Z - Position.Z) / Direction.Z;
                double tMaxZ = (box.Max.Z - Position.Z) / Direction.Z;

                if (tMinZ > tMaxZ)
                {
                    double temp = tMinZ;
                    tMinZ = tMaxZ;
                    tMaxZ = temp;
                }

                if ((tMin.HasValue && tMin > tMaxZ) ||
                    (tMax.HasValue && tMinZ > tMax))
                {
                    return null;
                }

                if (!tMin.HasValue || tMinZ > tMin) tMin = tMinZ;
                if (!tMax.HasValue || tMaxZ < tMax) tMax = tMaxZ;
            }

            /* Having a positive tMin and a negative tMax means the ray is inside the
			 * box we expect the intesection distance to be 0 in that case.
			 */
            if ((tMin.HasValue && tMin < 0) && tMax > 0) return 0;

            /* A negative tMin means that the intersection point is behind the ray's
			 * origin. We discard these as not hitting the AABB.
			 */
            if (tMin < 0) return null;

            return tMin;
        }


        public void Intersects(ref BoundingBoxD box, out double? result)
        {
            result = Intersects(box);
        }

        public double? Intersects(BoundingSphereD sphere)
        {
            double? result;
            Intersects(ref sphere, out result);
            return result;
        }

        public double? Intersects(PlaneD plane)
        {
            double? result;
            Intersects(ref plane, out result);
            return result;
        }

        public double? Intersects(BoundingFrustumD frustum)
        {
            double? result;
            frustum.Intersects(ref this, out result);
            return result;
        }

        public void Intersects(ref PlaneD plane, out double? result)
        {
            double den = Vector3D.Dot(Direction, plane.Normal);
            if (Math.Abs(den) < 0.00001f)
            {
                result = null;
                return;
            }

            result = (-plane.D - Vector3D.Dot(plane.Normal, Position)) / den;

            if (result < 0.0f)
            {
                if (result < -0.00001f)
                {
                    result = null;
                    return;
                }

                result = 0.0f;
            }
        }

        public void Intersects(ref BoundingSphereD sphere, out double? result)
        {
            // Find the vector between where the ray starts the the sphere's center.
            Vector3D difference = sphere.Center - this.Position;

            double differenceLengthSquared = difference.LengthSquared();
            double sphereRadiusSquared = sphere.Radius * sphere.Radius;

            double distanceAlongRay;

            /* If the distance between the ray start and the sphere's center is less than
			 * the radius of the sphere, it means we've intersected. Checking the
			 * LengthSquared is faster.
			 */
            if (differenceLengthSquared < sphereRadiusSquared)
            {
                result = 0.0f;
                return;
            }

            Vector3D.Dot(ref this.Direction, ref difference, out distanceAlongRay);
            // If the ray is pointing away from the sphere then we don't ever intersect.
            if (distanceAlongRay < 0)
            {
                result = null;
                return;
            }

            /* Next we kinda use Pythagoras to check if we are within the bounds of the
			 * sphere.
			 * if x = radius of sphere
			 * if y = distance between ray position and sphere centre
			 * if z = the distance we've travelled along the ray
			 * if x^2 + z^2 - y^2 < 0, we do not intersect
			 */
            double dist = (
                sphereRadiusSquared +
                (distanceAlongRay * distanceAlongRay) -
                differenceLengthSquared
            );

            result = (dist < 0) ? null : distanceAlongRay - (double?)Math.Sqrt(dist);
        }

        #endregion

        #region Public Static Methods

        public static bool operator !=(RayD a, RayD b)
        {
            return !a.Equals(b);
        }


        public static bool operator ==(RayD a, RayD b)
        {
            return a.Equals(b);
        }


        public override string ToString()
        {
            return (
                "{{Position:" + Position.ToString() +
                " Direction:" + Direction.ToString() +
                "}}"
            );
        }

        #endregion
    }
}
