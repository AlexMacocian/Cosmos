using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    [Serializable]
    public struct PlaneD : IEquatable<PlaneD>
    {
        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    Normal.DebugDisplayString, " ",
                    D.ToString()
                );
            }
        }

        #endregion

        #region Public Fields

        public Vector3D Normal;
        public double D;

        #endregion

        #region Public Constructors

        public PlaneD(Vector4D value)
            : this(new Vector3D(value.X, value.Y, value.Z), value.W)
        {
        }

        public PlaneD(Vector3D normal, double d)
        {
            Normal = normal;
            D = d;
        }

        public PlaneD(Vector3D a, Vector3D b, Vector3D c)
        {
            Vector3D ab = b - a;
            Vector3D ac = c - a;

            Vector3D cross = Vector3D.Cross(ab, ac);
            Vector3D.Normalize(ref cross, out Normal);
            D = -(Vector3D.Dot(Normal, a));
        }

        public PlaneD(double a, double b, double c, double d)
            : this(new Vector3D(a, b, c), d)
        {

        }

        #endregion

        #region Public Methods

        public double Dot(Vector4D value)
        {
            return (
                (this.Normal.X * value.X) +
                (this.Normal.Y * value.Y) +
                (this.Normal.Z * value.Z) +
                (this.D * value.W)
            );
        }

        public void Dot(ref Vector4D value, out double result)
        {
            result = (
                (this.Normal.X * value.X) +
                (this.Normal.Y * value.Y) +
                (this.Normal.Z * value.Z) +
                (this.D * value.W)
            );
        }

        public double DotCoordinate(Vector3D value)
        {
            return (
                (this.Normal.X * value.X) +
                (this.Normal.Y * value.Y) +
                (this.Normal.Z * value.Z) +
                this.D
            );
        }

        public void DotCoordinate(ref Vector3D value, out double result)
        {
            result = (
                (this.Normal.X * value.X) +
                (this.Normal.Y * value.Y) +
                (this.Normal.Z * value.Z) +
                this.D
            );
        }

        public double DotNormal(Vector3D value)
        {
            return (
                (this.Normal.X * value.X) +
                (this.Normal.Y * value.Y) +
                (this.Normal.Z * value.Z)
            );
        }

        public void DotNormal(ref Vector3D value, out double result)
        {
            result = (
                (this.Normal.X * value.X) +
                (this.Normal.Y * value.Y) +
                (this.Normal.Z * value.Z)
            );
        }

        public void Normalize()
        {
            double length = Normal.Length();
            double factor = 1.0f / length;
            Vector3D.Multiply(ref Normal, factor, out Normal);
            D = D * factor;
        }

        public PlaneIntersectionTypeD Intersects(BoundingBoxD box)
        {
            return box.Intersects(this);
        }

        public void Intersects(ref BoundingBoxD box, out PlaneIntersectionTypeD result)
        {
            box.Intersects(ref this, out result);
        }

        public PlaneIntersectionTypeD Intersects(BoundingSphereD sphere)
        {
            return sphere.Intersects(this);
        }

        public void Intersects(ref BoundingSphereD sphere, out PlaneIntersectionTypeD result)
        {
            sphere.Intersects(ref this, out result);
        }

        public PlaneIntersectionTypeD Intersects(BoundingFrustumD frustum)
        {
            return frustum.Intersects(this);
        }

        #endregion

        #region Internal Methods

        internal PlaneIntersectionTypeD Intersects(ref Vector3D point)
        {
            double distance;
            DotCoordinate(ref point, out distance);
            if (distance > 0)
            {
                return PlaneIntersectionTypeD.Front;
            }
            if (distance < 0)
            {
                return PlaneIntersectionTypeD.Back;
            }
            return PlaneIntersectionTypeD.Intersecting;
        }

        #endregion

        #region Public Static Methods

        public static PlaneD Normalize(PlaneD value)
        {
            PlaneD ret;
            Normalize(ref value, out ret);
            return ret;
        }

        public static void Normalize(ref PlaneD value, out PlaneD result)
        {
            double length = value.Normal.Length();
            double factor = 1.0f / length;
            Vector3D.Multiply(ref value.Normal, factor, out result.Normal);
            result.D = value.D * factor;
        }

        /// <summary>
        /// Transforms a normalized plane by a matrix.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed plane.</returns>
        public static PlaneD Transform(PlaneD plane, MatrixD matrix)
        {
            PlaneD result;
            Transform(ref plane, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a normalized plane by a matrix.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <param name="result">The transformed plane.</param>
        public static void Transform(
            ref PlaneD plane,
            ref MatrixD matrix,
            out PlaneD result
        )
        {
            /* See "Transforming Normals" in
			 * http://www.glprogramming.com/red/appendixf.html
			 * for an explanation of how this works.
			 */
            MatrixD transformedMatrix;
            MatrixD.Invert(ref matrix, out transformedMatrix);
            MatrixD.Transpose(
                ref transformedMatrix,
                out transformedMatrix
            );
            Vector4D vector = new Vector4D(plane.Normal, plane.D);
            Vector4D transformedVector;
            Vector4D.Transform(
                ref vector,
                ref transformedMatrix,
                out transformedVector
            );
            result = new PlaneD(transformedVector);
        }

        /// <summary>
        /// Transforms a normalized plane by a quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <returns>The transformed plane.</returns>
        public static PlaneD Transform(PlaneD plane, QuaternionD rotation)
        {
            PlaneD result;
            Transform(ref plane, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Transforms a normalized plane by a quaternion rotation.
        /// </summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The quaternion rotation.</param>
        /// <param name="result">The transformed plane.</param>
        public static void Transform(
            ref PlaneD plane,
            ref QuaternionD rotation,
            out PlaneD result
        )
        {
            Vector3D.Transform(
                ref plane.Normal,
                ref rotation,
                out result.Normal
            );
            result.D = plane.D;
        }

        #endregion

        #region Public Static Operators and Override Methods

        public static bool operator !=(PlaneD plane1, PlaneD plane2)
        {
            return !plane1.Equals(plane2);
        }

        public static bool operator ==(PlaneD plane1, PlaneD plane2)
        {
            return plane1.Equals(plane2);
        }

        public override bool Equals(object obj)
        {
            return (obj is PlaneD) && this.Equals((PlaneD)obj);
        }

        public bool Equals(PlaneD other)
        {
            return (Normal == other.Normal && D == other.D);
        }

        public override int GetHashCode()
        {
            return Normal.GetHashCode() ^ D.GetHashCode();
        }

        public override string ToString()
        {
            return (
                "{Normal:" + Normal.ToString() +
                " D:" + D.ToString() +
                "}"
            );
        }

        #endregion
    }
}
