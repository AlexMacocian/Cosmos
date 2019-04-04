using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    public class BoundingFrustumD : IEquatable<BoundingFrustumD>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="Matrix"/> of the frustum.
        /// </summary>
        public MatrixD Matrix
        {
            get
            {
                return this.matrix;
            }
            set
            {
                /* FIXME: The odds are the planes will be used a lot more often than
				 * the matrix is updated, so this should help performance. I hope. ;)
				 */
                this.matrix = value;
                this.CreatePlanes();
                this.CreateCorners();
            }
        }

        /// <summary>
        /// Gets the near plane of the frustum.
        /// </summary>
        public PlaneD Near
        {
            get
            {
                return this.planes[0];
            }
        }

        /// <summary>
        /// Gets the far plane of the frustum.
        /// </summary>
        public PlaneD Far
        {
            get
            {
                return this.planes[1];
            }
        }

        /// <summary>
        /// Gets the left plane of the frustum.
        /// </summary>
        public PlaneD Left
        {
            get
            {
                return this.planes[2];
            }
        }

        /// <summary>
        /// Gets the right plane of the frustum.
        /// </summary>
        public PlaneD Right
        {
            get
            {
                return this.planes[3];
            }
        }

        /// <summary>
        /// Gets the top plane of the frustum.
        /// </summary>
        public PlaneD Top
        {
            get
            {
                return this.planes[4];
            }
        }

        /// <summary>
        /// Gets the bottom plane of the frustum.
        /// </summary>
        public PlaneD Bottom
        {
            get
            {
                return this.planes[5];
            }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    "Near( ", planes[0].DebugDisplayString, " ) \r\n",
                    "Far( ", planes[1].DebugDisplayString, " ) \r\n",
                    "Left( ", planes[2].DebugDisplayString, " ) \r\n",
                    "Right( ", planes[3].DebugDisplayString, " ) \r\n",
                    "Top( ", planes[4].DebugDisplayString, " ) \r\n",
                    "Bottom( ", planes[5].DebugDisplayString, " ) "
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// The number of corner points in the frustum.
        /// </summary>
        public const int CornerCount = 8;

        #endregion

        #region Private Fields

        private MatrixD matrix;
        private readonly Vector3D[] corners = new Vector3D[CornerCount];
        private readonly PlaneD[] planes = new PlaneD[PlaneCount];

        /// <summary>
        /// The number of planes in the frustum.
        /// </summary>
        private const int PlaneCount = 6;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs the frustum by extracting the view planes from a matrix.
        /// </summary>
        /// <param name="value">Combined matrix which usually is (View * Projection).</param>
        public BoundingFrustumD(MatrixD value)
        {
            this.matrix = value;
            this.CreatePlanes();
            this.CreateCorners();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="frustum">A <see cref="BoundingFrustumD"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingFrustumD"/>.</returns>
        public ContainmentType Contains(BoundingFrustumD frustum)
        {
            if (this == frustum)
            {
                return ContainmentType.Contains;
            }
            bool intersects = false;
            for (int i = 0; i < PlaneCount; i += 1)
            {
                PlaneIntersectionTypeD planeIntersectionType;
                frustum.Intersects(ref planes[i], out planeIntersectionType);
                if (planeIntersectionType == PlaneIntersectionTypeD.Front)
                {
                    return ContainmentType.Disjoint;
                }
                else if (planeIntersectionType == PlaneIntersectionTypeD.Intersecting)
                {
                    intersects = true;
                }
            }
            return intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingBoxD"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBoxD"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingBoxD"/>.</returns>
        public ContainmentType Contains(BoundingBoxD box)
        {
            ContainmentType result = default(ContainmentType);
            this.Contains(ref box, out result);
            return result;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingBoxD"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBoxD"/> for testing.</param>
        /// <param name="result">Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingBoxD"/> as an output parameter.</param>
        public void Contains(ref BoundingBoxD box, out ContainmentType result)
        {
            bool intersects = false;
            for (int i = 0; i < PlaneCount; i += 1)
            {
                PlaneIntersectionTypeD planeIntersectionType = default(PlaneIntersectionTypeD);
                box.Intersects(ref this.planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                    case PlaneIntersectionTypeD.Front:
                        result = ContainmentType.Disjoint;
                        return;
                    case PlaneIntersectionTypeD.Intersecting:
                        intersects = true;
                        break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingSphereD"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphereD"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingSphereD"/>.</returns>
        public ContainmentType Contains(BoundingSphereD sphere)
        {
            ContainmentType result = default(ContainmentType);
            this.Contains(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingSphereD"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphereD"/> for testing.</param>
        /// <param name="result">Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="BoundingSphereD"/> as an output parameter.</param>
        public void Contains(ref BoundingSphereD sphere, out ContainmentType result)
        {
            bool intersects = false;
            for (int i = 0; i < PlaneCount; i += 1)
            {
                PlaneIntersectionTypeD planeIntersectionType = default(PlaneIntersectionTypeD);

                // TODO: We might want to inline this for performance reasons.
                sphere.Intersects(ref this.planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                    case PlaneIntersectionTypeD.Front:
                        result = ContainmentType.Disjoint;
                        return;
                    case PlaneIntersectionTypeD.Intersecting:
                        intersects = true;
                        break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="point">A <see cref="Vector3D"/> for testing.</param>
        /// <returns>Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="Vector3D"/>.</returns>
        public ContainmentType Contains(Vector3D point)
        {
            ContainmentType result = default(ContainmentType);
            this.Contains(ref point, out result);
            return result;
        }

        /// <summary>
        /// Containment test between this <see cref="BoundingFrustumD"/> and specified <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="point">A <see cref="Vector3D"/> for testing.</param>
        /// <param name="result">Result of testing for containment between this <see cref="BoundingFrustumD"/> and specified <see cref="Vector3D"/> as an output parameter.</param>
        public void Contains(ref Vector3D point, out ContainmentType result)
        {
            bool intersects = false;
            for (int i = 0; i < PlaneCount; i += 1)
            {
                double classifyPoint = (
                    (point.X * planes[i].Normal.X) +
                    (point.Y * planes[i].Normal.Y) +
                    (point.Z * planes[i].Normal.Z) +
                    planes[i].D
                );
                if (classifyPoint > 0)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
                else if (classifyPoint == 0)
                {
                    intersects = true;
                    break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        /// <summary>
        /// Returns a copy of internal corners array.
        /// </summary>
        /// <returns>The array of corners.</returns>
        public Vector3D[] GetCorners()
        {
            return (Vector3D[])this.corners.Clone();
        }

        /// <summary>
        /// Returns a copy of internal corners array.
        /// </summary>
        /// <param name="corners">The array which values will be replaced to corner values of this instance. It must have size of <see cref="BoundingFrustumD.CornerCount"/>.</param>
        public void GetCorners(Vector3D[] corners)
        {
            if (corners == null)
            {
                throw new ArgumentNullException("corners");
            }
            if (corners.Length < CornerCount)
            {
                throw new ArgumentOutOfRangeException("corners");
            }

            this.corners.CopyTo(corners, 0);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingFrustumD"/> intersects with this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="frustum">An other <see cref="BoundingFrustumD"/> for intersection test.</param>
        /// <returns><c>true</c> if other <see cref="BoundingFrustumD"/> intersects with this <see cref="BoundingFrustumD"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingFrustumD frustum)
        {
            return (Contains(frustum) != ContainmentType.Disjoint);
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBoxD"/> intersects with this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBoxD"/> for intersection test.</param>
        /// <returns><c>true</c> if specified <see cref="BoundingBoxD"/> intersects with this <see cref="BoundingFrustumD"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingBoxD box)
        {
            bool result = false;
            this.Intersects(ref box, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingBoxD"/> intersects with this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="box">A <see cref="BoundingBoxD"/> for intersection test.</param>
        /// <param name="result"><c>true</c> if specified <see cref="BoundingBoxD"/> intersects with this <see cref="BoundingFrustumD"/>; <c>false</c> otherwise as an output parameter.</param>
        public void Intersects(ref BoundingBoxD box, out bool result)
        {
            ContainmentType containment = default(ContainmentType);
            this.Contains(ref box, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingSphereD"/> intersects with this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphereD"/> for intersection test.</param>
        /// <returns><c>true</c> if specified <see cref="BoundingSphereD"/> intersects with this <see cref="BoundingFrustumD"/>; <c>false</c> otherwise.</returns>
        public bool Intersects(BoundingSphereD sphere)
        {
            bool result = default(bool);
            this.Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not a specified <see cref="BoundingSphereD"/> intersects with this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="sphere">A <see cref="BoundingSphereD"/> for intersection test.</param>
        /// <param name="result"><c>true</c> if specified <see cref="BoundingSphereD"/> intersects with this <see cref="BoundingFrustumD"/>; <c>false</c> otherwise as an output parameter.</param>
        public void Intersects(ref BoundingSphereD sphere, out bool result)
        {
            ContainmentType containment = default(ContainmentType);
            this.Contains(ref sphere, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Gets type of intersection between specified <see cref="PlaneD"/> and this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="plane">A <see cref="PlaneD"/> for intersection test.</param>
        /// <returns>A plane intersection type.</returns>
        public PlaneIntersectionTypeD Intersects(PlaneD plane)
        {
            PlaneIntersectionTypeD result;
            Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        /// Gets type of intersection between specified <see cref="PlaneD"/> and this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="plane">A <see cref="PlaneD"/> for intersection test.</param>
        /// <param name="result">A plane intersection type as an output parameter.</param>
        public void Intersects(ref PlaneD plane, out PlaneIntersectionTypeD result)
        {
            result = plane.Intersects(ref corners[0]);
            for (int i = 1; i < corners.Length; i += 1)
            {
                if (plane.Intersects(ref corners[i]) != result)
                {
                    result = PlaneIntersectionTypeD.Intersecting;
                }
            }
        }

        /// <summary>
        /// Gets the distance of intersection of <see cref="RayD"/> and this <see cref="BoundingFrustumD"/> or null if no intersection happens.
        /// </summary>
        /// <param name="ray">A <see cref="RayD"/> for intersection test.</param>
        /// <returns>Distance at which ray intersects with this <see cref="BoundingFrustumD"/> or null if no intersection happens.</returns>
        public double? Intersects(RayD ray)
        {
            double? result;
            Intersects(ref ray, out result);
            return result;
        }

        /// <summary>
        /// Gets the distance of intersection of <see cref="RayD"/> and this <see cref="BoundingFrustumD"/> or null if no intersection happens.
        /// </summary>
        /// <param name="ray">A <see cref="RayD"/> for intersection test.</param>
        /// <param name="result">Distance at which ray intersects with this <see cref="BoundingFrustumD"/> or null if no intersection happens as an output parameter.</param>
        public void Intersects(ref RayD ray, out double? result)
        {
            ContainmentType ctype;
            Contains(ref ray.Position, out ctype);

            if (ctype == ContainmentType.Disjoint)
            {
                result = null;
                return;
            }
            if (ctype == ContainmentType.Contains)
            {
                result = 0.0f;
                return;
            }
            if (ctype != ContainmentType.Intersects)
            {
                throw new ArgumentOutOfRangeException("ctype");
            }

            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void CreateCorners()
        {
            IntersectionPoint(
                ref this.planes[0],
                ref this.planes[2],
                ref this.planes[4],
                out this.corners[0]
            );
            IntersectionPoint(
                ref this.planes[0],
                ref this.planes[3],
                ref this.planes[4],
                out this.corners[1]
            );
            IntersectionPoint(
                ref this.planes[0],
                ref this.planes[3],
                ref this.planes[5],
                out this.corners[2]
            );
            IntersectionPoint(
                ref this.planes[0],
                ref this.planes[2],
                ref this.planes[5],
                out this.corners[3]
            );
            IntersectionPoint(
                ref this.planes[1],
                ref this.planes[2],
                ref this.planes[4],
                out this.corners[4]
            );
            IntersectionPoint(
                ref this.planes[1],
                ref this.planes[3],
                ref this.planes[4],
                out this.corners[5]
            );
            IntersectionPoint(
                ref this.planes[1],
                ref this.planes[3],
                ref this.planes[5],
                out this.corners[6]
            );
            IntersectionPoint(
                ref this.planes[1],
                ref this.planes[2],
                ref this.planes[5],
                out this.corners[7]
            );
        }

        private void CreatePlanes()
        {
            this.planes[0] = new PlaneD(
                -this.matrix.M13,
                -this.matrix.M23,
                -this.matrix.M33,
                -this.matrix.M43
            );
            this.planes[1] = new PlaneD(
                this.matrix.M13 - this.matrix.M14,
                this.matrix.M23 - this.matrix.M24,
                this.matrix.M33 - this.matrix.M34,
                this.matrix.M43 - this.matrix.M44
            );
            this.planes[2] = new PlaneD(
                -this.matrix.M14 - this.matrix.M11,
                -this.matrix.M24 - this.matrix.M21,
                -this.matrix.M34 - this.matrix.M31,
                -this.matrix.M44 - this.matrix.M41
            );
            this.planes[3] = new PlaneD(
                this.matrix.M11 - this.matrix.M14,
                this.matrix.M21 - this.matrix.M24,
                this.matrix.M31 - this.matrix.M34,
                this.matrix.M41 - this.matrix.M44
            );
            this.planes[4] = new PlaneD(
                this.matrix.M12 - this.matrix.M14,
                this.matrix.M22 - this.matrix.M24,
                this.matrix.M32 - this.matrix.M34,
                this.matrix.M42 - this.matrix.M44
            );
            this.planes[5] = new PlaneD(
                -this.matrix.M14 - this.matrix.M12,
                -this.matrix.M24 - this.matrix.M22,
                -this.matrix.M34 - this.matrix.M32,
                -this.matrix.M44 - this.matrix.M42
            );

            this.NormalizePlane(ref this.planes[0]);
            this.NormalizePlane(ref this.planes[1]);
            this.NormalizePlane(ref this.planes[2]);
            this.NormalizePlane(ref this.planes[3]);
            this.NormalizePlane(ref this.planes[4]);
            this.NormalizePlane(ref this.planes[5]);
        }

        private void NormalizePlane(ref PlaneD p)
        {
            double factor = 1f / p.Normal.Length();
            p.Normal.X *= factor;
            p.Normal.Y *= factor;
            p.Normal.Z *= factor;
            p.D *= factor;
        }

        #endregion

        #region Private Static Methods

        private static void IntersectionPoint(
            ref PlaneD a,
            ref PlaneD b,
            ref PlaneD c,
            out Vector3D result
        )
        {
            /* Formula used
			 *                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
			 * P =   -------------------------------------------------------------------
			 *                             N1 . ( N2 * N3 )
			 *
			 * Note: N refers to the normal, d refers to the displacement. '.' means dot
			 * product. '*' means cross product
			 */

            Vector3D v1, v2, v3;
            Vector3D cross;

            Vector3D.Cross(ref b.Normal, ref c.Normal, out cross);

            double f;
            Vector3D.Dot(ref a.Normal, ref cross, out f);
            f *= -1.0f;

            Vector3D.Cross(ref b.Normal, ref c.Normal, out cross);
            Vector3D.Multiply(ref cross, a.D, out v1);
            // v1 = (a.D * (Vector3D.Cross(b.Normal, c.Normal)));


            Vector3D.Cross(ref c.Normal, ref a.Normal, out cross);
            Vector3D.Multiply(ref cross, b.D, out v2);
            // v2 = (b.D * (Vector3D.Cross(c.Normal, a.Normal)));


            Vector3D.Cross(ref a.Normal, ref b.Normal, out cross);
            Vector3D.Multiply(ref cross, c.D, out v3);
            // v3 = (c.D * (Vector3D.Cross(a.Normal, b.Normal)));

            result.X = (v1.X + v2.X + v3.X) / f;
            result.Y = (v1.Y + v2.Y + v3.Y) / f;
            result.Z = (v1.Z + v2.Z + v3.Z) / f;
        }

        #endregion

        #region Public Static Operators and Override Methods

        /// <summary>
        /// Compares whether two <see cref="BoundingFrustumD"/> instances are equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingFrustumD"/> instance on the left of the equal sign.</param>
        /// <param name="b"><see cref="BoundingFrustumD"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(BoundingFrustumD a, BoundingFrustumD b)
        {
            if (object.Equals(a, null))
            {
                return (object.Equals(b, null));
            }

            if (object.Equals(b, null))
            {
                return (object.Equals(a, null));
            }

            return a.matrix == (b.matrix);
        }

        /// <summary>
        /// Compares whether two <see cref="BoundingFrustumD"/> instances are not equal.
        /// </summary>
        /// <param name="a"><see cref="BoundingFrustumD"/> instance on the left of the not equal sign.</param>
        /// <param name="b"><see cref="BoundingFrustumD"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(BoundingFrustumD a, BoundingFrustumD b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="other">The <see cref="BoundingFrustumD"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(BoundingFrustumD other)
        {
            return (this == other);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is BoundingFrustumD) && Equals((BoundingFrustumD)obj);
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="BoundingFrustumD"/> in the format:
        /// {Near:[nearPlane] Far:[farPlane] Left:[leftPlane] Right:[rightPlane] Top:[topPlane] Bottom:[bottomPlane]}
        /// </summary>
        /// <returns><see cref="String"/> representation of this <see cref="BoundingFrustumD"/>.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(256);
            sb.Append("{Near:");
            sb.Append(this.planes[0].ToString());
            sb.Append(" Far:");
            sb.Append(this.planes[1].ToString());
            sb.Append(" Left:");
            sb.Append(this.planes[2].ToString());
            sb.Append(" Right:");
            sb.Append(this.planes[3].ToString());
            sb.Append(" Top:");
            sb.Append(this.planes[4].ToString());
            sb.Append(" Bottom:");
            sb.Append(this.planes[5].ToString());
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Gets the hash code of this <see cref="BoundingFrustumD"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="BoundingFrustumD"/>.</returns>
        public override int GetHashCode()
        {
            return this.matrix.GetHashCode();
        }

        #endregion
    }
}
