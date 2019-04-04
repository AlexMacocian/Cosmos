using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    /// <summary>
    /// Describes a 4D-vector.
    /// </summary>
    [Serializable]
    public struct Vector4D : IEquatable<Vector4D>
    {
        #region Public Static Properties

        /// <summary>
        /// Returns a <see cref="Vector4D"/> with components 0, 0, 0, 0.
        /// </summary>
        public static Vector4D Zero
        {
            get
            {
                return zero;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector4D"/> with components 1, 1, 1, 1.
        /// </summary>
        public static Vector4D One
        {
            get
            {
                return unit;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector4D"/> with components 1, 0, 0, 0.
        /// </summary>
        public static Vector4D UnitX
        {
            get
            {
                return unitX;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector4D"/> with components 0, 1, 0, 0.
        /// </summary>
        public static Vector4D UnitY
        {
            get
            {
                return unitY;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector4D"/> with components 0, 0, 1, 0.
        /// </summary>
        public static Vector4D UnitZ
        {
            get
            {
                return unitZ;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector4D"/> with components 0, 0, 0, 1.
        /// </summary>
        public static Vector4D UnitW
        {
            get
            {
                return unitW;
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
                    Z.ToString(), " ",
                    W.ToString()
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Vector4D"/>.
        /// </summary>
        public double X;

        /// <summary>
        /// The y coordinate of this <see cref="Vector4D"/>.
        /// </summary>
        public double Y;

        /// <summary>
        /// The z coordinate of this <see cref="Vector4D"/>.
        /// </summary>
        public double Z;

        /// <summary>
        /// The w coordinate of this <see cref="Vector4D"/>.
        /// </summary>
        public double W;

        #endregion

        #region Private Static Fields

        private static Vector4D zero = new Vector4D(); // Not readonly for performance -flibit
        private static readonly Vector4D unit = new Vector4D(1f, 1f, 1f, 1f);
        private static readonly Vector4D unitX = new Vector4D(1f, 0f, 0f, 0f);
        private static readonly Vector4D unitY = new Vector4D(0f, 1f, 0f, 0f);
        private static readonly Vector4D unitZ = new Vector4D(0f, 0f, 1f, 0f);
        private static readonly Vector4D unitW = new Vector4D(0f, 0f, 0f, 1f);

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs a 3d vector with X, Y, Z and W from four values.
        /// </summary>
        /// <param name="x">The x coordinate in 4d-space.</param>
        /// <param name="y">The y coordinate in 4d-space.</param>
        /// <param name="z">The z coordinate in 4d-space.</param>
        /// <param name="w">The w coordinate in 4d-space.</param>
        public Vector4D(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Constructs a 3d vector with X and Z from <see cref="Vector2"/> and Z and W from the scalars.
        /// </summary>
        /// <param name="value">The x and y coordinates in 4d-space.</param>
        /// <param name="z">The z coordinate in 4d-space.</param>
        /// <param name="w">The w coordinate in 4d-space.</param>
        public Vector4D(Vector2D value, double z, double w)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Constructs a 3d vector with X, Y, Z from <see cref="Vector3"/> and W from a scalar.
        /// </summary>
        /// <param name="value">The x, y and z coordinates in 4d-space.</param>
        /// <param name="w">The w coordinate in 4d-space.</param>
        public Vector4D(Vector3D value, double w)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.W = w;
        }

        /// <summary>
        /// Constructs a 4d vector with X, Y, Z and W set to the same value.
        /// </summary>
        /// <param name="value">The x, y, z and w coordinates in 4d-space.</param>
        public Vector4D(double value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
            this.W = value;
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
            return (obj is Vector4D) && Equals((Vector4D)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vector4D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector4D"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vector4D other)
        {
            return (X == other.X &&
                    Y == other.Y &&
                    Z == other.Z &&
                    W == other.W);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vector4D"/>.</returns>
        public override int GetHashCode()
        {
            return W.GetHashCode() + X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        /// Returns the length of this <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Vector4D"/>.</returns>
        public double Length()
        {
            return (double)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Vector4D"/>.</returns>
        public double LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        /// <summary>
        /// Turns this <see cref="Vector4D"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            double factor = 1.0f / (double)Math.Sqrt(
                (X * X) +
                (Y * Y) +
                (Z * Z) +
                (W * W)
            );
            X *= factor;
            Y *= factor;
            Z *= factor;
            W *= factor;
        }

        public override string ToString()
        {
            return (
                "{X:" + X.ToString() +
                " Y:" + Y.ToString() +
                " Z:" + Z.ToString() +
                " W:" + W.ToString() + "}"
            );
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Vector4D Add(Vector4D value1, Vector4D value2)
        {
            value1.W += value2.W;
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
        public static void Add(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.W = value1.W + value2.W;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 4d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 4d-triangle.</param>
        /// <param name="value2">The second vector of 4d-triangle.</param>
        /// <param name="value3">The third vector of 4d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 4d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 4d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Vector4D Barycentric(
            Vector4D value1,
            Vector4D value2,
            Vector4D value3,
            double amount1,
            double amount2
        )
        {
            return new Vector4D(
                MathHelperD.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelperD.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
                MathHelperD.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2),
                MathHelperD.Barycentric(value1.W, value2.W, value3.W, amount1, amount2)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 4d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 4d-triangle.</param>
        /// <param name="value2">The second vector of 4d-triangle.</param>
        /// <param name="value3">The third vector of 4d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 4d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 4d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(
            ref Vector4D value1,
            ref Vector4D value2,
            ref Vector4D value3,
            double amount1,
            double amount2,
            out Vector4D result
        )
        {
            result.X = MathHelperD.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = MathHelperD.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
            result.Z = MathHelperD.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
            result.W = MathHelperD.Barycentric(value1.W, value2.W, value3.W, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vector4D CatmullRom(
            Vector4D value1,
            Vector4D value2,
            Vector4D value3,
            Vector4D value4,
            double amount
        )
        {
            return new Vector4D(
                MathHelperD.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelperD.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                MathHelperD.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount),
                MathHelperD.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(
            ref Vector4D value1,
            ref Vector4D value2,
            ref Vector4D value3,
            ref Vector4D value4,
            double amount,
            out Vector4D result
        )
        {
            result.X = MathHelperD.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelperD.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
            result.Z = MathHelperD.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
            result.W = MathHelperD.CatmullRom(value1.W, value2.W, value3.W, value4.W, amount);
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector4D Clamp(Vector4D value1, Vector4D min, Vector4D max)
        {
            return new Vector4D(
                MathHelperD.Clamp(value1.X, min.X, max.X),
                MathHelperD.Clamp(value1.Y, min.Y, max.Y),
                MathHelperD.Clamp(value1.Z, min.Z, max.Z),
                MathHelperD.Clamp(value1.W, min.W, max.W)
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
            ref Vector4D value1,
            ref Vector4D min,
            ref Vector4D max,
            out Vector4D result
        )
        {
            result.X = MathHelperD.Clamp(value1.X, min.X, max.X);
            result.Y = MathHelperD.Clamp(value1.Y, min.Y, max.Y);
            result.Z = MathHelperD.Clamp(value1.Z, min.Z, max.Z);
            result.W = MathHelperD.Clamp(value1.W, min.W, max.W);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static double Distance(Vector4D value1, Vector4D value2)
        {
            return (double)Math.Sqrt(DistanceSquared(value1, value2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vector4D value1, ref Vector4D value2, out double result)
        {
            result = (double)Math.Sqrt(DistanceSquared(value1, value2));
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static double DistanceSquared(Vector4D value1, Vector4D value2)
        {
            return (
                (value1.W - value2.W) * (value1.W - value2.W) +
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
            ref Vector4D value1,
            ref Vector4D value2,
            out double result
        )
        {
            result = (
                (value1.W - value2.W) * (value1.W - value2.W) +
                (value1.X - value2.X) * (value1.X - value2.X) +
                (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                (value1.Z - value2.Z) * (value1.Z - value2.Z)
            );
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector4D"/> by the components of another <see cref="Vector4D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector4D"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector4D Divide(Vector4D value1, Vector4D value2)
        {
            value1.W /= value2.W;
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector4D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector4D Divide(Vector4D value1, double divider)
        {
            double factor = 1f / divider;
            value1.W *= factor;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector4D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Vector4D value1, double divider, out Vector4D result)
        {
            double factor = 1f / divider;
            result.W = value1.W * factor;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
            result.Z = value1.Z * factor;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector4D"/> by the components of another <see cref="Vector4D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector4D"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(
            ref Vector4D value1,
            ref Vector4D value2,
            out Vector4D result
        )
        {
            result.W = value1.W / value2.W;
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static double Dot(Vector4D vector1, Vector4D vector2)
        {
            return (
                vector1.X * vector2.X +
                vector1.Y * vector2.Y +
                vector1.Z * vector2.Z +
                vector1.W * vector2.W
            );
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Vector4D vector1, ref Vector4D vector2, out double result)
        {
            result = (
                (vector1.X * vector2.X) +
                (vector1.Y * vector2.Y) +
                (vector1.Z * vector2.Z) +
                (vector1.W * vector2.W)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vector4D Hermite(
            Vector4D value1,
            Vector4D tangent1,
            Vector4D value2,
            Vector4D tangent2,
            double amount
        )
        {
            return new Vector4D(
                MathHelperD.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
                MathHelperD.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount),
                MathHelperD.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount),
                MathHelperD.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(
            ref Vector4D value1,
            ref Vector4D tangent1,
            ref Vector4D value2,
            ref Vector4D tangent2,
            double amount,
            out Vector4D result
        )
        {
            result.W = MathHelperD.Hermite(value1.W, tangent1.W, value2.W, tangent2.W, amount);
            result.X = MathHelperD.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = MathHelperD.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
            result.Z = MathHelperD.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vector4D Lerp(Vector4D value1, Vector4D value2, double amount)
        {
            return new Vector4D(
                MathHelperD.Lerp(value1.X, value2.X, amount),
                MathHelperD.Lerp(value1.Y, value2.Y, amount),
                MathHelperD.Lerp(value1.Z, value2.Z, amount),
                MathHelperD.Lerp(value1.W, value2.W, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(
            ref Vector4D value1,
            ref Vector4D value2,
            double amount,
            out Vector4D result
        )
        {
            result.X = MathHelperD.Lerp(value1.X, value2.X, amount);
            result.Y = MathHelperD.Lerp(value1.Y, value2.Y, amount);
            result.Z = MathHelperD.Lerp(value1.Z, value2.Z, amount);
            result.W = MathHelperD.Lerp(value1.W, value2.W, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector4D"/> with maximal values from the two vectors.</returns>
        public static Vector4D Max(Vector4D value1, Vector4D value2)
        {
            return new Vector4D(
                MathHelperD.Max(value1.X, value2.X),
                MathHelperD.Max(value1.Y, value2.Y),
                MathHelperD.Max(value1.Z, value2.Z),
                MathHelperD.Max(value1.W, value2.W)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector4D"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.X = MathHelperD.Max(value1.X, value2.X);
            result.Y = MathHelperD.Max(value1.Y, value2.Y);
            result.Z = MathHelperD.Max(value1.Z, value2.Z);
            result.W = MathHelperD.Max(value1.W, value2.W);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector4D"/> with minimal values from the two vectors.</returns>
        public static Vector4D Min(Vector4D value1, Vector4D value2)
        {
            return new Vector4D(
                MathHelperD.Min(value1.X, value2.X),
                MathHelperD.Min(value1.Y, value2.Y),
                MathHelperD.Min(value1.Z, value2.Z),
                MathHelperD.Min(value1.W, value2.W)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector4D"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.X = MathHelperD.Min(value1.X, value2.X);
            result.Y = MathHelperD.Min(value1.Y, value2.Y);
            result.Z = MathHelperD.Min(value1.Z, value2.Z);
            result.W = MathHelperD.Min(value1.W, value2.W);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Source <see cref="Vector4D"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Vector4D Multiply(Vector4D value1, Vector4D value2)
        {
            value1.W *= value2.W;
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a multiplication of <see cref="Vector4D"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Vector4D Multiply(Vector4D value1, double scaleFactor)
        {
            value1.W *= scaleFactor;
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            value1.Z *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a multiplication of <see cref="Vector4D"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Vector4D value1, double scaleFactor, out Vector4D result)
        {
            result.W = value1.W * scaleFactor;
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Source <see cref="Vector4D"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.W = value1.W * value2.W;
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Vector4D Negate(Vector4D value)
        {
            value = new Vector4D(-value.X, -value.Y, -value.Z, -value.W);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Vector4D value, out Vector4D result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = -value.W;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Vector4D Normalize(Vector4D vector)
        {
            double factor = 1.0f / (double)Math.Sqrt(
                (vector.X * vector.X) +
                (vector.Y * vector.Y) +
                (vector.Z * vector.Z) +
                (vector.W * vector.W)
            );
            return new Vector4D(
                vector.X * factor,
                vector.Y * factor,
                vector.Z * factor,
                vector.W * factor
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Vector4D vector, out Vector4D result)
        {
            double factor = 1.0f / (double)Math.Sqrt(
                (vector.X * vector.X) +
                (vector.Y * vector.Y) +
                (vector.Z * vector.Z) +
                (vector.W * vector.W)
            );
            result.X = vector.X * factor;
            result.Y = vector.Y * factor;
            result.Z = vector.Z * factor;
            result.W = vector.W * factor;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Source <see cref="Vector4D"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Vector4D SmoothStep(Vector4D value1, Vector4D value2, double amount)
        {
            return new Vector4D(
                MathHelperD.SmoothStep(value1.X, value2.X, amount),
                MathHelperD.SmoothStep(value1.Y, value2.Y, amount),
                MathHelperD.SmoothStep(value1.Z, value2.Z, amount),
                MathHelperD.SmoothStep(value1.W, value2.W, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Source <see cref="Vector4D"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(
            ref Vector4D value1,
            ref Vector4D value2,
            double amount,
            out Vector4D result
        )
        {
            result.X = MathHelperD.SmoothStep(value1.X, value2.X, amount);
            result.Y = MathHelperD.SmoothStep(value1.Y, value2.Y, amount);
            result.Z = MathHelperD.SmoothStep(value1.Z, value2.Z, amount);
            result.W = MathHelperD.SmoothStep(value1.W, value2.W, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains subtraction of on <see cref="Vector4D"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Source <see cref="Vector4D"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vector4D Subtract(Vector4D value1, Vector4D value2)
        {
            value1.W -= value2.W;
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains subtraction of on <see cref="Vector4D"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector4D"/>.</param>
        /// <param name="value2">Source <see cref="Vector4D"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Vector4D value1, ref Vector4D value2, out Vector4D result)
        {
            result.W = value1.W - value2.W;
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 2d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed <see cref="Vector4D"/>.</returns>
        public static Vector4D Transform(Vector2D position, MatrixD matrix)
        {
            Vector4D result;
            Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 3d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed <see cref="Vector4D"/>.</returns>
        public static Vector4D Transform(Vector3D position, MatrixD matrix)
        {
            Vector4D result;
            Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 4d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed <see cref="Vector4D"/>.</returns>
        public static Vector4D Transform(Vector4D vector, MatrixD matrix)
        {
            Transform(ref vector, ref matrix, out vector);
            return vector;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 2d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed <see cref="Vector4D"/> as an output parameter.</param>
        public static void Transform(ref Vector2D position, ref MatrixD matrix, out Vector4D result)
        {
            result = new Vector4D(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42,
                (position.X * matrix.M13) + (position.Y * matrix.M23) + matrix.M43,
                (position.X * matrix.M14) + (position.Y * matrix.M24) + matrix.M44
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 3d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed <see cref="Vector4D"/> as an output parameter.</param>
        public static void Transform(ref Vector3D position, ref MatrixD matrix, out Vector4D result)
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
            double w = (
                (position.X * matrix.M14) +
                (position.Y * matrix.M24) +
                (position.Z * matrix.M34) +
                matrix.M44
            );
            result.X = x;
            result.Y = y;
            result.Z = z;
            result.W = w;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 4d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed <see cref="Vector4D"/> as an output parameter.</param>
        public static void Transform(ref Vector4D vector, ref MatrixD matrix, out Vector4D result)
        {
            double x = (
                (vector.X * matrix.M11) +
                (vector.Y * matrix.M21) +
                (vector.Z * matrix.M31) +
                (vector.W * matrix.M41)
            );
            double y = (
                (vector.X * matrix.M12) +
                (vector.Y * matrix.M22) +
                (vector.Z * matrix.M32) +
                (vector.W * matrix.M42)
            );
            double z = (
                (vector.X * matrix.M13) +
                (vector.Y * matrix.M23) +
                (vector.Z * matrix.M33) +
                (vector.W * matrix.M43)
            );
            double w = (
                (vector.X * matrix.M14) +
                (vector.Y * matrix.M24) +
                (vector.Z * matrix.M34) +
                (vector.W * matrix.M44)
            );
            result.X = x;
            result.Y = y;
            result.Z = z;
            result.W = w;
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector4D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vector4D[] sourceArray,
            ref MatrixD matrix,
            Vector4D[] destinationArray
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
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException(
                    "destinationArray is too small to contain the result."
                );
            }
            for (int i = 0; i < sourceArray.Length; i += 1)
            {
                Transform(
                    ref sourceArray[i],
                    ref matrix,
                    out destinationArray[i]
                );
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector4D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector4D"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vector4D[] sourceArray,
            int sourceIndex,
            ref MatrixD matrix,
            Vector4D[] destinationArray,
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
            if (destinationIndex + length > destinationArray.Length)
            {
                throw new ArgumentException(
                    "destinationArray is too small to contain the result."
                );
            }
            if (sourceIndex + length > sourceArray.Length)
            {
                throw new ArgumentException(
                    "The combination of sourceIndex and length was greater than sourceArray.Length."
                );
            }
            for (int i = 0; i < length; i += 1)
            {
                Transform(
                    ref sourceArray[i + sourceIndex],
                    ref matrix,
                    out destinationArray[i + destinationIndex]
                );
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 2d-vector by the specified <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vector4D"/>.</returns>
        public static Vector4D Transform(Vector2D value, QuaternionD rotation)
        {
            Vector4D result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 3d-vector by the specified <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vector4D"/>.</returns>
        public static Vector4D Transform(Vector3D value, QuaternionD rotation)
        {
            Vector4D result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 4d-vector by the specified <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vector4D"/>.</returns>
        public static Vector4D Transform(Vector4D value, QuaternionD rotation)
        {
            Vector4D result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 2d-vector by the specified <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vector4D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector2D value,
            ref QuaternionD rotation,
            out Vector4D result
        )
        {
            double xx = rotation.X + rotation.X;
            double yy = rotation.Y + rotation.Y;
            double zz = rotation.Z + rotation.Z;
            double wxx = rotation.W * xx;
            double wyy = rotation.W * yy;
            double wzz = rotation.W * zz;
            double xxx = rotation.X * xx;
            double xyy = rotation.X * yy;
            double xzz = rotation.X * zz;
            double yyy = rotation.Y * yy;
            double yzz = rotation.Y * zz;
            double zzz = rotation.Z * zz;
            result.X = (double)(
                (double)value.X * (1.0 - yyy - zzz) +
                (double)value.Y * (xyy - wzz)
            );
            result.Y = (double)(
                (double)value.X * (xyy + wzz) +
                (double)value.Y * (1.0 - xxx - zzz)
            );
            result.Z = (double)(
                (double)value.X * (xzz - wyy) +
                (double)value.Y * (yzz + wxx)
            );
            result.W = 1.0f;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 3d-vector by the specified <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vector4D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector3D value,
            ref QuaternionD rotation,
            out Vector4D result
        )
        {
            double xx = rotation.X + rotation.X;
            double yy = rotation.Y + rotation.Y;
            double zz = rotation.Z + rotation.Z;
            double wxx = rotation.W * xx;
            double wyy = rotation.W * yy;
            double wzz = rotation.W * zz;
            double xxx = rotation.X * xx;
            double xyy = rotation.X * yy;
            double xzz = rotation.X * zz;
            double yyy = rotation.Y * yy;
            double yzz = rotation.Y * zz;
            double zzz = rotation.Z * zz;
            result.X = (double)(
                (double)value.X * (1.0 - yyy - zzz) +
                (double)value.Y * (xyy - wzz) +
                (double)value.Z * (xzz + wyy)
            );
            result.Y = (double)(
                (double)value.X * (xyy + wzz) +
                (double)value.Y * (1.0 - xxx - zzz) +
                (double)value.Z * (yzz - wxx)
            );
            result.Z = (double)(
                (double)value.X * (xzz - wyy) +
                (double)value.Y * (yzz + wxx) +
                (double)value.Z * (1.0 - xxx - yyy)
            );
            result.W = 1.0f;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4D"/> that contains a transformation of 4d-vector by the specified <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector4D"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vector4D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector4D value,
            ref QuaternionD rotation,
            out Vector4D result
        )
        {
            double xx = rotation.X + rotation.X;
            double yy = rotation.Y + rotation.Y;
            double zz = rotation.Z + rotation.Z;
            double wxx = rotation.W * xx;
            double wyy = rotation.W * yy;
            double wzz = rotation.W * zz;
            double xxx = rotation.X * xx;
            double xyy = rotation.X * yy;
            double xzz = rotation.X * zz;
            double yyy = rotation.Y * yy;
            double yzz = rotation.Y * zz;
            double zzz = rotation.Z * zz;
            result.X = (double)(
                (double)value.X * (1.0 - yyy - zzz) +
                (double)value.Y * (xyy - wzz) +
                (double)value.Z * (xzz + wyy)
            );
            result.Y = (double)(
                (double)value.X * (xyy + wzz) +
                (double)value.Y * (1.0 - xxx - zzz) +
                (double)value.Z * (yzz - wxx)
            );
            result.Z = (double)(
                (double)value.X * (xzz - wyy) +
                (double)value.Y * (yzz + wxx) +
                (double)value.Z * (1.0 - xxx - yyy)
            );
            result.W = value.W;
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector4D"/> by the specified <see cref="QuaternionD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vector4D[] sourceArray,
            ref QuaternionD rotation,
            Vector4D[] destinationArray
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException(
                    "destinationArray is too small to contain the result."
                );
            }
            for (int i = 0; i < sourceArray.Length; i += 1)
            {
                Transform(
                    ref sourceArray[i],
                    ref rotation,
                    out destinationArray[i]
                );
            }
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector4D"/> by the specified <see cref="QuaternionD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector4D"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vector4D[] sourceArray,
            int sourceIndex,
            ref QuaternionD rotation,
            Vector4D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            if (sourceArray == null)
            {
                throw new ArgumentException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentException("destinationArray");
            }
            if (destinationIndex + length > destinationArray.Length)
            {
                throw new ArgumentException(
                    "destinationArray is too small to contain the result."
                );
            }
            if (sourceIndex + length > sourceArray.Length)
            {
                throw new ArgumentException(
                    "The combination of sourceIndex and length was greater than sourceArray.Length."
                );
            }
            for (int i = 0; i < length; i += 1)
            {
                Transform(
                    ref sourceArray[i + sourceIndex],
                    ref rotation,
                    out destinationArray[i + destinationIndex]
                );
            }
        }

        #endregion

        #region Public Static Operators

        public static Vector4D operator -(Vector4D value)
        {
            return new Vector4D(-value.X, -value.Y, -value.Z, -value.W);
        }

        public static bool operator ==(Vector4D value1, Vector4D value2)
        {
            return (value1.X == value2.X &&
                    value1.Y == value2.Y &&
                    value1.Z == value2.Z &&
                    value1.W == value2.W);
        }

        public static bool operator !=(Vector4D value1, Vector4D value2)
        {
            return !(value1 == value2);
        }

        public static Vector4D operator +(Vector4D value1, Vector4D value2)
        {
            value1.W += value2.W;
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        public static Vector4D operator -(Vector4D value1, Vector4D value2)
        {
            value1.W -= value2.W;
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        public static Vector4D operator *(Vector4D value1, Vector4D value2)
        {
            value1.W *= value2.W;
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        public static Vector4D operator *(Vector4D value1, double scaleFactor)
        {
            value1.W *= scaleFactor;
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            value1.Z *= scaleFactor;
            return value1;
        }

        public static Vector4D operator *(double scaleFactor, Vector4D value1)
        {
            value1.W *= scaleFactor;
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            value1.Z *= scaleFactor;
            return value1;
        }

        public static Vector4D operator /(Vector4D value1, Vector4D value2)
        {
            value1.W /= value2.W;
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }

        public static Vector4D operator /(Vector4D value1, double divider)
        {
            double factor = 1f / divider;
            value1.W *= factor;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }

        #endregion
    }
}
