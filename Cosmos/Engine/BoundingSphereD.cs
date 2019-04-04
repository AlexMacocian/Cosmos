using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    public struct BoundingSphereD : IEquatable<BoundingSphereD>
    {
        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    "Center( ", Center.DebugDisplayString, " ) \r\n",
                    "Radius( ", Radius.ToString(), " ) "
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// The sphere center.
        /// </summary>
        public Vector3D Center;

        /// <summary>
        /// The sphere radius.
        /// </summary>
        public double Radius;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs a bounding sphere with the specified center and radius.
        /// </summary>
        /// <param name="center">The sphere center.</param>
        /// <param name="radius">The sphere radius.</param>
        public BoundingSphereD(Vector3D center, double radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="BoundingSphereD"/> that contains a transformation of translation and scale from this sphere by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed <see cref="BoundingSphereD"/>.</returns>
        public BoundingSphereD Transform(MatrixD matrix)
        {
            BoundingSphereD sphere = new BoundingSphereD();
            sphere.Center = Vector3D.Transform(this.Center, matrix);
            sphere.Radius = this.Radius *
                (
                    (double)Math.Sqrt((double)Math.Max(
                        ((matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12)) + (matrix.M13 * matrix.M13),
                        Math.Max(
                            ((matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22)) + (matrix.M23 * matrix.M23),
                            ((matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32)) + (matrix.M33 * matrix.M33))
                        )
                    )
                );
            return sphere;
        }

        /// <summary>
        /// Creates a new <see cref="BoundingSphereD"/> that contains a transformation of translation and scale from this sphere by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed <see cref="BoundingSphereD"/> as an output parameter.</param>
        public void Transform(ref MatrixD matrix, out BoundingSphereD result)
        {
            result.Center = Vector3D.Transform(this.Center, matrix);
            result.Radius = this.Radius *
                (
                    (double)Math.Sqrt((double)Math.Max(
                        ((matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12)) + (matrix.M13 * matrix.M13),
                        Math.Max(
                            ((matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22)) + (matrix.M23 * matrix.M23),
                            ((matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32)) + (matrix.M33 * matrix.M33))
                        )
                    )
                );
        }

        /// <summary>
        /// Test if a bounding box is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref BoundingBoxD box, out ContainmentType result)
        {
            result = this.Contains(box);
        }

        /// <summary>
        /// Test if a sphere is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
        {
            result = Contains(sphere);
        }

        /// <summary>
        /// Test if a point is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="point">The vector in 3D-space for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public void Contains(ref Vector3D point, out ContainmentType result)
        {
            result = Contains(point);
        }

        /// <summary>
        /// Test if a bounding box is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(BoundingBoxD box)
        {
            // Check if all corners are in sphere.
            bool inside = true;
            foreach (Vector3D corner in box.GetCorners())
            {
                if (this.Contains(corner) == ContainmentType.Disjoint)
                {
                    inside = false;
                    break;
                }
            }

            if (inside)
            {
                return ContainmentType.Contains;
            }

            // Check if the distance from sphere center to cube face is less than radius.
            double dmin = 0;

            if (Center.X < box.Min.X)
            {
                dmin += (Center.X - box.Min.X) * (Center.X - box.Min.X);
            }
            else if (Center.X > box.Max.X)
            {
                dmin += (Center.X - box.Max.X) * (Center.X - box.Max.X);
            }

            if (Center.Y < box.Min.Y)
            {
                dmin += (Center.Y - box.Min.Y) * (Center.Y - box.Min.Y);
            }
            else if (Center.Y > box.Max.Y)
            {
                dmin += (Center.Y - box.Max.Y) * (Center.Y - box.Max.Y);
            }

            if (Center.Z < box.Min.Z)
            {
                dmin += (Center.Z - box.Min.Z) * (Center.Z - box.Min.Z);
            }
            else if (Center.Z > box.Max.Z)
            {
                dmin += (Center.Z - box.Max.Z) * (Center.Z - box.Max.Z);
            }

            if (dmin <= Radius * Radius)
            {
                return ContainmentType.Intersects;
            }

            // Else disjoint
            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Test if a frustum is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <param name="result">The containment type as an output parameter.</param>
        public ContainmentType Contains(BoundingFrustumD frustum)
        {
            // Check if all corners are in sphere.
            bool inside = true;

            Vector3D[] corners = frustum.GetCorners();
            foreach (Vector3D corner in corners)
            {
                if (this.Contains(corner) == ContainmentType.Disjoint)
                {
                    inside = false;
                    break;
                }
            }
            if (inside)
            {
                return ContainmentType.Contains;
            }

            // Check if the distance from sphere center to frustrum face is less than radius.
            double dmin = 0;
            // TODO : calcul dmin

            if (dmin <= Radius * Radius)
            {
                return ContainmentType.Intersects;
            }

            // Else disjoint
            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Test if a sphere is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(BoundingSphereD sphere)
        {
            double sqDistance;
            Vector3D.DistanceSquared(ref sphere.Center, ref Center, out sqDistance);

            if (sqDistance > (sphere.Radius + Radius) * (sphere.Radius + Radius))
            {
                return ContainmentType.Disjoint;
            }
            else if (sqDistance <= (Radius - sphere.Radius) * (Radius - sphere.Radius))
            {
                return ContainmentType.Contains;
            }
            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Test if a point is fully inside, outside, or just intersecting the sphere.
        /// </summary>
        /// <param name="point">The vector in 3D-space for testing.</param>
        /// <returns>The containment type.</returns>
        public ContainmentType Contains(Vector3D point)
        {
            double sqRadius = Radius * Radius;
            double sqDistance;
            Vector3D.DistanceSquared(ref point, ref Center, out sqDistance);

            if (sqDistance > sqRadius)
            {
                return ContainmentType.Disjoint;
            }
            else if (sqDistance < sqRadius)
            {
                return ContainmentType.Contains;
            }
            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="BoundingSphereD"/>.
        /// </summary>
        /// <param name="other">The <see cref="BoundingSphereD"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(BoundingSphereD other)
        {
            return (Center == other.Center &&
                    Radius == other.Radius);
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphereD"/> that can contain a specified <see cref="BoundingBoxD"/>.
        /// </summary>
        /// <param name="box">The box to create the sphere from.</param>
        /// <returns>The new <see cref="BoundingSphereD"/>.</returns>
        public static BoundingSphereD CreateFromBoundingBox(BoundingBoxD box)
        {
            BoundingSphereD result;
            CreateFromBoundingBox(ref box, out result);
            return result;
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphereD"/> that can contain a specified <see cref="BoundingBoxD"/>.
        /// </summary>
        /// <param name="box">The box to create the sphere from.</param>
        /// <param name="result">The new <see cref="BoundingSphereD"/> as an output parameter.</param>
        public static void CreateFromBoundingBox(ref BoundingBoxD box, out BoundingSphereD result)
        {
            // Find the center of the box.
            Vector3D center = new Vector3D(
                (box.Min.X + box.Max.X) / 2.0f,
                (box.Min.Y + box.Max.Y) / 2.0f,
                (box.Min.Z + box.Max.Z) / 2.0f
            );

            // Find the distance between the center and one of the corners of the box.
            double radius = Vector3D.Distance(center, box.Max);

            result = new BoundingSphereD(center, radius);
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphereD"/> that can contain a specified <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="frustum">The frustum to create the sphere from.</param>
        /// <returns>The new <see cref="BoundingSphereD"/>.</returns>
        public static BoundingSphereD CreateFromFrustum(BoundingFrustumD frustum)
        {
            return CreateFromPoints(frustum.GetCorners());
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphereD"/> that can contain a specified list of points in 3D-space.
        /// </summary>
        /// <param name="points">List of point to create the sphere from.</param>
        /// <returns>The new <see cref="BoundingSphereD"/>.</returns>
        public static BoundingSphereD CreateFromPoints(IEnumerable<Vector3D> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }

            // From "Real-Time Collision Detection" (Page 89)

            Vector3D minx = new Vector3D(double.MaxValue, double.MaxValue, double.MaxValue);
            Vector3D maxx = -minx;
            Vector3D miny = minx;
            Vector3D maxy = -minx;
            Vector3D minz = minx;
            Vector3D maxz = -minx;

            // Find the most extreme points along the principle axis.
            int numPoints = 0;
            foreach (Vector3D pt in points)
            {
                numPoints += 1;

                if (pt.X < minx.X)
                {
                    minx = pt;
                }
                if (pt.X > maxx.X)
                {
                    maxx = pt;
                }
                if (pt.Y < miny.Y)
                {
                    miny = pt;
                }
                if (pt.Y > maxy.Y)
                {
                    maxy = pt;
                }
                if (pt.Z < minz.Z)
                {
                    minz = pt;
                }
                if (pt.Z > maxz.Z)
                {
                    maxz = pt;
                }
            }

            if (numPoints == 0)
            {
                throw new ArgumentException(
                    "You should have at least one point in points."
                );
            }

            double sqDistX = Vector3D.DistanceSquared(maxx, minx);
            double sqDistY = Vector3D.DistanceSquared(maxy, miny);
            double sqDistZ = Vector3D.DistanceSquared(maxz, minz);

            // Pick the pair of most distant points.
            Vector3D min = minx;
            Vector3D max = maxx;
            if (sqDistY > sqDistX && sqDistY > sqDistZ)
            {
                max = maxy;
                min = miny;
            }
            if (sqDistZ > sqDistX && sqDistZ > sqDistY)
            {
                max = maxz;
                min = minz;
            }

            Vector3D center = (min + max) * 0.5f;
            double radius = Vector3D.Distance(max, center);

            // Test every point and expand the sphere.
            // The current bounding sphere is just a good approximation and may not enclose all points.
            // From: Mathematics for 3D Game Programming and Computer Graphics, Eric Lengyel, Third Edition.
            // Page 218
            double sqRadius = radius * radius;
            foreach (Vector3D pt in points)
            {
                Vector3D diff = (pt - center);
                double sqDist = diff.LengthSquared();
                if (sqDist > sqRadius)
                {
                    double distance = (double)Math.Sqrt(sqDist); // equal to diff.Length();
                    Vector3D direction = diff / distance;
                    Vector3D G = center - radius * direction;
                    center = (G + pt) / 2;
                    radius = Vector3D.Distance(pt, center);
                    sqRadius = radius * radius;
                }
            }

            return new BoundingSphereD(center, radius);
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphereD"/> that can contain two spheres.
        /// </summary>
        /// <param name="original">First sphere.</param>
        /// <param name="additional">Second sphere.</param>
        /// <returns>The new <see cref="BoundingSphereD"/>.</returns>
        public static BoundingSphereD CreateMerged(BoundingSphereD original, BoundingSphereD additional)
        {
            BoundingSphereD result;
            CreateMerged(ref original, ref additional, out result);
            return result;
        }

        /// <summary>
        /// Creates the smallest <see cref="BoundingSphereD"/> that can contain two spheres.
        /// </summary>
        /// <param name="original">First sphere.</param>
        /// <param name="additional">Second sphere.</param>
        /// <param name="result">The new <see cref="BoundingSphereD"/> as an output parameter.</param>
        public static void CreateMerged(
            ref BoundingSphereD original,
            ref BoundingSphereD additional,
            out BoundingSphereD result
        )
        {
            Vector3D ocenterToaCenter = Vector3D.Subtract(additional.Center, original.Center);
            double distance = ocenterToaCenter.Length();

            // Intersect
            if (distance <= original.Radius + additional.Radius)
            {
                // Original contains additional.
                if (distance <= original.Radius - additional.Radius)
                {
                    result = original;
                    return;
                }

                // Additional contains original.
                if (distance <= additional.Radius - original.Radius)
                {
                    result = additional;
                    return;
                }
            }

            // Else find center of new sphere and radius
            double leftRadius = Math.Max(original.Radius - distance, additional.Radius);
            double Rightradius = Math.Max(original.Radius + distance, additional.Radius);

            // oCenterToResultCenter
            ocenterToaCenter = ocenterToaCenter +
                (
                    ((leftRadius - Rightradius) / (2 * ocenterToaCenter.Length()))
                    * ocenterToaCenter
                );

            result = new BoundingSphereD();
            result.Center = original.Center + ocenterToaCenter;
            result.Radius = (leftRadius + Rightradius) / 2;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBoxD"/> intersects with this sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <returns><c>true</c> if <see cref="BoundingBoxD"/> intersects with this sphere; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingBoxD box)
        {
            return box.Intersects(this);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBoxD"/> intersects with this sphere.
        /// </summary>
        /// <param name="box">The box for testing.</param>
        /// <param name="result"><c>true</c> if <see cref="BoundingBoxD"/> intersects with this sphere; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref BoundingBoxD box, out bool result)
        {
            box.Intersects(ref this, out result);
        }

        public bool Intersects(BoundingFrustumD frustum)
        {
            return frustum.Intersects(this);
        }

        /// <summary>
        /// Gets whether or not the other <see cref="BoundingSphereD"/> intersects with this sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <returns><c>true</c> if other <see cref="BoundingSphereD"/> intersects with this sphere; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingSphereD sphere)
        {
            bool result;
            Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not the other <see cref="BoundingSphereD"/> intersects with this sphere.
        /// </summary>
        /// <param name="sphere">The other sphere for testing.</param>
        /// <param name="result"><c>true</c> if other <see cref="BoundingSphereD"/> intersects with this sphere; <c>false</c> otherwise. As an output parameter.</param>
        public void Intersects(ref BoundingSphereD sphere, out bool result)
        {
            double sqDistance;
            Vector3D.DistanceSquared(ref sphere.Center, ref Center, out sqDistance);
            result = !(sqDistance > (sphere.Radius + Radius) * (sphere.Radius + Radius));
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="RayD"/> intersects with this sphere.
        /// </summary>
        /// <param name="ray">The ray for testing.</param>
        /// <returns>Distance of ray intersection or <c>null</c> if there is no intersection.</returns>
        public double? Intersects(RayD ray)
        {
            return ray.Intersects(this);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="RayD"/> intersects with this sphere.
        /// </summary>
        /// <param name="ray">The ray for testing.</param>
        /// <param name="result">Distance of ray intersection or <c>null</c> if there is no intersection as an output parameter.</param>
        public void Intersects(ref RayD ray, out double? result)
        {
            ray.Intersects(ref this, out result);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="PlaneD"/> intersects with this sphere.
        /// </summary>
        /// <param name="plane">The plane for testing.</param>
        /// <returns>Type of intersection.</returns>
        public PlaneIntersectionTypeD Intersects(PlaneD plane)
        {
            PlaneIntersectionTypeD result = default(PlaneIntersectionTypeD);
            // TODO: We might want to inline this for performance reasons.
            this.Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="PlaneD"/> intersects with this sphere.
        /// </summary>
        /// <param name="plane">The plane for testing.</param>
        /// <param name="result">Type of intersection as an output parameter.</param>
        public void Intersects(ref PlaneD plane, out PlaneIntersectionTypeD result)
        {
            double distance = default(double);
            // TODO: We might want to inline this for performance reasons.
            Vector3D.Dot(ref plane.Normal, ref this.Center, out distance);
            distance += plane.D;
            if (distance > this.Radius)
            {
                result = PlaneIntersectionTypeD.Front;
            }
            else if (distance < -this.Radius)
            {
                result = PlaneIntersectionTypeD.Back;
            }
            else
            {
                result = PlaneIntersectionTypeD.Intersecting;
            }
        }

        #endregion

        #region Public Static Operators and Override Methods

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is BoundingSphereD) && Equals((BoundingSphereD)obj);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="BoundingSphereD"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="BoundingSphereD"/>.</returns>
        public override int GetHashCode()
        {
            return this.Center.GetHashCode() + this.Radius.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="BoundingSphereD"/> in the format:
        /// {Center:[<see cref="Center"/>] Radius:[<see cref="Radius"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="BoundingSphereD"/>.</returns>
        public override string ToString()
        {
            return (
                "{Center:" + Center.ToString() +
                " Radius:" + Radius.ToString() +
                "}"
            );
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingSphereD"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingSphereD"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="BoundingSphereD"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(BoundingSphereD a, BoundingSphereD b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingSphereD"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingSphereD"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="BoundingSphereD"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(BoundingSphereD a, BoundingSphereD b)
        {
            return !a.Equals(b);
        }

        #endregion
    }
}
