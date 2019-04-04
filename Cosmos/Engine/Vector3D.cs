using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    /// <summary>
    /// Describes a 3D-vector.
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Vector3D : IEquatable<Vector3D>
    {
        #region Public Static Properties

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, 0, 0.
        /// </summary>
        public static Vector3D Zero
        {
            get
            {
                return zero;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 1, 1, 1.
        /// </summary>
        public static Vector3D One
        {
            get
            {
                return one;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 1, 0, 0.
        /// </summary>
        public static Vector3D UnitX
        {
            get
            {
                return unitX;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, 1, 0.
        /// </summary>
        public static Vector3D UnitY
        {
            get
            {
                return unitY;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, 0, 1.
        /// </summary>
        public static Vector3D UnitZ
        {
            get
            {
                return unitZ;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, 1, 0.
        /// </summary>
        public static Vector3D Up
        {
            get
            {
                return up;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, -1, 0.
        /// </summary>
        public static Vector3D Down
        {
            get
            {
                return down;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 1, 0, 0.
        /// </summary>
        public static Vector3D Right
        {
            get
            {
                return right;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components -1, 0, 0.
        /// </summary>
        public static Vector3D Left
        {
            get
            {
                return left;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, 0, -1.
        /// </summary>
        public static Vector3D Forward
        {
            get
            {
                return forward;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector3D"/> with components 0, 0, 1.
        /// </summary>
        public static Vector3D Backward
        {
            get
            {
                return backward;
            }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    X.ToString(), " ",
                    Y.ToString(), " ",
                    Z.ToString()
                );
            }
        }

        #endregion

        #region Private Static Fields

        private static Vector3D zero = new Vector3D(0f, 0f, 0f); // Not readonly for performance -flibit
        private static readonly Vector3D one = new Vector3D(1f, 1f, 1f);
        private static readonly Vector3D unitX = new Vector3D(1f, 0f, 0f);
        private static readonly Vector3D unitY = new Vector3D(0f, 1f, 0f);
        private static readonly Vector3D unitZ = new Vector3D(0f, 0f, 1f);
        private static readonly Vector3D up = new Vector3D(0f, 1f, 0f);
        private static readonly Vector3D down = new Vector3D(0f, -1f, 0f);
        private static readonly Vector3D right = new Vector3D(1f, 0f, 0f);
        private static readonly Vector3D left = new Vector3D(-1f, 0f, 0f);
        private static readonly Vector3D forward = new Vector3D(0f, 0f, -1f);
        private static readonly Vector3D backward = new Vector3D(0f, 0f, 1f);

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Vector3D"/>.
        /// </summary>
        public double X;

        /// <summary>
        /// The y coordinate of this <see cref="Vector3D"/>.
        /// </summary>
        public double Y;

        /// <summary>
        /// The z coordinate of this <see cref="Vector3D"/>.
        /// </summary>
        public double Z;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs a 3d vector with X, Y and Z from three values.
        /// </summary>
        /// <param name="x">The x coordinate in 3d-space.</param>
        /// <param name="y">The y coordinate in 3d-space.</param>
        /// <param name="z">The z coordinate in 3d-space.</param>
        public Vector3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Constructs a 3d vector with X, Y and Z set to the same value.
        /// </summary>
        /// <param name="value">The x, y and z coordinates in 3d-space.</param>
        public Vector3D(double value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }

        /// <summary>
        /// Constructs a 3d vector with X, Y from <see cref="Vector2"/> and Z from a scalar.
        /// </summary>
        /// <param name="value">The x and y coordinates in 3d-space.</param>
        /// <param name="z">The z coordinate in 3d-space.</param>
        public Vector3D(Vector2D value, double z)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = z;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Vector3D) && Equals((Vector3D)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector3D"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vector3D other)
        {
            return (X == other.X &&
                    Y == other.Y &&
                    Z == other.Z);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vector3D"/>.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        /// Returns the length of this <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Vector3D"/>.</returns>
        public double Length()
        {
            return (double)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Vector3D"/>.</returns>
        public double LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        /// <summary>
        /// Turns this <see cref="Vector3D"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            double factor = 1.0f / (double)Math.Sqrt(
                (X * X) +
                (Y * Y) +
                (Z * Z)
            );
            X *= factor;
            Y *= factor;
            Z *= factor;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Vector3D"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Z:[<see cref="Z"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Vector3D"/>.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(32);
            sb.Append("{X:");
            sb.Append(this.X);
            sb.Append(" Y:");
            sb.Append(this.Y);
            sb.Append(" Z:");
            sb.Append(this.Z);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Vector3D Add(Vector3D value1, Vector3D value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and
        /// <paramref name="value2"/>, storing the result of the
        /// addition in <paramref name="result"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <param name="result">The result of the vector addition.</param>
        public static void Add(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 3d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 3d-triangle.</param>
        /// <param name="value2">The second vector of 3d-triangle.</param>
        /// <param name="value3">The third vector of 3d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 3d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 3d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Vector3D Barycentric(
            Vector3D value1,
            Vector3D value2,
            Vector3D value3,
            double amount1,
            double amount2
        )
        {
            return new Vector3D(
                MathHelperD.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelperD.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
                MathHelperD.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 3d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 3d-triangle.</param>
        /// <param name="value2">The second vector of 3d-triangle.</param>
        /// <param name="value3">The third vector of 3d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 3d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 3d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(
            ref Vector3D value1,
            ref Vector3D value2,
            ref Vector3D value3,
            double amount1,
            double amount2,
            out Vector3D result
        )
        {
            result.X = MathHelperD.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = MathHelperD.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
            result.Z = MathHelperD.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vector3D CatmullRom(
            Vector3D value1,
            Vector3D value2,
            Vector3D value3,
            Vector3D value4,
            double amount
        )
        {
            return new Vector3D(
                MathHelperD.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelperD.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                MathHelperD.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(
            ref Vector3D value1,
            ref Vector3D value2,
            ref Vector3D value3,
            ref Vector3D value4,
            double amount,
            out Vector3D result
        )
        {
            result.X = MathHelperD.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelperD.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
            result.Z = MathHelperD.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector3D Clamp(Vector3D value1, Vector3D min, Vector3D max)
        {
            return new Vector3D(
                MathHelperD.Clamp(value1.X, min.X, max.X),
                MathHelperD.Clamp(value1.Y, min.Y, max.Y),
                MathHelperD.Clamp(value1.Z, min.Z, max.Z)
            );
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(
            ref Vector3D value1,
            ref Vector3D min,
            ref Vector3D max,
            out Vector3D result
        )
        {
            result.X = MathHelperD.Clamp(value1.X, min.X, max.X);
            result.Y = MathHelperD.Clamp(value1.Y, min.Y, max.Y);
            result.Z = MathHelperD.Clamp(value1.Z, min.Z, max.Z);
        }

        /// <summary>
        /// Computes the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of two vectors.</returns>
        public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
        {
            Cross(ref vector1, ref vector2, out vector1);
            return vector1;
        }

        /// <summary>
        /// Computes the cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of two vectors as an output parameter.</param>
        public static void Cross(ref Vector3D vector1, ref Vector3D vector2, out Vector3D result)
        {
            double x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
            double y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
            double z = vector1.X * vector2.Y - vector2.X * vector1.Y;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static double Distance(Vector3D vector1, Vector3D vector2)
        {
            double result;
            DistanceSquared(ref vector1, ref vector2, out result);
            return (double)Math.Sqrt(result);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vector3D value1, ref Vector3D value2, out double result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (double)Math.Sqrt(result);
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static double DistanceSquared(Vector3D value1, Vector3D value2)
        {
            return (
                (value1.X - value2.X) * (value1.X - value2.X) +
                (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                (value1.Z - value2.Z) * (value1.Z - value2.Z)
            );
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(
            ref Vector3D value1,
            ref Vector3D value2,
            out double result
        )
        {
            result = (
                (value1.X - value2.X) * (value1.X - value2.X) +
                (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                (value1.Z - value2.Z) * (value1.Z - value2.Z)
            );
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector3D"/> by the components of another <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector3D"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector3D Divide(Vector3D value1, Vector3D value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector3D"/> by the components of another <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector3D"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector3D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector3D Divide(Vector3D value1, double value2)
        {
            double factor = 1 / value2;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector3D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Vector3D value1, double value2, out Vector3D result)
        {
            double factor = 1 / value2;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
            result.Z = value1.Z * factor;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static double Dot(Vector3D vector1, Vector3D vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Vector3D vector1, ref Vector3D vector2, out double result)
        {
            result = (
                (vector1.X * vector2.X) +
                (vector1.Y * vector2.Y) +
                (vector1.Z * vector2.Z)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vector3D Hermite(
            Vector3D value1,
            Vector3D tangent1,
            Vector3D value2,
            Vector3D tangent2,
            double amount
        )
        {
            Vector3D result = new Vector3D();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(
            ref Vector3D value1,
            ref Vector3D tangent1,
            ref Vector3D value2,
            ref Vector3D tangent2,
            double amount,
            out Vector3D result
        )
        {
            result.X = MathHelperD.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = MathHelperD.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
            result.Z = MathHelperD.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vector3D Lerp(Vector3D value1, Vector3D value2, double amount)
        {
            return new Vector3D(
                MathHelperD.Lerp(value1.X, value2.X, amount),
                MathHelperD.Lerp(value1.Y, value2.Y, amount),
                MathHelperD.Lerp(value1.Z, value2.Z, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(
            ref Vector3D value1,
            ref Vector3D value2,
            double amount,
            out Vector3D result
        )
        {
            result.X = MathHelperD.Lerp(value1.X, value2.X, amount);
            result.Y = MathHelperD.Lerp(value1.Y, value2.Y, amount);
            result.Z = MathHelperD.Lerp(value1.Z, value2.Z, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector3D"/> with maximal values from the two vectors.</returns>
        public static Vector3D Max(Vector3D value1, Vector3D value2)
        {
            return new Vector3D(
                MathHelperD.Max(value1.X, value2.X),
                MathHelperD.Max(value1.Y, value2.Y),
                MathHelperD.Max(value1.Z, value2.Z)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector3D"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.X = MathHelperD.Max(value1.X, value2.X);
            result.Y = MathHelperD.Max(value1.Y, value2.Y);
            result.Z = MathHelperD.Max(value1.Z, value2.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector3D"/> with minimal values from the two vectors.</returns>
        public static Vector3D Min(Vector3D value1, Vector3D value2)
        {
            return new Vector3D(
                MathHelperD.Min(value1.X, value2.X),
                MathHelperD.Min(value1.Y, value2.Y),
                MathHelperD.Min(value1.Z, value2.Z)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector3D"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.X = MathHelperD.Min(value1.X, value2.X);
            result.Y = MathHelperD.Min(value1.Y, value2.Y);
            result.Z = MathHelperD.Min(value1.Z, value2.Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Source <see cref="Vector3D"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Vector3D Multiply(Vector3D value1, Vector3D value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a multiplication of <see cref="Vector3D"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Vector3D Multiply(Vector3D value1, double scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            value1.Z *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a multiplication of <see cref="Vector3D"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Vector3D value1, double scaleFactor, out Vector3D result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Source <see cref="Vector3D"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Vector3D Negate(Vector3D value)
        {
            value = new Vector3D(-value.X, -value.Y, -value.Z);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Vector3D value, out Vector3D result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Vector3D Normalize(Vector3D value)
        {
            double factor = 1.0f / (double)Math.Sqrt(
                (value.X * value.X) +
                (value.Y * value.Y) +
                (value.Z * value.Z)
            );
            return new Vector3D(
                value.X * factor,
                value.Y * factor,
                value.Z * factor
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Vector3D value, out Vector3D result)
        {
            double factor = 1.0f / (double)Math.Sqrt(
                (value.X * value.X) +
                (value.Y * value.Y) +
                (value.Z * value.Z)
            );
            result.X = value.X * factor;
            result.Y = value.Y * factor;
            result.Z = value.Z * factor;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vector3D"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Vector3D Reflect(Vector3D vector, Vector3D normal)
        {
            /* I is the original array.
			 * N is the normal of the incident plane.
			 * R = I - (2 * N * ( DotProduct[ I,N] ))
			 */
            Vector3D reflectedVector;
            // Inline the dotProduct here instead of calling method
            double dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) +
                        (vector.Z * normal.Z);
            reflectedVector.X = vector.X - (2.0f * normal.X) * dotProduct;
            reflectedVector.Y = vector.Y - (2.0f * normal.Y) * dotProduct;
            reflectedVector.Z = vector.Z - (2.0f * normal.Z) * dotProduct;

            return reflectedVector;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vector3D"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref Vector3D vector, ref Vector3D normal, out Vector3D result)
        {
            /* I is the original array.
			 * N is the normal of the incident plane.
			 * R = I - (2 * N * ( DotProduct[ I,N] ))
			 */

            // Inline the dotProduct here instead of calling method.
            double dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) +
                        (vector.Z * normal.Z);
            result.X = vector.X - (2.0f * normal.X) * dotProduct;
            result.Y = vector.Y - (2.0f * normal.Y) * dotProduct;
            result.Z = vector.Z - (2.0f * normal.Z) * dotProduct;

        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Source <see cref="Vector3D"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Vector3D SmoothStep(Vector3D value1, Vector3D value2, double amount)
        {
            return new Vector3D(
                MathHelperD.SmoothStep(value1.X, value2.X, amount),
                MathHelperD.SmoothStep(value1.Y, value2.Y, amount),
                MathHelperD.SmoothStep(value1.Z, value2.Z, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Source <see cref="Vector3D"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(
            ref Vector3D value1,
            ref Vector3D value2,
            double amount,
            out Vector3D result
        )
        {
            result.X = MathHelperD.SmoothStep(value1.X, value2.X, amount);
            result.Y = MathHelperD.SmoothStep(value1.Y, value2.Y, amount);
            result.Z = MathHelperD.SmoothStep(value1.Z, value2.Z, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains subtraction of on <see cref="Vector3D"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Source <see cref="Vector3D"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vector3D Subtract(Vector3D value1, Vector3D value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains subtraction of on <see cref="Vector3D"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/>.</param>
        /// <param name="value2">Source <see cref="Vector3D"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Vector3D value1, ref Vector3D value2, out Vector3D result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a transformation of 3d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector3D"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed <see cref="Vector3D"/>.</returns>
        public static Vector3D Transform(Vector3D position, MatrixD matrix)
        {
            Transform(ref position, ref matrix, out position);
            return position;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a transformation of 3d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector3D"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed <see cref="Vector3D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector3D position,
            ref MatrixD matrix,
            out Vector3D result
        )
        {
            double x = (
                (position.X * matrix.M11) +
                (position.Y * matrix.M21) +
                (position.Z * matrix.M31) +
                matrix.M41
            );
            double y = (
                (position.X * matrix.M12) +
                (position.Y * matrix.M22) +
                (position.Z * matrix.M32) +
                matrix.M42
            );
            double z = (
                (position.X * matrix.M13) +
                (position.Y * matrix.M23) +
                (position.Z * matrix.M33) +
                matrix.M43
            );
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector3D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vector3D[] sourceArray,
            ref MatrixD matrix,
            Vector3D[] destinationArray
        )
        {
            Debug.Assert(
                destinationArray.Length >= sourceArray.Length,
                "The destination array is smaller than the source array."
            );

            /* TODO: Are there options on some platforms to implement
			 * a vectorized version of this?
			 */

            for (int i = 0; i < sourceArray.Length; i += 1)
            {
                Vector3D position = sourceArray[i];
                destinationArray[i] = new Vector3D(
                    (position.X * matrix.M11) + (position.Y * matrix.M21) +
                        (position.Z * matrix.M31) + matrix.M41,
                    (position.X * matrix.M12) + (position.Y * matrix.M22) +
                        (position.Z * matrix.M32) + matrix.M42,
                    (position.X * matrix.M13) + (position.Y * matrix.M23) +
                        (position.Z * matrix.M33) + matrix.M43
                );
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector3D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector3D"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vector3D[] sourceArray,
            int sourceIndex,
            ref MatrixD matrix,
            Vector3D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            Debug.Assert(
                sourceArray.Length - sourceIndex >= length,
                "The source array is too small for the given sourceIndex and length."
            );
            Debug.Assert(
                destinationArray.Length - destinationIndex >= length,
                "The destination array is too small for " +
                "the given destinationIndex and length."
            );

            /* TODO: Are there options on some platforms to implement a
			 * vectorized version of this?
			 */

            for (int i = 0; i < length; i += 1)
            {
                Vector3D position = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] = new Vector3D(
                    (position.X * matrix.M11) + (position.Y * matrix.M21) +
                        (position.Z * matrix.M31) + matrix.M41,
                    (position.X * matrix.M12) + (position.Y * matrix.M22) +
                        (position.Z * matrix.M32) + matrix.M42,
                    (position.X * matrix.M13) + (position.Y * matrix.M23) +
                        (position.Z * matrix.M33) + matrix.M43
                );
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a transformation of 3d-vector by the specified <see cref="QuaternionD"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vector3D"/>.</returns>
        public static Vector3D Transform(Vector3D value, QuaternionD rotation)
        {
            Vector3D result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a transformation of 3d-vector by the specified <see cref="QuaternionD"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vector3D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector3D value,
            ref QuaternionD rotation,
            out Vector3D result
        )
        {
            double x = 2 * (rotation.Y * value.Z - rotation.Z * value.Y);
            double y = 2 * (rotation.Z * value.X - rotation.X * value.Z);
            double z = 2 * (rotation.X * value.Y - rotation.Y * value.X);

            result.X = value.X + x * rotation.W + (rotation.Y * z - rotation.Z * y);
            result.Y = value.Y + y * rotation.W + (rotation.Z * x - rotation.X * z);
            result.Z = value.Z + z * rotation.W + (rotation.X * y - rotation.Y * x);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector3D"/> by the specified <see cref="QuaternionD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vector3D[] sourceArray,
            ref QuaternionD rotation,
            Vector3D[] destinationArray
        )
        {
            Debug.Assert(
                destinationArray.Length >= sourceArray.Length,
                "The destination array is smaller than the source array."
            );

            /* TODO: Are there options on some platforms to implement
			 * a vectorized version of this?
			 */

            for (int i = 0; i < sourceArray.Length; i += 1)
            {
                Vector3D position = sourceArray[i];

                double x = 2 * (rotation.Y * position.Z - rotation.Z * position.Y);
                double y = 2 * (rotation.Z * position.X - rotation.X * position.Z);
                double z = 2 * (rotation.X * position.Y - rotation.Y * position.X);

                destinationArray[i] = new Vector3D(
                    position.X + x * rotation.W + (rotation.Y * z - rotation.Z * y),
                    position.Y + y * rotation.W + (rotation.Z * x - rotation.X * z),
                    position.Z + z * rotation.W + (rotation.X * y - rotation.Y * x)
                );
            }
        }

        /// <summary>

        /// Apply transformation on vectors within array of <see cref="Vector3D"/> by the specified <see cref="QuaternionD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector3"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vector3D[] sourceArray,
            int sourceIndex,
            ref QuaternionD rotation,
            Vector3D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            Debug.Assert(
                sourceArray.Length - sourceIndex >= length,
                "The source array is too small for the given sourceIndex and length."
            );
            Debug.Assert(
                destinationArray.Length - destinationIndex >= length,
                "The destination array is too small for the " +
                "given destinationIndex and length."
            );

            /* TODO: Are there options on some platforms to implement
			 * a vectorized version of this?
			 */

            for (int i = 0; i < length; i += 1)
            {
                Vector3D position = sourceArray[sourceIndex + i];

                double x = 2 * (rotation.Y * position.Z - rotation.Z * position.Y);
                double y = 2 * (rotation.Z * position.X - rotation.X * position.Z);
                double z = 2 * (rotation.X * position.Y - rotation.Y * position.X);

                destinationArray[destinationIndex + i] = new Vector3D(
                    position.X + x * rotation.W + (rotation.Y * z - rotation.Z * y),
                    position.Y + y * rotation.W + (rotation.Z * x - rotation.X * z),
                    position.Z + z * rotation.W + (rotation.X * y - rotation.Y * x)
                );
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a transformation of the specified normal by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector3D"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Vector3D TransformNormal(Vector3D normal, MatrixD matrix)
        {
            TransformNormal(ref normal, ref matrix, out normal);
            return normal;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3D"/> that contains a transformation of the specified normal by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector3D"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(
            ref Vector3D normal,
            ref MatrixD matrix,
            out Vector3D result
        )
        {
            double x = (normal.X * matrix.M11) + (normal.Y * matrix.M21) + (normal.Z * matrix.M31);
            double y = (normal.X * matrix.M12) + (normal.Y * matrix.M22) + (normal.Z * matrix.M32);
            double z = (normal.X * matrix.M13) + (normal.Y * matrix.M23) + (normal.Z * matrix.M33);
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Vector3D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal(
            Vector3D[] sourceArray,
            ref MatrixD matrix,
            Vector3D[] destinationArray
        )
        {
            Debug.Assert(
                destinationArray.Length >= sourceArray.Length,
                "The destination array is smaller than the source array."
            );

            for (int i = 0; i < sourceArray.Length; i += 1)
            {
                Vector3D normal = sourceArray[i];
                destinationArray[i].X = (normal.X * matrix.M11) + (normal.Y * matrix.M21) + (normal.Z * matrix.M31);
                destinationArray[i].Y = (normal.X * matrix.M12) + (normal.Y * matrix.M22) + (normal.Z * matrix.M32);
                destinationArray[i].Z = (normal.X * matrix.M13) + (normal.Y * matrix.M23) + (normal.Z * matrix.M33);
            }
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Vector3D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector3D"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal(
            Vector3D[] sourceArray,
            int sourceIndex,
            ref MatrixD matrix,
            Vector3D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if ((sourceIndex + length) > sourceArray.Length)
            {
                throw new ArgumentException(
                    "the combination of sourceIndex and " +
                    "length was greater than sourceArray.Length"
                );
            }
            if ((destinationIndex + length) > destinationArray.Length)
            {
                throw new ArgumentException(
                    "destinationArray is too small to " +
                    "contain the result"
                );
            }

            for (int i = 0; i < length; i += 1)
            {
                Vector3D normal = sourceArray[i + sourceIndex];
                destinationArray[i + destinationIndex].X = (
                    (normal.X * matrix.M11) +
                    (normal.Y * matrix.M21) +
                    (normal.Z * matrix.M31)
                );
                destinationArray[i + destinationIndex].Y = (
                    (normal.X * matrix.M12) +
                    (normal.Y * matrix.M22) +
                    (normal.Z * matrix.M32)
                );
                destinationArray[i + destinationIndex].Z = (
                    (normal.X * matrix.M13) +
                    (normal.Y * matrix.M23) +
                    (normal.Z * matrix.M33)
                );
            }
        }

        #endregion

        #region Public Static Operators

        /// <summary>
        /// Compares whether two <see cref="Vector3D"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector3D"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vector3D"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Vector3D value1, Vector3D value2)
        {
            return (value1.X == value2.X &&
                    value1.Y == value2.Y &&
                    value1.Z == value2.Z);
        }

        /// <summary>
        /// Compares whether two <see cref="Vector3D"/> instances are not equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector3D"/> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="Vector3D"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Vector3D value1, Vector3D value2)
        {
            return !(value1 == value2);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vector3D"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vector3D operator +(Vector3D value1, Vector3D value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        /// <summary>
        /// Inverts values in the specified <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vector3D operator -(Vector3D value)
        {
            value = new Vector3D(-value.X, -value.Y, -value.Z);
            return value;
        }

        /// <summary>
        /// Subtracts a <see cref="Vector3D"/> from a <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vector3D"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vector3D operator -(Vector3D value1, Vector3D value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vector3D"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vector3D operator *(Vector3D value1, Vector3D value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector3D operator *(Vector3D value, double scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vector3D"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector3D operator *(double scaleFactor, Vector3D value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector3D"/> by the components of another <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector3D"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vector3D"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector3D operator /(Vector3D value1, Vector3D value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector3D"/> by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3D"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector3D operator /(Vector3D value, double divider)
        {
            double factor = 1 / divider;
            value.X *= factor;
            value.Y *= factor;
            value.Z *= factor;
            return value;
        }

        #endregion
    }
}
