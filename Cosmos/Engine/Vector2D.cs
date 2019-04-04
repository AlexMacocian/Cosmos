using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    public struct Vector2D : IEquatable<Vector2D>
    {
        #region Public Static Properties

        /// <summary>
        /// Returns a <see cref="Vector2D"/> with components 0, 0.
        /// </summary>
        public static Vector2D Zero
        {
            get
            {
                return zeroVector;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector2D"/> with components 1, 1.
        /// </summary>
        public static Vector2D One
        {
            get
            {
                return unitVector;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector2D"/> with components 1, 0.
        /// </summary>
        public static Vector2D UnitX
        {
            get
            {
                return unitXVector;
            }
        }

        /// <summary>
        /// Returns a <see cref="Vector2D"/> with components 0, 1.
        /// </summary>
        public static Vector2D UnitY
        {
            get
            {
                return unitYVector;
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
                    Y.ToString()
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Vector2D"/>.
        /// </summary>
        public double X;

        /// <summary>
        /// The y coordinate of this <see cref="Vector2D"/>.
        /// </summary>
        public double Y;

        #endregion

        #region Private Static Fields

        private static readonly Vector2D zeroVector = new Vector2D(0f, 0f);
        private static readonly Vector2D unitVector = new Vector2D(1f, 1f);
        private static readonly Vector2D unitXVector = new Vector2D(1f, 0f);
        private static readonly Vector2D unitYVector = new Vector2D(0f, 1f);

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs a 2d vector with X and Y from two values.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Vector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Constructs a 2d vector with X and Y set to the same value.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Vector2D(double value)
        {
            this.X = value;
            this.Y = value;
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
            return (obj is Vector2D) && Equals((Vector2D)obj);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector2D"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vector2D other)
        {
            return (X == other.X &&
                    Y == other.Y);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vector2D"/>.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Returns the length of this <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Vector2D"/>.</returns>
        public double Length()
        {
            return Math.Sqrt((X * X) + (Y * Y));
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Vector2D"/>.</returns>
        public double LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Turns this <see cref="Vector2D"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            double val = 1.0f / Math.Sqrt((X * X) + (Y * Y));
            X *= val;
            Y *= val;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Vector2D"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Vector2D"/>.</returns>
        public override string ToString()
        {
            return (
                "{X:" + X.ToString() +
                " Y:" + Y.ToString() +
                "}"
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
        public static Vector2D Add(Vector2D value1, Vector2D value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
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
        public static void Add(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Vector2D Barycentric(
            Vector2D value1,
            Vector2D value2,
            Vector2D value3,
            double amount1,
            double amount2
        )
        {
            return new Vector2D(
                MathHelperD.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelperD.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 2d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 2d-triangle.</param>
        /// <param name="value2">The second vector of 2d-triangle.</param>
        /// <param name="value3">The third vector of 2d-triangle.</param>
        /// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 2d-triangle.</param>
        /// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 2d-triangle.</param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(
            ref Vector2D value1,
            ref Vector2D value2,
            ref Vector2D value3,
            double amount1,
            double amount2,
            out Vector2D result
        )
        {
            result.X = MathHelperD.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = MathHelperD.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vector2D CatmullRom(
            Vector2D value1,
            Vector2D value2,
            Vector2D value3,
            Vector2D value4,
            double amount
        )
        {
            return new Vector2D(
                MathHelperD.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelperD.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains CatmullRom interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(
            ref Vector2D value1,
            ref Vector2D value2,
            ref Vector2D value3,
            ref Vector2D value4,
            double amount,
            out Vector2D result
        )
        {
            result.X = MathHelperD.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelperD.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }

        /// <summary>
        /// Clamps the specified value within a range.
        /// </summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector2D Clamp(Vector2D value1, Vector2D min, Vector2D max)
        {
            return new Vector2D(
                MathHelperD.Clamp(value1.X, min.X, max.X),
                MathHelperD.Clamp(value1.Y, min.Y, max.Y)
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
            ref Vector2D value1,
            ref Vector2D min,
            ref Vector2D max,
            out Vector2D result
        )
        {
            result.X = MathHelperD.Clamp(value1.X, min.X, max.X);
            result.Y = MathHelperD.Clamp(value1.Y, min.Y, max.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static double Distance(Vector2D value1, Vector2D value2)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vector2D value1, ref Vector2D value2, out double result)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static double DistanceSquared(Vector2D value1, Vector2D value2)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(
            ref Vector2D value1,
            ref Vector2D value2,
            out double result
        )
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (v1 * v1) + (v2 * v2);
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2D"/> by the components of another <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector2D"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector2D Divide(Vector2D value1, Vector2D value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2D"/> by the components of another <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector2D"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector2D Divide(Vector2D value1, double divider)
        {
            double factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Vector2D value1, double divider, out Vector2D result)
        {
            double factor = 1 / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static double Dot(Vector2D value1, Vector2D value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Vector2D value1, ref Vector2D value2, out double result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vector2D Hermite(
            Vector2D value1,
            Vector2D tangent1,
            Vector2D value2,
            Vector2D tangent2,
            double amount
        )
        {
            Vector2D result = new Vector2D();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains hermite spline interpolation.
        /// </summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(
            ref Vector2D value1,
            ref Vector2D tangent1,
            ref Vector2D value2,
            ref Vector2D tangent2,
            double amount,
            out Vector2D result
        )
        {
            result.X = MathHelperD.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = MathHelperD.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vector2D Lerp(Vector2D value1, Vector2D value2, double amount)
        {
            return new Vector2D(
                MathHelperD.Lerp(value1.X, value2.X, amount),
                MathHelperD.Lerp(value1.Y, value2.Y, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains linear interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(
            ref Vector2D value1,
            ref Vector2D value2,
            double amount,
            out Vector2D result
        )
        {
            result.X = MathHelperD.Lerp(value1.X, value2.X, amount);
            result.Y = MathHelperD.Lerp(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector2D"/> with maximal values from the two vectors.</returns>
        public static Vector2D Max(Vector2D value1, Vector2D value2)
        {
            return new Vector2D(
                value1.X > value2.X ? value1.X : value2.X,
                value1.Y > value2.Y ? value1.Y : value2.Y
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector2D"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector2D"/> with minimal values from the two vectors.</returns>
        public static Vector2D Min(Vector2D value1, Vector2D value2)
        {
            return new Vector2D(
                value1.X < value2.X ? value1.X : value2.X,
                value1.Y < value2.Y ? value1.Y : value2.Y
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector2D"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Source <see cref="Vector2D"/>.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Vector2D Multiply(Vector2D value1, Vector2D value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a multiplication of <see cref="Vector2D"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Vector2D Multiply(Vector2D value1, double scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a multiplication of <see cref="Vector2D"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Vector2D value1, double scaleFactor, out Vector2D result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Source <see cref="Vector2D"/>.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains the specified vector inversion.
        /// direction of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/>.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Vector2D Negate(Vector2D value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains the specified vector inversion.
        /// direction of <paramref name="value"/> in <paramref name="result"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/>.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Vector2D value, out Vector2D result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Vector2D Normalize(Vector2D value)
        {
            double val = 1.0f / Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X *= val;
            value.Y *= val;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Vector2D value, out Vector2D result)
        {
            double val = 1.0f / Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            result.X = value.X * val;
            result.Y = value.Y * val;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vector2D"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Vector2D Reflect(Vector2D vector, Vector2D normal)
        {
            Vector2D result;
            double val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vector2D"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref Vector2D vector, ref Vector2D normal, out Vector2D result)
        {
            double val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Source <see cref="Vector2D"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Vector2D SmoothStep(Vector2D value1, Vector2D value2, double amount)
        {
            return new Vector2D(
                MathHelperD.SmoothStep(value1.X, value2.X, amount),
                MathHelperD.SmoothStep(value1.Y, value2.Y, amount)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains cubic interpolation of the specified vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Source <see cref="Vector2D"/>.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(
            ref Vector2D value1,
            ref Vector2D value2,
            double amount,
            out Vector2D result
        )
        {
            result.X = MathHelperD.SmoothStep(value1.X, value2.X, amount);
            result.Y = MathHelperD.SmoothStep(value1.Y, value2.Y, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains subtraction of on <see cref="Vector2D"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Source <see cref="Vector2D"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vector2D Subtract(Vector2D value1, Vector2D value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains subtraction of on <see cref="Vector2D"/> from a another.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/>.</param>
        /// <param name="value2">Source <see cref="Vector2D"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Vector2D value1, ref Vector2D value2, out Vector2D result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a transformation of 2d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector2D"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed <see cref="Vector2D"/>.</returns>
        public static Vector2D Transform(Vector2D position, MatrixD matrix)
        {
            return new Vector2D(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a transformation of 2d-vector by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector2D"/>.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed <see cref="Vector2D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector2D position,
            ref MatrixD matrix,
            out Vector2D result
        )
        {
            double x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41;
            double y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42;
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a transformation of 2d-vector by the specified <see cref="QuaternionD"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <returns>Transformed <see cref="Vector2D"/>.</returns>
        public static Vector2D Transform(Vector2D value, QuaternionD rotation)
        {
            Transform(ref value, ref rotation, out value);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a transformation of 2d-vector by the specified <see cref="QuaternionD"/>, representing the rotation.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/>.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="result">Transformed <see cref="Vector2D"/> as an output parameter.</param>
        public static void Transform(
            ref Vector2D value,
            ref QuaternionD rotation,
            out Vector2D result
        )
        {
            double x = 2 * -(rotation.Z * value.Y);
            double y = 2 * (rotation.Z * value.X);
            double z = 2 * (rotation.X * value.Y - rotation.Y * value.X);

            result.X = value.X + x * rotation.W + (rotation.Y * z - rotation.Z * y);
            result.Y = value.Y + y * rotation.W + (rotation.Z * x - rotation.X * z);
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector2D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vector2D[] sourceArray,
            ref MatrixD matrix,
            Vector2D[] destinationArray
        )
        {
            Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector2D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector2D"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vector2D[] sourceArray,
            int sourceIndex,
            ref MatrixD matrix,
            Vector2D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            for (int x = 0; x < length; x += 1)
            {
                Vector2D position = sourceArray[sourceIndex + x];
                Vector2D destination = destinationArray[destinationIndex + x];
                destination.X = (position.X * matrix.M11) + (position.Y * matrix.M21)
                        + matrix.M41;
                destination.Y = (position.X * matrix.M12) + (position.Y * matrix.M22)
                        + matrix.M42;
                destinationArray[destinationIndex + x] = destination;
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector2D"/> by the specified <see cref="QuaternionD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(
            Vector2D[] sourceArray,
            ref QuaternionD rotation,
            Vector2D[] destinationArray
        )
        {
            Transform(
                sourceArray,
                0,
                ref rotation,
                destinationArray,
                0,
                sourceArray.Length
            );
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector2D"/> by the specified <see cref="QuaternionD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="rotation">The <see cref="QuaternionD"/> which contains rotation transformation.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector2D"/> should be written.</param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(
            Vector2D[] sourceArray,
            int sourceIndex,
            ref QuaternionD rotation,
            Vector2D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            for (int i = 0; i < length; i += 1)
            {
                Vector2D position = sourceArray[sourceIndex + i];
                Vector2D v;
                Transform(ref position, ref rotation, out v);
                destinationArray[destinationIndex + i] = v;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a transformation of the specified normal by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector2D"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <returns>Transformed normal.</returns>
        public static Vector2D TransformNormal(Vector2D normal, MatrixD matrix)
        {
            return new Vector2D(
                (normal.X * matrix.M11) + (normal.Y * matrix.M21),
                (normal.X * matrix.M12) + (normal.Y * matrix.M22)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2D"/> that contains a transformation of the specified normal by the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector2D"/> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(
            ref Vector2D normal,
            ref MatrixD matrix,
            out Vector2D result
        )
        {
            double x = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            double y = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = x;
            result.Y = y;
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Vector2D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal(
            Vector2D[] sourceArray,
            ref MatrixD matrix,
            Vector2D[] destinationArray
        )
        {
            TransformNormal(
                sourceArray,
                0,
                ref matrix,
                destinationArray,
                0,
                sourceArray.Length
            );
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Vector2D"/> by the specified <see cref="MatrixD"/> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="MatrixD"/>.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector2D"/> should be written.</param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal(
            Vector2D[] sourceArray,
            int sourceIndex,
            ref MatrixD matrix,
            Vector2D[] destinationArray,
            int destinationIndex,
            int length
        )
        {
            for (int i = 0; i < length; i += 1)
            {
                Vector2D position = sourceArray[sourceIndex + i];
                Vector2D result;
                result.X = (position.X * matrix.M11) + (position.Y * matrix.M21);
                result.Y = (position.X * matrix.M12) + (position.Y * matrix.M22);
                destinationArray[destinationIndex + i] = result;
            }
        }

        #endregion

        #region Public Static Operators

        /// <summary>
        /// Inverts values in the specified <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vector2D operator -(Vector2D value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Compares whether two <see cref="Vector2D"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector2D"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vector2D"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Vector2D value1, Vector2D value2)
        {
            return (value1.X == value2.X &&
                    value1.Y == value2.Y);
        }

        /// <summary>
        /// Compares whether two <see cref="Vector2D"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector2D"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vector2D"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Vector2D value1, Vector2D value2)
        {
            return !(value1 == value2);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vector2D"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vector2D operator +(Vector2D value1, Vector2D value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="Vector2D"/> from a <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vector2D"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vector2D operator -(Vector2D value1, Vector2D value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vector2D"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vector2D operator *(Vector2D value1, Vector2D value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2D"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2D operator *(Vector2D value, double scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vector2D"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2D operator *(double scaleFactor, Vector2D value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2D"/> by the components of another <see cref="Vector2D"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vector2D"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector2D operator /(Vector2D value1, Vector2D value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2D"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2D"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector2D operator /(Vector2D value1, double divider)
        {
            double factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        #endregion
    }
}

