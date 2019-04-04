using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Engine
{
    /// <summary>
    /// Represents the right-handed 4x4 double point matrix, which can store translation, scale and rotation information.
    /// </summary>
    [Serializable]
    public struct MatrixD : IEquatable<MatrixD>
    {
        #region Public Properties

        /// <summary>
        /// The backward vector formed from the third row M31, M32, M33 elements.
        /// </summary>
        public Vector3D Backward
        {
            get
            {
                return new Vector3D(M31, M32, M33);
            }
            set
            {
                M31 = value.X;
                M32 = value.Y;
                M33 = value.Z;
            }
        }

        /// <summary>
        /// The down vector formed from the second row -M21, -M22, -M23 elements.
        /// </summary>
        public Vector3D Down
        {
            get
            {
                return new Vector3D(-M21, -M22, -M23);
            }
            set
            {
                M21 = -value.X;
                M22 = -value.Y;
                M23 = -value.Z;
            }
        }

        /// <summary>
        /// The forward vector formed from the third row -M31, -M32, -M33 elements.
        /// </summary>
        public Vector3D Forward
        {
            get
            {
                return new Vector3D(-M31, -M32, -M33);
            }
            set
            {
                M31 = -value.X;
                M32 = -value.Y;
                M33 = -value.Z;
            }
        }

        /// <summary>
        /// Returns the identity matrix.
        /// </summary>
        public static MatrixD Identity
        {
            get
            {
                return identity;
            }
        }

        /// <summary>
        /// The left vector formed from the first row -M11, -M12, -M13 elements.
        /// </summary>
        public Vector3D Left
        {
            get
            {
                return new Vector3D(-M11, -M12, -M13);
            }
            set
            {
                M11 = -value.X;
                M12 = -value.Y;
                M13 = -value.Z;
            }
        }

        /// <summary>
        /// The right vector formed from the first row M11, M12, M13 elements.
        /// </summary>
        public Vector3D Right
        {
            get
            {
                return new Vector3D(M11, M12, M13);
            }
            set
            {
                M11 = value.X;
                M12 = value.Y;
                M13 = value.Z;
            }
        }

        /// <summary>
        /// Position stored in this matrix.
        /// </summary>
        public Vector3D Translation
        {
            get
            {
                return new Vector3D(M41, M42, M43);
            }
            set
            {
                M41 = value.X;
                M42 = value.Y;
                M43 = value.Z;
            }
        }

        /// <summary>
        /// The upper vector formed from the second row M21, M22, M23 elements.
        /// </summary>
        public Vector3D Up
        {
            get
            {
                return new Vector3D(M21, M22, M23);
            }
            set
            {
                M21 = value.X;
                M22 = value.Y;
                M23 = value.Z;
            }
        }

        public Microsoft.Xna.Framework.Matrix XNAMatrix
        {
            get
            {
                return new Microsoft.Xna.Framework.Matrix((float)M11, (float)M12, (float)M13, (float)M14,
                                                          (float)M21, (float)M22, (float)M23, (float)M24,
                                                          (float)M31, (float)M32, (float)M33, (float)M34,
                                                          (float)M41, (float)M42, (float)M43, (float)M44);
            }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    "( ", M11.ToString(), " ",
                    M12.ToString(), " ",
                    M13.ToString(), " ",
                    M14.ToString(), " ) \r\n",
                    "( ", M21.ToString(), " ",
                    M22.ToString(), " ",
                    M23.ToString(), " ",
                    M24.ToString(), " ) \r\n",
                    "( ", M31.ToString(), " ",
                    M32.ToString(), " ",
                    M33.ToString(), " ",
                    M34.ToString(), " ) \r\n",
                    "( ", M41.ToString(), " ",
                    M42.ToString(), " ",
                    M43.ToString(), " ",
                    M44.ToString(), " )"
                );
            }
        }

        #endregion

        #region Public Fields

        /// <summary>
        /// A first row and first column value.
        /// </summary>
        public double M11;

        /// <summary>
        /// A first row and second column value.
        /// </summary>
        public double M12;

        /// <summary>
        /// A first row and third column value.
        /// </summary>
        public double M13;

        /// <summary>
        /// A first row and fourth column value.
        /// </summary>
        public double M14;

        /// <summary>
        /// A second row and first column value.
        /// </summary>
        public double M21;

        /// <summary>
        /// A second row and second column value.
        /// </summary>
        public double M22;

        /// <summary>
        /// A second row and third column value.
        /// </summary>
        public double M23;

        /// <summary>
        /// A second row and fourth column value.
        /// </summary>
        public double M24;

        /// <summary>
        /// A third row and first column value.
        /// </summary>
        public double M31;

        /// <summary>
        /// A third row and second column value.
        /// </summary>
        public double M32;

        /// <summary>
        /// A third row and third column value.
        /// </summary>
        public double M33;

        /// <summary>
        /// A third row and fourth column value.
        /// </summary>
        public double M34;

        /// <summary>
        /// A fourth row and first column value.
        /// </summary>
        public double M41;

        /// <summary>
        /// A fourth row and second column value.
        /// </summary>
        public double M42;

        /// <summary>
        /// A fourth row and third column value.
        /// </summary>
        public double M43;

        /// <summary>
        /// A fourth row and fourth column value.
        /// </summary>
        public double M44;

        #endregion

        #region Private Static Variables

        private static MatrixD identity = new MatrixD(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f
        );

        #endregion

        #region Public Constructors

        /// <summary>
        /// Constructs a matrix.
        /// </summary>
        /// <param name="m11">A first row and first column value.</param>
        /// <param name="m12">A first row and second column value.</param>
        /// <param name="m13">A first row and third column value.</param>
        /// <param name="m14">A first row and fourth column value.</param>
        /// <param name="m21">A second row and first column value.</param>
        /// <param name="m22">A second row and second column value.</param>
        /// <param name="m23">A second row and third column value.</param>
        /// <param name="m24">A second row and fourth column value.</param>
        /// <param name="m31">A third row and first column value.</param>
        /// <param name="m32">A third row and second column value.</param>
        /// <param name="m33">A third row and third column value.</param>
        /// <param name="m34">A third row and fourth column value.</param>
        /// <param name="m41">A fourth row and first column value.</param>
        /// <param name="m42">A fourth row and second column value.</param>
        /// <param name="m43">A fourth row and third column value.</param>
        /// <param name="m44">A fourth row and fourth column value.</param>
        public MatrixD(
            double m11, double m12, double m13, double m14,
            double m21, double m22, double m23, double m24,
            double m31, double m32, double m33, double m34,
            double m41, double m42, double m43, double m44
        )
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Decomposes this matrix to translation, rotation and scale elements. Returns <c>true</c> if matrix can be decomposed; <c>false</c> otherwise.
        /// </summary>
        /// <param name="scale">Scale vector as an output parameter.</param>
        /// <param name="rotation">Rotation quaternion as an output parameter.</param>
        /// <param name="translation">Translation vector as an output parameter.</param>
        /// <returns><c>true</c> if matrix can be decomposed; <c>false</c> otherwise.</returns>
        public bool Decompose(
            out Vector3D scale,
            out QuaternionD rotation,
            out Vector3D translation
        )
        {
            translation.X = M41;
            translation.Y = M42;
            translation.Z = M43;

            double xs = (Math.Sign(M11 * M12 * M13 * M14) < 0) ? -1 : 1;
            double ys = (Math.Sign(M21 * M22 * M23 * M24) < 0) ? -1 : 1;
            double zs = (Math.Sign(M31 * M32 * M33 * M34) < 0) ? -1 : 1;

            scale.X = xs * (double)Math.Sqrt(M11 * M11 + M12 * M12 + M13 * M13);
            scale.Y = ys * (double)Math.Sqrt(M21 * M21 + M22 * M22 + M23 * M23);
            scale.Z = zs * (double)Math.Sqrt(M31 * M31 + M32 * M32 + M33 * M33);

            if (MathHelperD.WithinEpsilon(scale.X, 0.0f) ||
                MathHelperD.WithinEpsilon(scale.Y, 0.0f) ||
                MathHelperD.WithinEpsilon(scale.Z, 0.0f))
            {
                rotation = QuaternionD.Identity;
                return false;
            }

            MatrixD m1 = new MatrixD(
                M11 / scale.X, M12 / scale.X, M13 / scale.X, 0,
                M21 / scale.Y, M22 / scale.Y, M23 / scale.Y, 0,
                M31 / scale.Z, M32 / scale.Z, M33 / scale.Z, 0,
                0, 0, 0, 1
            );

            rotation = QuaternionD.CreateFromRotationMatrix(m1);
            return true;
        }

        /// <summary>
        /// Returns a determinant of this <see cref="MatrixD"/>.
        /// </summary>
        /// <returns>Determinant of this <see cref="MatrixD"/></returns>
        /// <remarks>See more about determinant here - http://en.wikipedia.org/wiki/Determinant.
        /// </remarks>
        public double Determinant()
        {
            double num18 = (M33 * M44) - (M34 * M43);
            double num17 = (M32 * M44) - (M34 * M42);
            double num16 = (M32 * M43) - (M33 * M42);
            double num15 = (M31 * M44) - (M34 * M41);
            double num14 = (M31 * M43) - (M33 * M41);
            double num13 = (M31 * M42) - (M32 * M41);
            return (
                (
                    (
                        (M11 * (((M22 * num18) - (M23 * num17)) + (M24 * num16))) -
                        (M12 * (((M21 * num18) - (M23 * num15)) + (M24 * num14)))
                    ) + (M13 * (((M21 * num17) - (M22 * num15)) + (M24 * num13)))
                ) - (M14 * (((M21 * num16) - (M22 * num14)) + (M23 * num13)))
            );
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="MatrixD"/> without any tolerance.
        /// </summary>
        /// <param name="other">The <see cref="MatrixD"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(MatrixD other)
        {
            return (M11 == other.M11 &&
                    M12 == other.M12 &&
                    M13 == other.M13 &&
                    M14 == other.M14 &&
                    M21 == other.M21 &&
                    M22 == other.M22 &&
                    M23 == other.M23 &&
                    M24 == other.M24 &&
                    M31 == other.M31 &&
                    M32 == other.M32 &&
                    M33 == other.M33 &&
                    M34 == other.M34 &&
                    M41 == other.M41 &&
                    M42 == other.M42 &&
                    M43 == other.M43 &&
                    M44 == other.M44);
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/> without any tolerance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is MatrixD) && Equals((MatrixD)obj);
        }

        /// <summary>
        /// Gets the hash code of this <see cref="MatrixD"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="MatrixD"/>.</returns>
        public override int GetHashCode()
        {
            return (
                M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
                M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
                M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
                M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode()
            );
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="MatrixD"/> in the format:
        /// {M11:[<see cref="M11"/>] M12:[<see cref="M12"/>] M13:[<see cref="M13"/>] M14:[<see cref="M14"/>]}
        /// {M21:[<see cref="M21"/>] M12:[<see cref="M22"/>] M13:[<see cref="M23"/>] M14:[<see cref="M24"/>]}
        /// {M31:[<see cref="M31"/>] M32:[<see cref="M32"/>] M33:[<see cref="M33"/>] M34:[<see cref="M34"/>]}
        /// {M41:[<see cref="M41"/>] M42:[<see cref="M42"/>] M43:[<see cref="M43"/>] M44:[<see cref="M44"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="MatrixD"/>.</returns>
        public override string ToString()
        {
            return (
                "{M11:" + M11.ToString() +
                " M12:" + M12.ToString() +
                " M13:" + M13.ToString() +
                " M14:" + M14.ToString() +
                "} {M21:" + M21.ToString() +
                " M22:" + M22.ToString() +
                " M23:" + M23.ToString() +
                " M24:" + M24.ToString() +
                "} {M31:" + M31.ToString() +
                " M32:" + M32.ToString() +
                " M33:" + M33.ToString() +
                " M34:" + M34.ToString() +
                "} {M41:" + M41.ToString() +
                " M42:" + M42.ToString() +
                " M43:" + M43.ToString() +
                " M44:" + M44.ToString() + "}"
            );
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> which contains sum of two matrixes.
        /// </summary>
        /// <param name="matrix1">The first matrix to add.</param>
        /// <param name="matrix2">The second matrix to add.</param>
        /// <returns>The result of the matrix addition.</returns>
        public static MatrixD Add(MatrixD matrix1, MatrixD matrix2)
        {
            matrix1.M11 += matrix2.M11;
            matrix1.M12 += matrix2.M12;
            matrix1.M13 += matrix2.M13;
            matrix1.M14 += matrix2.M14;
            matrix1.M21 += matrix2.M21;
            matrix1.M22 += matrix2.M22;
            matrix1.M23 += matrix2.M23;
            matrix1.M24 += matrix2.M24;
            matrix1.M31 += matrix2.M31;
            matrix1.M32 += matrix2.M32;
            matrix1.M33 += matrix2.M33;
            matrix1.M34 += matrix2.M34;
            matrix1.M41 += matrix2.M41;
            matrix1.M42 += matrix2.M42;
            matrix1.M43 += matrix2.M43;
            matrix1.M44 += matrix2.M44;
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> which contains sum of two matrixes.
        /// </summary>
        /// <param name="matrix1">The first matrix to add.</param>
        /// <param name="matrix2">The second matrix to add.</param>
        /// <param name="result">The result of the matrix addition as an output parameter.</param>
        public static void Add(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M13 = matrix1.M13 + matrix2.M13;
            result.M14 = matrix1.M14 + matrix2.M14;
            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
            result.M23 = matrix1.M23 + matrix2.M23;
            result.M24 = matrix1.M24 + matrix2.M24;
            result.M31 = matrix1.M31 + matrix2.M31;
            result.M32 = matrix1.M32 + matrix2.M32;
            result.M33 = matrix1.M33 + matrix2.M33;
            result.M34 = matrix1.M34 + matrix2.M34;
            result.M41 = matrix1.M41 + matrix2.M41;
            result.M42 = matrix1.M42 + matrix2.M42;
            result.M43 = matrix1.M43 + matrix2.M43;
            result.M44 = matrix1.M44 + matrix2.M44;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> for spherical billboarding that rotates around specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of billboard object. It will rotate around that vector.</param>
        /// <param name="cameraPosition">The camera position.</param>
        /// <param name="cameraUpVector">The camera up vector.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <returns>The <see cref="MatrixD"/> for spherical billboarding.</returns>
        public static MatrixD CreateBillboard(
            Vector3D objectPosition,
            Vector3D cameraPosition,
            Vector3D cameraUpVector,
            Nullable<Vector3D> cameraForwardVector
        )
        {
            MatrixD result;

            // Delegate to the other overload of the function to do the work
            CreateBillboard(
                ref objectPosition,
                ref cameraPosition,
                ref cameraUpVector,
                cameraForwardVector,
                out result
            );

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> for spherical billboarding that rotates around specified object position.
        /// </summary>
        /// <param name="objectPosition">Position of billboard object. It will rotate around that vector.</param>
        /// <param name="cameraPosition">The camera position.</param>
        /// <param name="cameraUpVector">The camera up vector.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="result">The <see cref="MatrixD"/> for spherical billboarding as an output parameter.</param>
        public static void CreateBillboard(
            ref Vector3D objectPosition,
            ref Vector3D cameraPosition,
            ref Vector3D cameraUpVector,
            Vector3D? cameraForwardVector,
            out MatrixD result
        )
        {
            Vector3D vector;
            Vector3D vector2;
            Vector3D vector3;
            vector.X = objectPosition.X - cameraPosition.X;
            vector.Y = objectPosition.Y - cameraPosition.Y;
            vector.Z = objectPosition.Z - cameraPosition.Z;
            double num = vector.LengthSquared();
            if (num < 0.0001f)
            {
                vector = cameraForwardVector.HasValue ?
                    -cameraForwardVector.Value :
                    Vector3D.Forward;
            }
            else
            {
                Vector3D.Multiply(
                    ref vector,
                    (double)(1f / ((double)Math.Sqrt((double)num))),
                    out vector
                );
            }
            Vector3D.Cross(ref cameraUpVector, ref vector, out vector3);
            vector3.Normalize();
            Vector3D.Cross(ref vector, ref vector3, out vector2);
            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0;
            result.M21 = vector2.X;
            result.M22 = vector2.Y;
            result.M23 = vector2.Z;
            result.M24 = 0;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> for cylindrical billboarding that rotates around specified axis.
        /// </summary>
        /// <param name="objectPosition">Object position the billboard will rotate around.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <param name="rotateAxis">Axis of billboard for rotation.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="objectForwardVector">Optional object forward vector.</param>
        /// <returns>The <see cref="MatrixD"/> for cylindrical billboarding.</returns>
        public static MatrixD CreateConstrainedBillboard(
            Vector3D objectPosition,
            Vector3D cameraPosition,
            Vector3D rotateAxis,
            Nullable<Vector3D> cameraForwardVector,
            Nullable<Vector3D> objectForwardVector
        )
        {
            MatrixD result;
            CreateConstrainedBillboard(
                ref objectPosition,
                ref cameraPosition,
                ref rotateAxis,
                cameraForwardVector,
                objectForwardVector,
                out result
            );
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> for cylindrical billboarding that rotates around specified axis.
        /// </summary>
        /// <param name="objectPosition">Object position the billboard will rotate around.</param>
        /// <param name="cameraPosition">Camera position.</param>
        /// <param name="rotateAxis">Axis of billboard for rotation.</param>
        /// <param name="cameraForwardVector">Optional camera forward vector.</param>
        /// <param name="objectForwardVector">Optional object forward vector.</param>
        /// <param name="result">The <see cref="MatrixD"/> for cylindrical billboarding as an output parameter.</param>
        public static void CreateConstrainedBillboard(
            ref Vector3D objectPosition,
            ref Vector3D cameraPosition,
            ref Vector3D rotateAxis,
            Vector3D? cameraForwardVector,
            Vector3D? objectForwardVector,
            out MatrixD result
        )
        {
            double num;
            Vector3D vector;
            Vector3D vector2;
            Vector3D vector3;
            vector2.X = objectPosition.X - cameraPosition.X;
            vector2.Y = objectPosition.Y - cameraPosition.Y;
            vector2.Z = objectPosition.Z - cameraPosition.Z;
            double num2 = vector2.LengthSquared();
            if (num2 < 0.0001f)
            {
                vector2 = cameraForwardVector.HasValue ?
                    -cameraForwardVector.Value :
                    Vector3D.Forward;
            }
            else
            {
                Vector3D.Multiply(
                    ref vector2,
                    (double)(1f / ((double)Math.Sqrt((double)num2))),
                    out vector2
                );
            }
            Vector3D vector4 = rotateAxis;
            Vector3D.Dot(ref rotateAxis, ref vector2, out num);
            if (Math.Abs(num) > 0.9982547f)
            {
                if (objectForwardVector.HasValue)
                {
                    vector = objectForwardVector.Value;
                    Vector3D.Dot(ref rotateAxis, ref vector, out num);
                    if (Math.Abs(num) > 0.9982547f)
                    {
                        num = (
                            (rotateAxis.X * Vector3D.Forward.X) +
                            (rotateAxis.Y * Vector3D.Forward.Y)
                        ) + (rotateAxis.Z * Vector3D.Forward.Z);
                        vector = (Math.Abs(num) > 0.9982547f) ?
                            Vector3D.Right :
                            Vector3D.Forward;
                    }
                }
                else
                {
                    num = (
                        (rotateAxis.X * Vector3D.Forward.X) +
                        (rotateAxis.Y * Vector3D.Forward.Y)
                    ) + (rotateAxis.Z * Vector3D.Forward.Z);
                    vector = (Math.Abs(num) > 0.9982547f) ?
                        Vector3D.Right :
                        Vector3D.Forward;
                }
                Vector3D.Cross(ref rotateAxis, ref vector, out vector3);
                vector3.Normalize();
                Vector3D.Cross(ref vector3, ref rotateAxis, out vector);
                vector.Normalize();
            }
            else
            {
                Vector3D.Cross(ref rotateAxis, ref vector2, out vector3);
                vector3.Normalize();
                Vector3D.Cross(ref vector3, ref vector4, out vector);
                vector.Normalize();
            }

            result.M11 = vector3.X;
            result.M12 = vector3.Y;
            result.M13 = vector3.Z;
            result.M14 = 0;
            result.M21 = vector4.X;
            result.M22 = vector4.Y;
            result.M23 = vector4.Z;
            result.M24 = 0;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0;
            result.M41 = objectPosition.X;
            result.M42 = objectPosition.Y;
            result.M43 = objectPosition.Z;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> which contains the rotation moment around specified axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation in radians.</param>
        /// <returns>The rotation <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateFromAxisAngle(Vector3D axis, double angle)
        {
            MatrixD result;
            CreateFromAxisAngle(ref axis, angle, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> which contains the rotation moment around specified axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation in radians.</param>
        /// <param name="result">The rotation <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateFromAxisAngle(
            ref Vector3D axis,
            double angle,
            out MatrixD result
        )
        {
            double x = axis.X;
            double y = axis.Y;
            double z = axis.Z;
            double num2 = (double)Math.Sin((double)angle);
            double num = (double)Math.Cos((double)angle);
            double num11 = x * x;
            double num10 = y * y;
            double num9 = z * z;
            double num8 = x * y;
            double num7 = x * z;
            double num6 = y * z;
            result.M11 = num11 + (num * (1f - num11));
            result.M12 = (num8 - (num * num8)) + (num2 * z);
            result.M13 = (num7 - (num * num7)) - (num2 * y);
            result.M14 = 0;
            result.M21 = (num8 - (num * num8)) - (num2 * z);
            result.M22 = num10 + (num * (1f - num10));
            result.M23 = (num6 - (num * num6)) + (num2 * x);
            result.M24 = 0;
            result.M31 = (num7 - (num * num7)) + (num2 * y);
            result.M32 = (num6 - (num * num6)) - (num2 * x);
            result.M33 = num9 + (num * (1f - num9));
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> from a <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="quaternion"><see cref="QuaternionD"/> of rotation moment.</param>
        /// <returns>The rotation <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateFromQuaternion(QuaternionD quaternion)
        {
            MatrixD result;
            CreateFromQuaternion(ref quaternion, out result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> from a <see cref="QuaternionD"/>.
        /// </summary>
        /// <param name="quaternion"><see cref="QuaternionD"/> of rotation moment.</param>
        /// <param name="result">The rotation <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateFromQuaternion(ref QuaternionD quaternion, out MatrixD result)
        {
            double num9 = quaternion.X * quaternion.X;
            double num8 = quaternion.Y * quaternion.Y;
            double num7 = quaternion.Z * quaternion.Z;
            double num6 = quaternion.X * quaternion.Y;
            double num5 = quaternion.Z * quaternion.W;
            double num4 = quaternion.Z * quaternion.X;
            double num3 = quaternion.Y * quaternion.W;
            double num2 = quaternion.Y * quaternion.Z;
            double num = quaternion.X * quaternion.W;
            result.M11 = 1f - (2f * (num8 + num7));
            result.M12 = 2f * (num6 + num5);
            result.M13 = 2f * (num4 - num3);
            result.M14 = 0f;
            result.M21 = 2f * (num6 - num5);
            result.M22 = 1f - (2f * (num7 + num9));
            result.M23 = 2f * (num2 + num);
            result.M24 = 0f;
            result.M31 = 2f * (num4 + num3);
            result.M32 = 2f * (num2 - num);
            result.M33 = 1f - (2f * (num8 + num9));
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> from the specified yaw, pitch and roll values.
        /// </summary>
        /// <param name="yaw">The yaw rotation value in radians.</param>
        /// <param name="pitch">The pitch rotation value in radians.</param>
        /// <param name="roll">The roll rotation value in radians.</param>
        /// <returns>The rotation <see cref="MatrixD"/>.</returns>
        /// <remarks>For more information about yaw, pitch and roll visit http://en.wikipedia.org/wiki/Euler_angles.
        /// </remarks>
        public static MatrixD CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            MatrixD matrix;
            CreateFromYawPitchRoll(yaw, pitch, roll, out matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> from the specified yaw, pitch and roll values.
        /// </summary>
        /// <param name="yaw">The yaw rotation value in radians.</param>
        /// <param name="pitch">The pitch rotation value in radians.</param>
        /// <param name="roll">The roll rotation value in radians.</param>
        /// <param name="result">The rotation <see cref="MatrixD"/> as an output parameter.</param>
        /// <remarks>For more information about yaw, pitch and roll visit http://en.wikipedia.org/wiki/Euler_angles.
        /// </remarks>
        public static void CreateFromYawPitchRoll(
            double yaw,
            double pitch,
            double roll,
            out MatrixD result
        )
        {
            QuaternionD quaternion;
            QuaternionD.CreateFromYawPitchRoll(yaw, pitch, roll, out quaternion);
            CreateFromQuaternion(ref quaternion, out result);
        }

        /// <summary>
        /// Creates a new viewing <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraTarget">Lookup vector of the camera.</param>
        /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
        /// <returns>The viewing <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateLookAt(
            Vector3D cameraPosition,
            Vector3D cameraTarget,
            Vector3D cameraUpVector
        )
        {
            MatrixD matrix;
            CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new viewing <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="cameraPosition">Position of the camera.</param>
        /// <param name="cameraTarget">Lookup vector of the camera.</param>
        /// <param name="cameraUpVector">The direction of the upper edge of the camera.</param>
        /// <param name="result">The viewing <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateLookAt(
            ref Vector3D cameraPosition,
            ref Vector3D cameraTarget,
            ref Vector3D cameraUpVector,
            out MatrixD result
        )
        {
            Vector3D vectorA = Vector3D.Normalize(cameraPosition - cameraTarget);
            Vector3D vectorB = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, vectorA));
            Vector3D vectorC = Vector3D.Cross(vectorA, vectorB);
            result.M11 = vectorB.X;
            result.M12 = vectorC.X;
            result.M13 = vectorA.X;
            result.M14 = 0f;
            result.M21 = vectorB.Y;
            result.M22 = vectorC.Y;
            result.M23 = vectorA.Y;
            result.M24 = 0f;
            result.M31 = vectorB.Z;
            result.M32 = vectorC.Z;
            result.M33 = vectorA.Z;
            result.M34 = 0f;
            result.M41 = -Vector3D.Dot(vectorB, cameraPosition);
            result.M42 = -Vector3D.Dot(vectorC, cameraPosition);
            result.M43 = -Vector3D.Dot(vectorA, cameraPosition);
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for orthographic view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="MatrixD"/> for orthographic view.</returns>
        public static MatrixD CreateOrthographic(
            double width,
            double height,
            double zNearPlane,
            double zFarPlane
        )
        {
            MatrixD matrix;
            CreateOrthographic(width, height, zNearPlane, zFarPlane, out matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for orthographic view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <param name="result">The new projection <see cref="MatrixD"/> for orthographic view as an output parameter.</param>
        public static void CreateOrthographic(
            double width,
            double height,
            double zNearPlane,
            double zFarPlane,
            out MatrixD result
        )
        {
            result.M11 = 2f / width;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = 2f / height;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M33 = 1f / (zNearPlane - zFarPlane);
            result.M31 = result.M32 = result.M34 = 0f;
            result.M41 = result.M42 = 0f;
            result.M43 = zNearPlane / (zNearPlane - zFarPlane);
            result.M44 = 1f;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for customized orthographic view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <returns>The new projection <see cref="MatrixD"/> for customized orthographic view.</returns>
        public static MatrixD CreateOrthographicOffCenter(
            double left,
            double right,
            double bottom,
            double top,
            double zNearPlane,
            double zFarPlane
        )
        {
            MatrixD matrix;
            CreateOrthographicOffCenter(
                left,
                right,
                bottom,
                top,
                zNearPlane,
                zFarPlane,
                out matrix
            );
            return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for customized orthographic view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="zNearPlane">Depth of the near plane.</param>
        /// <param name="zFarPlane">Depth of the far plane.</param>
        /// <param name="result">The new projection <see cref="MatrixD"/> for customized orthographic view as an output parameter.</param>
        public static void CreateOrthographicOffCenter(
            double left,
            double right,
            double bottom,
            double top,
            double zNearPlane,
            double zFarPlane,
            out MatrixD result
        )
        {
            result.M11 = (double)(2.0 / ((double)right - (double)left));
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = (double)(2.0 / ((double)top - (double)bottom));
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = (double)(1.0 / ((double)zNearPlane - (double)zFarPlane));
            result.M34 = 0.0f;
            result.M41 = (double)(
                ((double)left + (double)right) /
                ((double)left - (double)right)
            );
            result.M42 = (double)(
                ((double)top + (double)bottom) /
                ((double)bottom - (double)top)
            );
            result.M43 = (double)(
                (double)zNearPlane /
                ((double)zNearPlane - (double)zFarPlane)
            );
            result.M44 = 1.0f;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for perspective view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new projection <see cref="MatrixD"/> for perspective view.</returns>
        public static MatrixD CreatePerspective(
            double width,
            double height,
            double nearPlaneDistance,
            double farPlaneDistance
        )
        {
            MatrixD matrix;
            CreatePerspective(width, height, nearPlaneDistance, farPlaneDistance, out matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for perspective view.
        /// </summary>
        /// <param name="width">Width of the viewing volume.</param>
        /// <param name="height">Height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <param name="result">The new projection <see cref="MatrixD"/> for perspective view as an output parameter.</param>
        public static void CreatePerspective(
            double width,
            double height,
            double nearPlaneDistance,
            double farPlaneDistance,
            out MatrixD result
        )
        {
            if (nearPlaneDistance <= 0f)
            {
                throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
                throw new ArgumentException("farPlaneDistance <= 0");
            }
            if (nearPlaneDistance >= farPlaneDistance)
            {
                throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }
            result.M11 = (2f * nearPlaneDistance) / width;
            result.M12 = result.M13 = result.M14 = 0f;
            result.M22 = (2f * nearPlaneDistance) / height;
            result.M21 = result.M23 = result.M24 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M31 = result.M32 = 0f;
            result.M34 = -1f;
            result.M41 = result.M42 = result.M44 = 0f;
            result.M43 = (
                (nearPlaneDistance * farPlaneDistance) /
                (nearPlaneDistance - farPlaneDistance)
            );
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for perspective view with field of view.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
        /// <param name="aspectRatio">Width divided by height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new projection <see cref="MatrixD"/> for perspective view with FOV.</returns>
        public static MatrixD CreatePerspectiveFieldOfView(
            double fieldOfView,
            double aspectRatio,
            double nearPlaneDistance,
            double farPlaneDistance
        )
        {
            MatrixD result;
            CreatePerspectiveFieldOfView(
                fieldOfView,
                aspectRatio,
                nearPlaneDistance,
                farPlaneDistance,
                out result
            );
            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for perspective view with field of view.
        /// </summary>
        /// <param name="fieldOfView">Field of view in the y direction in radians.</param>
        /// <param name="aspectRatio">Width divided by height of the viewing volume.</param>
        /// <param name="nearPlaneDistance">Distance of the near plane.</param>
        /// <param name="farPlaneDistance">Distance of the far plane.</param>
        /// <param name="result">The new projection <see cref="MatrixD"/> for perspective view with FOV as an output parameter.</param>
        public static void CreatePerspectiveFieldOfView(
            double fieldOfView,
            double aspectRatio,
            double nearPlaneDistance,
            double farPlaneDistance,
            out MatrixD result
        )
        {
            if ((fieldOfView <= 0f) || (fieldOfView >= 3.141593f))
            {
                throw new ArgumentException("fieldOfView <= 0 or >= PI");
            }
            if (nearPlaneDistance <= 0f)
            {
                throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
                throw new ArgumentException("farPlaneDistance <= 0");
            }
            if (nearPlaneDistance >= farPlaneDistance)
            {
                throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }
            double num = 1f / ((double)Math.Tan((double)(fieldOfView * 0.5f)));
            double num9 = num / aspectRatio;
            result.M11 = num9;
            result.M12 = result.M13 = result.M14 = 0;
            result.M22 = num;
            result.M21 = result.M23 = result.M24 = 0;
            result.M31 = result.M32 = 0f;
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1;
            result.M41 = result.M42 = result.M44 = 0;
            result.M43 = (
                (nearPlaneDistance * farPlaneDistance) /
                (nearPlaneDistance - farPlaneDistance)
            );
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for customized perspective view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <returns>The new <see cref="MatrixD"/> for customized perspective view.</returns>
        public static MatrixD CreatePerspectiveOffCenter(
            double left,
            double right,
            double bottom,
            double top,
            double nearPlaneDistance,
            double farPlaneDistance
        )
        {
            MatrixD result;
            CreatePerspectiveOffCenter(
                left,
                right,
                bottom,
                top,
                nearPlaneDistance,
                farPlaneDistance,
                out result
            );
            return result;
        }

        /// <summary>
        /// Creates a new projection <see cref="MatrixD"/> for customized perspective view.
        /// </summary>
        /// <param name="left">Lower x-value at the near plane.</param>
        /// <param name="right">Upper x-value at the near plane.</param>
        /// <param name="bottom">Lower y-coordinate at the near plane.</param>
        /// <param name="top">Upper y-value at the near plane.</param>
        /// <param name="nearPlaneDistance">Distance to the near plane.</param>
        /// <param name="farPlaneDistance">Distance to the far plane.</param>
        /// <param name="result">The new <see cref="MatrixD"/> for customized perspective view as an output parameter.</param>
        public static void CreatePerspectiveOffCenter(
            double left,
            double right,
            double bottom,
            double top,
            double nearPlaneDistance,
            double farPlaneDistance,
            out MatrixD result
        )
        {
            if (nearPlaneDistance <= 0f)
            {
                throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
                throw new ArgumentException("farPlaneDistance <= 0");
            }
            if (nearPlaneDistance >= farPlaneDistance)
            {
                throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }
            result.M11 = (2f * nearPlaneDistance) / (right - left);
            result.M12 = result.M13 = result.M14 = 0;
            result.M22 = (2f * nearPlaneDistance) / (top - bottom);
            result.M21 = result.M23 = result.M24 = 0;
            result.M31 = (left + right) / (right - left);
            result.M32 = (top + bottom) / (top - bottom);
            result.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M34 = -1;
            result.M43 = (
                (nearPlaneDistance * farPlaneDistance) /
                (nearPlaneDistance - farPlaneDistance)
            );
            result.M41 = result.M42 = result.M44 = 0;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> around X axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="MatrixD"/> around X axis.</returns>
        public static MatrixD CreateRotationX(double radians)
        {
            MatrixD result;
            CreateRotationX(radians, out result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> around X axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <param name="result">The rotation <see cref="MatrixD"/> around X axis as an output parameter.</param>
        public static void CreateRotationX(double radians, out MatrixD result)
        {
            result = MatrixD.Identity;

            double val1 = (double)Math.Cos(radians);
            double val2 = (double)Math.Sin(radians);

            result.M22 = val1;
            result.M23 = val2;
            result.M32 = -val2;
            result.M33 = val1;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> around Y axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="MatrixD"/> around Y axis.</returns>
        public static MatrixD CreateRotationY(double radians)
        {
            MatrixD result;
            CreateRotationY(radians, out result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> around Y axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <param name="result">The rotation <see cref="MatrixD"/> around Y axis as an output parameter.</param>
        public static void CreateRotationY(double radians, out MatrixD result)
        {
            result = MatrixD.Identity;

            double val1 = (double)Math.Cos(radians);
            double val2 = (double)Math.Sin(radians);

            result.M11 = val1;
            result.M13 = -val2;
            result.M31 = val2;
            result.M33 = val1;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> around Z axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>The rotation <see cref="MatrixD"/> around Z axis.</returns>
        public static MatrixD CreateRotationZ(double radians)
        {
            MatrixD result;
            CreateRotationZ(radians, out result);
            return result;
        }

        /// <summary>
        /// Creates a new rotation <see cref="MatrixD"/> around Z axis.
        /// </summary>
        /// <param name="radians">Angle in radians.</param>
        /// <param name="result">The rotation <see cref="MatrixD"/> around Z axis as an output parameter.</param>
        public static void CreateRotationZ(double radians, out MatrixD result)
        {
            result = MatrixD.Identity;

            double val1 = (double)Math.Cos(radians);
            double val2 = (double)Math.Sin(radians);

            result.M11 = val1;
            result.M12 = val2;
            result.M21 = -val2;
            result.M22 = val1;
        }

        /// <summary>
        /// Creates a new scaling <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="scale">Scale value for all three axises.</param>
        /// <returns>The scaling <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateScale(double scale)
        {
            MatrixD result;
            CreateScale(scale, scale, scale, out result);
            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="scale">Scale value for all three axises.</param>
        /// <param name="result">The scaling <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateScale(double scale, out MatrixD result)
        {
            CreateScale(scale, scale, scale, out result);
        }

        /// <summary>
        /// Creates a new scaling <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="xScale">Scale value for X axis.</param>
        /// <param name="yScale">Scale value for Y axis.</param>
        /// <param name="zScale">Scale value for Z axis.</param>
        /// <returns>The scaling <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateScale(double xScale, double yScale, double zScale)
        {
            MatrixD result;
            CreateScale(xScale, yScale, zScale, out result);
            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="xScale">Scale value for X axis.</param>
        /// <param name="yScale">Scale value for Y axis.</param>
        /// <param name="zScale">Scale value for Z axis.</param>
        /// <param name="result">The scaling <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateScale(
            double xScale,
            double yScale,
            double zScale,
            out MatrixD result
        )
        {
            result.M11 = xScale;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = yScale;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = zScale;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new scaling <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="scales"><see cref="Vector3D"/> representing x,y and z scale values.</param>
        /// <returns>The scaling <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateScale(Vector3D scales)
        {
            MatrixD result;
            CreateScale(ref scales, out result);
            return result;
        }

        /// <summary>
        /// Creates a new scaling <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="scales"><see cref="Vector3D"/> representing x,y and z scale values.</param>
        /// <param name="result">The scaling <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateScale(ref Vector3D scales, out MatrixD result)
        {
            result.M11 = scales.X;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = scales.Y;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = scales.Z;
            result.M34 = 0;
            result.M41 = 0;
            result.M42 = 0;
            result.M43 = 0;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that flattens geometry into a specified <see cref="PlaneD"/> as if casting a shadow from a specified light source.
        /// </summary>
        /// <param name="lightDirection">A vector specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <returns>A <see cref="MatrixD"/> that can be used to flatten geometry onto the specified plane from the specified direction. </returns>
        public static MatrixD CreateShadow(Vector3D lightDirection, PlaneD plane)
        {
            MatrixD result;
            CreateShadow(ref lightDirection, ref plane, out result);
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that flattens geometry into a specified <see cref="PlaneD"/> as if casting a shadow from a specified light source.
        /// </summary>
        /// <param name="lightDirection">A vector specifying the direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <param name="result">A <see cref="MatrixD"/> that can be used to flatten geometry onto the specified plane from the specified direction as an output parameter.</param>
        public static void CreateShadow(
            ref Vector3D lightDirection,
            ref PlaneD plane,
            out MatrixD result)
        {
            double dot = (
                (plane.Normal.X * lightDirection.X) +
                (plane.Normal.Y * lightDirection.Y) +
                (plane.Normal.Z * lightDirection.Z)
            );
            double x = -plane.Normal.X;
            double y = -plane.Normal.Y;
            double z = -plane.Normal.Z;
            double d = -plane.D;

            result.M11 = (x * lightDirection.X) + dot;
            result.M12 = x * lightDirection.Y;
            result.M13 = x * lightDirection.Z;
            result.M14 = 0;
            result.M21 = y * lightDirection.X;
            result.M22 = (y * lightDirection.Y) + dot;
            result.M23 = y * lightDirection.Z;
            result.M24 = 0;
            result.M31 = z * lightDirection.X;
            result.M32 = z * lightDirection.Y;
            result.M33 = (z * lightDirection.Z) + dot;
            result.M34 = 0;
            result.M41 = d * lightDirection.X;
            result.M42 = d * lightDirection.Y;
            result.M43 = d * lightDirection.Z;
            result.M44 = dot;
        }

        /// <summary>
        /// Creates a new translation <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="xPosition">X coordinate of translation.</param>
        /// <param name="yPosition">Y coordinate of translation.</param>
        /// <param name="zPosition">Z coordinate of translation.</param>
        /// <returns>The translation <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateTranslation(
            double xPosition,
            double yPosition,
            double zPosition
        )
        {
            MatrixD result;
            CreateTranslation(xPosition, yPosition, zPosition, out result);
            return result;
        }

        /// <summary>
        /// Creates a new translation <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">X,Y and Z coordinates of translation.</param>
        /// <param name="result">The translation <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateTranslation(ref Vector3D position, out MatrixD result)
        {
            result.M11 = 1;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = 1;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = 1;
            result.M34 = 0;
            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new translation <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">X,Y and Z coordinates of translation.</param>
        /// <returns>The translation <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateTranslation(Vector3D position)
        {
            MatrixD result;
            CreateTranslation(ref position, out result);
            return result;
        }

        /// <summary>
        /// Creates a new translation <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="xPosition">X coordinate of translation.</param>
        /// <param name="yPosition">Y coordinate of translation.</param>
        /// <param name="zPosition">Z coordinate of translation.</param>
        /// <param name="result">The translation <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateTranslation(
            double xPosition,
            double yPosition,
            double zPosition,
            out MatrixD result
        )
        {
            result.M11 = 1;
            result.M12 = 0;
            result.M13 = 0;
            result.M14 = 0;
            result.M21 = 0;
            result.M22 = 1;
            result.M23 = 0;
            result.M24 = 0;
            result.M31 = 0;
            result.M32 = 0;
            result.M33 = 1;
            result.M34 = 0;
            result.M41 = xPosition;
            result.M42 = yPosition;
            result.M43 = zPosition;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new reflection <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">The plane that used for reflection calculation.</param>
        /// <returns>The reflection <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateReflection(PlaneD value)
        {
            MatrixD result;
            CreateReflection(ref value, out result);
            return result;
        }

        /// <summary>
        /// Creates a new reflection <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="value">The plane that used for reflection calculation.</param>
        /// <param name="result">The reflection <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateReflection(ref PlaneD value, out MatrixD result)
        {
            PlaneD plane;
            PlaneD.Normalize(ref value, out plane);
            double x = plane.Normal.X;
            double y = plane.Normal.Y;
            double z = plane.Normal.Z;
            double num3 = -2f * x;
            double num2 = -2f * y;
            double num = -2f * z;
            result.M11 = (num3 * x) + 1f;
            result.M12 = num2 * x;
            result.M13 = num * x;
            result.M14 = 0;
            result.M21 = num3 * y;
            result.M22 = (num2 * y) + 1;
            result.M23 = num * y;
            result.M24 = 0;
            result.M31 = num3 * z;
            result.M32 = num2 * z;
            result.M33 = (num * z) + 1;
            result.M34 = 0;
            result.M41 = num3 * plane.D;
            result.M42 = num2 * plane.D;
            result.M43 = num * plane.D;
            result.M44 = 1;
        }

        /// <summary>
        /// Creates a new world <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">The position vector.</param>
        /// <param name="forward">The forward direction vector.</param>
        /// <param name="up">The upward direction vector. Usually <see cref="Vector3D.Up"/>.</param>
        /// <returns>The world <see cref="MatrixD"/>.</returns>
        public static MatrixD CreateWorld(Vector3D position, Vector3D forward, Vector3D up)
        {
            MatrixD ret;
            CreateWorld(ref position, ref forward, ref up, out ret);
            return ret;
        }

        /// <summary>
        /// Creates a new world <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="position">The position vector.</param>
        /// <param name="forward">The forward direction vector.</param>
        /// <param name="up">The upward direction vector. Usually <see cref="Vector3D.Up"/>.</param>
        /// <param name="result">The world <see cref="MatrixD"/> as an output parameter.</param>
        public static void CreateWorld(
            ref Vector3D position,
            ref Vector3D forward,
            ref Vector3D up,
            out MatrixD result
        )
        {
            Vector3D x, y, z;
            Vector3D.Normalize(ref forward, out z);
            Vector3D.Cross(ref forward, ref up, out x);
            Vector3D.Cross(ref x, ref forward, out y);
            x.Normalize();
            y.Normalize();

            result = new MatrixD();
            result.Right = x;
            result.Up = y;
            result.Forward = z;
            result.Translation = position;
            result.M44 = 1f;
        }

        /// <summary>
        /// Divides the elements of a <see cref="MatrixD"/> by the elements of another matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">Divisor <see cref="MatrixD"/>.</param>
        /// <returns>The result of dividing the matrix.</returns>
        public static MatrixD Divide(MatrixD matrix1, MatrixD matrix2)
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M13 = matrix1.M13 / matrix2.M13;
            matrix1.M14 = matrix1.M14 / matrix2.M14;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            matrix1.M23 = matrix1.M23 / matrix2.M23;
            matrix1.M24 = matrix1.M24 / matrix2.M24;
            matrix1.M31 = matrix1.M31 / matrix2.M31;
            matrix1.M32 = matrix1.M32 / matrix2.M32;
            matrix1.M33 = matrix1.M33 / matrix2.M33;
            matrix1.M34 = matrix1.M34 / matrix2.M34;
            matrix1.M41 = matrix1.M41 / matrix2.M41;
            matrix1.M42 = matrix1.M42 / matrix2.M42;
            matrix1.M43 = matrix1.M43 / matrix2.M43;
            matrix1.M44 = matrix1.M44 / matrix2.M44;
            return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="MatrixD"/> by the elements of another matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">Divisor <see cref="MatrixD"/>.</param>
        /// <param name="result">The result of dividing the matrix as an output parameter.</param>
        public static void Divide(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
        {
            result.M11 = matrix1.M11 / matrix2.M11;
            result.M12 = matrix1.M12 / matrix2.M12;
            result.M13 = matrix1.M13 / matrix2.M13;
            result.M14 = matrix1.M14 / matrix2.M14;
            result.M21 = matrix1.M21 / matrix2.M21;
            result.M22 = matrix1.M22 / matrix2.M22;
            result.M23 = matrix1.M23 / matrix2.M23;
            result.M24 = matrix1.M24 / matrix2.M24;
            result.M31 = matrix1.M31 / matrix2.M31;
            result.M32 = matrix1.M32 / matrix2.M32;
            result.M33 = matrix1.M33 / matrix2.M33;
            result.M34 = matrix1.M34 / matrix2.M34;
            result.M41 = matrix1.M41 / matrix2.M41;
            result.M42 = matrix1.M42 / matrix2.M42;
            result.M43 = matrix1.M43 / matrix2.M43;
            result.M44 = matrix1.M44 / matrix2.M44;
        }

        /// <summary>
        /// Divides the elements of a <see cref="MatrixD"/> by a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a matrix by a scalar.</returns>
        public static MatrixD Divide(MatrixD matrix1, double divider)
        {
            double num = 1f / divider;
            matrix1.M11 = matrix1.M11 * num;
            matrix1.M12 = matrix1.M12 * num;
            matrix1.M13 = matrix1.M13 * num;
            matrix1.M14 = matrix1.M14 * num;
            matrix1.M21 = matrix1.M21 * num;
            matrix1.M22 = matrix1.M22 * num;
            matrix1.M23 = matrix1.M23 * num;
            matrix1.M24 = matrix1.M24 * num;
            matrix1.M31 = matrix1.M31 * num;
            matrix1.M32 = matrix1.M32 * num;
            matrix1.M33 = matrix1.M33 * num;
            matrix1.M34 = matrix1.M34 * num;
            matrix1.M41 = matrix1.M41 * num;
            matrix1.M42 = matrix1.M42 * num;
            matrix1.M43 = matrix1.M43 * num;
            matrix1.M44 = matrix1.M44 * num;
            return matrix1;
        }

        /// <summary>
        /// Divides the elements of a <see cref="MatrixD"/> by a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a matrix by a scalar as an output parameter.</param>
        public static void Divide(ref MatrixD matrix1, double divider, out MatrixD result)
        {
            double num = 1f / divider;
            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M13 = matrix1.M13 * num;
            result.M14 = matrix1.M14 * num;
            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
            result.M23 = matrix1.M23 * num;
            result.M24 = matrix1.M24 * num;
            result.M31 = matrix1.M31 * num;
            result.M32 = matrix1.M32 * num;
            result.M33 = matrix1.M33 * num;
            result.M34 = matrix1.M34 * num;
            result.M41 = matrix1.M41 * num;
            result.M42 = matrix1.M42 * num;
            result.M43 = matrix1.M43 * num;
            result.M44 = matrix1.M44 * num;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> which contains inversion of the specified matrix.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/>.</param>
        /// <returns>The inverted matrix.</returns>
        public static MatrixD Invert(MatrixD matrix)
        {
            Invert(ref matrix, out matrix);
            return matrix;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> which contains inversion of the specified matrix.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/>.</param>
        /// <param name="result">The inverted matrix as output parameter.</param>
        public static void Invert(ref MatrixD matrix, out MatrixD result)
        {
            /*
			 * Use Laplace expansion theorem to calculate the inverse of a 4x4 matrix.
			 *
			 * 1. Calculate the 2x2 determinants needed the 4x4 determinant based on
			 *    the 2x2 determinants.
			 * 3. Create the adjugate matrix, which satisfies: A * adj(A) = det(A) * I.
			 * 4. Divide adjugate matrix with the determinant to find the inverse.
			 */

            double num1 = matrix.M11;
            double num2 = matrix.M12;
            double num3 = matrix.M13;
            double num4 = matrix.M14;
            double num5 = matrix.M21;
            double num6 = matrix.M22;
            double num7 = matrix.M23;
            double num8 = matrix.M24;
            double num9 = matrix.M31;
            double num10 = matrix.M32;
            double num11 = matrix.M33;
            double num12 = matrix.M34;
            double num13 = matrix.M41;
            double num14 = matrix.M42;
            double num15 = matrix.M43;
            double num16 = matrix.M44;
            double num17 = (double)(
                (double)num11 * (double)num16 -
                (double)num12 * (double)num15
            );
            double num18 = (double)(
                (double)num10 * (double)num16 -
                (double)num12 * (double)num14
            );
            double num19 = (double)(
                (double)num10 * (double)num15 -
                (double)num11 * (double)num14
            );
            double num20 = (double)(
                (double)num9 * (double)num16 -
                (double)num12 * (double)num13
            );
            double num21 = (double)(
                (double)num9 * (double)num15 -
                (double)num11 * (double)num13
            );
            double num22 = (double)(
                (double)num9 * (double)num14 -
                (double)num10 * (double)num13
            );
            double num23 = (double)(
                (double)num6 * (double)num17 -
                (double)num7 * (double)num18 +
                (double)num8 * (double)num19
            );
            double num24 = (double)-(
                (double)num5 * (double)num17 -
                (double)num7 * (double)num20 +
                (double)num8 * (double)num21
            );
            double num25 = (double)(
                (double)num5 * (double)num18 -
                (double)num6 * (double)num20 +
                (double)num8 * (double)num22
            );
            double num26 = (double)-(
                (double)num5 * (double)num19 -
                (double)num6 * (double)num21 +
                (double)num7 * (double)num22
            );
            double num27 = (double)(
                1.0 / (
                    (double)num1 * (double)num23 +
                    (double)num2 * (double)num24 +
                    (double)num3 * (double)num25 +
                    (double)num4 * (double)num26
                )
            );

            result.M11 = num23 * num27;
            result.M21 = num24 * num27;
            result.M31 = num25 * num27;
            result.M41 = num26 * num27;
            result.M12 = (double)(
                -(
                    (double)num2 * (double)num17 -
                    (double)num3 * (double)num18 +
                    (double)num4 * (double)num19
                ) * num27
            );
            result.M22 = (double)(
                (
                    (double)num1 * (double)num17 -
                    (double)num3 * (double)num20 +
                    (double)num4 * (double)num21
                ) * num27
            );
            result.M32 = (double)(
                -(
                    (double)num1 * (double)num18 -
                    (double)num2 * (double)num20 +
                    (double)num4 * (double)num22
                ) * num27
            );
            result.M42 = (double)(
                (
                    (double)num1 * (double)num19 -
                    (double)num2 * (double)num21 +
                    (double)num3 * (double)num22
                ) * num27
            );
            double num28 = (double)(
                (double)num7 * (double)num16 -
                (double)num8 * (double)num15
            );
            double num29 = (double)(
                (double)num6 * (double)num16 -
                (double)num8 * (double)num14
            );
            double num30 = (double)(
                (double)num6 * (double)num15 -
                (double)num7 * (double)num14
            );
            double num31 = (double)(
                (double)num5 * (double)num16 -
                (double)num8 * (double)num13
            );
            double num32 = (double)(
                (double)num5 * (double)num15 -
                (double)num7 * (double)num13
            );
            double num33 = (double)(
                (double)num5 * (double)num14 -
                (double)num6 * (double)num13
            );
            result.M13 = (double)(
                (
                    (double)num2 * (double)num28 -
                    (double)num3 * (double)num29 +
                    (double)num4 * (double)num30
                ) * num27
            );
            result.M23 = (double)(
                -(
                    (double)num1 * (double)num28 -
                    (double)num3 * (double)num31 +
                    (double)num4 * (double)num32
                ) * num27
            );
            result.M33 = (double)(
                (
                    (double)num1 * (double)num29 -
                    (double)num2 * (double)num31 +
                    (double)num4 * (double)num33
                ) * num27
            );
            result.M43 = (double)(
                -(
                    (double)num1 * (double)num30 -
                    (double)num2 * (double)num32 +
                    (double)num3 * (double)num33
                ) * num27
            );
            double num34 = (double)(
                (double)num7 * (double)num12 -
                (double)num8 * (double)num11
            );
            double num35 = (double)(
                (double)num6 * (double)num12 -
                (double)num8 * (double)num10
            );
            double num36 = (double)(
                (double)num6 * (double)num11 -
                (double)num7 * (double)num10
            );
            double num37 = (double)(
                (double)num5 * (double)num12 -
                (double)num8 * (double)num9);
            double num38 = (double)(
                (double)num5 * (double)num11 -
                (double)num7 * (double)num9
            );
            double num39 = (double)(
                (double)num5 * (double)num10 -
                (double)num6 * (double)num9
            );
            result.M14 = (double)(
                -(
                    (double)num2 * (double)num34 -
                    (double)num3 * (double)num35 +
                    (double)num4 * (double)num36
                ) * num27
            );
            result.M24 = (double)(
                (
                    (double)num1 * (double)num34 -
                    (double)num3 * (double)num37 +
                    (double)num4 * (double)num38
                ) * num27
            );
            result.M34 = (double)(
                -(
                    (double)num1 * (double)num35 -
                    (double)num2 * (double)num37 +
                    (double)num4 * (double)num39
                ) * num27
            );
            result.M44 = (double)(
                (
                    (double)num1 * (double)num36 -
                    (double)num2 * (double)num38 +
                    (double)num3 * (double)num39
                ) * num27
            );
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains linear interpolation of the values in specified matrixes.
        /// </summary>
        /// <param name="matrix1">The first <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>>The result of linear interpolation of the specified matrixes.</returns>
        public static MatrixD Lerp(MatrixD matrix1, MatrixD matrix2, double amount)
        {
            matrix1.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
            matrix1.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
            matrix1.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
            matrix1.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
            matrix1.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
            matrix1.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
            matrix1.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
            matrix1.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
            matrix1.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
            matrix1.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
            matrix1.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
            matrix1.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
            matrix1.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
            matrix1.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
            matrix1.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
            matrix1.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains linear interpolation of the values in specified matrixes.
        /// </summary>
        /// <param name="matrix1">The first <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">The second <see cref="Vector2"/>.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified matrixes as an output parameter.</param>
        public static void Lerp(
            ref MatrixD matrix1,
            ref MatrixD matrix2,
            double amount,
            out MatrixD result
        )
        {
            result.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
            result.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
            result.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
            result.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
            result.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
            result.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
            result.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
            result.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
            result.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
            result.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
            result.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
            result.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
            result.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
            result.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
            result.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
            result.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains a multiplication of two matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/>.</param>
        /// <returns>Result of the matrix multiplication.</returns>
        public static MatrixD Multiply(
            MatrixD matrix1,
            MatrixD matrix2
        )
        {
            double m11 = (
                (matrix1.M11 * matrix2.M11) +
                (matrix1.M12 * matrix2.M21) +
                (matrix1.M13 * matrix2.M31) +
                (matrix1.M14 * matrix2.M41)
            );
            double m12 = (
                (matrix1.M11 * matrix2.M12) +
                (matrix1.M12 * matrix2.M22) +
                (matrix1.M13 * matrix2.M32) +
                (matrix1.M14 * matrix2.M42)
            );
            double m13 = (
                (matrix1.M11 * matrix2.M13) +
                (matrix1.M12 * matrix2.M23) +
                (matrix1.M13 * matrix2.M33) +
                (matrix1.M14 * matrix2.M43)
            );
            double m14 = (
                (matrix1.M11 * matrix2.M14) +
                (matrix1.M12 * matrix2.M24) +
                (matrix1.M13 * matrix2.M34) +
                (matrix1.M14 * matrix2.M44)
            );
            double m21 = (
                (matrix1.M21 * matrix2.M11) +
                (matrix1.M22 * matrix2.M21) +
                (matrix1.M23 * matrix2.M31) +
                (matrix1.M24 * matrix2.M41)
            );
            double m22 = (
                (matrix1.M21 * matrix2.M12) +
                (matrix1.M22 * matrix2.M22) +
                (matrix1.M23 * matrix2.M32) +
                (matrix1.M24 * matrix2.M42)
            );
            double m23 = (
                (matrix1.M21 * matrix2.M13) +
                (matrix1.M22 * matrix2.M23) +
                (matrix1.M23 * matrix2.M33) +
                (matrix1.M24 * matrix2.M43)
            );
            double m24 = (
                (matrix1.M21 * matrix2.M14) +
                (matrix1.M22 * matrix2.M24) +
                (matrix1.M23 * matrix2.M34) +
                (matrix1.M24 * matrix2.M44)
            );
            double m31 = (
                (matrix1.M31 * matrix2.M11) +
                (matrix1.M32 * matrix2.M21) +
                (matrix1.M33 * matrix2.M31) +
                (matrix1.M34 * matrix2.M41)
            );
            double m32 = (
                (matrix1.M31 * matrix2.M12) +
                (matrix1.M32 * matrix2.M22) +
                (matrix1.M33 * matrix2.M32) +
                (matrix1.M34 * matrix2.M42)
            );
            double m33 = (
                (matrix1.M31 * matrix2.M13) +
                (matrix1.M32 * matrix2.M23) +
                (matrix1.M33 * matrix2.M33) +
                (matrix1.M34 * matrix2.M43)
            );
            double m34 = (
                (matrix1.M31 * matrix2.M14) +
                (matrix1.M32 * matrix2.M24) +
                (matrix1.M33 * matrix2.M34) +
                (matrix1.M34 * matrix2.M44)
            );
            double m41 = (
                (matrix1.M41 * matrix2.M11) +
                (matrix1.M42 * matrix2.M21) +
                (matrix1.M43 * matrix2.M31) +
                (matrix1.M44 * matrix2.M41)
            );
            double m42 = (
                (matrix1.M41 * matrix2.M12) +
                (matrix1.M42 * matrix2.M22) +
                (matrix1.M43 * matrix2.M32) +
                (matrix1.M44 * matrix2.M42)
            );
            double m43 = (
                (matrix1.M41 * matrix2.M13) +
                (matrix1.M42 * matrix2.M23) +
                (matrix1.M43 * matrix2.M33) +
                (matrix1.M44 * matrix2.M43)
            );
            double m44 = (
                (matrix1.M41 * matrix2.M14) +
                (matrix1.M42 * matrix2.M24) +
                (matrix1.M43 * matrix2.M34) +
                (matrix1.M44 * matrix2.M44)
            );
            matrix1.M11 = m11;
            matrix1.M12 = m12;
            matrix1.M13 = m13;
            matrix1.M14 = m14;
            matrix1.M21 = m21;
            matrix1.M22 = m22;
            matrix1.M23 = m23;
            matrix1.M24 = m24;
            matrix1.M31 = m31;
            matrix1.M32 = m32;
            matrix1.M33 = m33;
            matrix1.M34 = m34;
            matrix1.M41 = m41;
            matrix1.M42 = m42;
            matrix1.M43 = m43;
            matrix1.M44 = m44;
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains a multiplication of two matrix.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/>.</param>
        /// <param name="result">Result of the matrix multiplication as an output parameter.</param>
        public static void Multiply(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
        {
            double m11 = (
                (matrix1.M11 * matrix2.M11) +
                (matrix1.M12 * matrix2.M21) +
                (matrix1.M13 * matrix2.M31) +
                (matrix1.M14 * matrix2.M41)
            );
            double m12 = (
                (matrix1.M11 * matrix2.M12) +
                (matrix1.M12 * matrix2.M22) +
                (matrix1.M13 * matrix2.M32) +
                (matrix1.M14 * matrix2.M42)
            );
            double m13 = (
                (matrix1.M11 * matrix2.M13) +
                (matrix1.M12 * matrix2.M23) +
                (matrix1.M13 * matrix2.M33) +
                (matrix1.M14 * matrix2.M43)
            );
            double m14 = (
                (matrix1.M11 * matrix2.M14) +
                (matrix1.M12 * matrix2.M24) +
                (matrix1.M13 * matrix2.M34) +
                (matrix1.M14 * matrix2.M44)
            );
            double m21 = (
                (matrix1.M21 * matrix2.M11) +
                (matrix1.M22 * matrix2.M21) +
                (matrix1.M23 * matrix2.M31) +
                (matrix1.M24 * matrix2.M41)
            );
            double m22 = (
                (matrix1.M21 * matrix2.M12) +
                (matrix1.M22 * matrix2.M22) +
                (matrix1.M23 * matrix2.M32) +
                (matrix1.M24 * matrix2.M42)
            );
            double m23 = (
                (matrix1.M21 * matrix2.M13) +
                (matrix1.M22 * matrix2.M23) +
                (matrix1.M23 * matrix2.M33) +
                (matrix1.M24 * matrix2.M43)
                );
            double m24 = (
                (matrix1.M21 * matrix2.M14) +
                (matrix1.M22 * matrix2.M24) +
                (matrix1.M23 * matrix2.M34) +
                (matrix1.M24 * matrix2.M44)
            );
            double m31 = (
                (matrix1.M31 * matrix2.M11) +
                (matrix1.M32 * matrix2.M21) +
                (matrix1.M33 * matrix2.M31) +
                (matrix1.M34 * matrix2.M41)
            );
            double m32 = (
                (matrix1.M31 * matrix2.M12) +
                (matrix1.M32 * matrix2.M22) +
                (matrix1.M33 * matrix2.M32) +
                (matrix1.M34 * matrix2.M42)
            );
            double m33 = (
                (matrix1.M31 * matrix2.M13) +
                (matrix1.M32 * matrix2.M23) +
                (matrix1.M33 * matrix2.M33) +
                (matrix1.M34 * matrix2.M43)
            );
            double m34 = (
                (matrix1.M31 * matrix2.M14) +
                (matrix1.M32 * matrix2.M24) +
                (matrix1.M33 * matrix2.M34) +
                (matrix1.M34 * matrix2.M44)
            );
            double m41 = (
                (matrix1.M41 * matrix2.M11) +
                (matrix1.M42 * matrix2.M21) +
                (matrix1.M43 * matrix2.M31) +
                (matrix1.M44 * matrix2.M41)
            );
            double m42 = (
                (matrix1.M41 * matrix2.M12) +
                (matrix1.M42 * matrix2.M22) +
                (matrix1.M43 * matrix2.M32) +
                (matrix1.M44 * matrix2.M42)
            );
            double m43 = (
                (matrix1.M41 * matrix2.M13) +
                (matrix1.M42 * matrix2.M23) +
                (matrix1.M43 * matrix2.M33) +
                (matrix1.M44 * matrix2.M43)
            );
            double m44 = (
                (matrix1.M41 * matrix2.M14) +
                (matrix1.M42 * matrix2.M24) +
                (matrix1.M43 * matrix2.M34) +
                (matrix1.M44 * matrix2.M44)
            );
            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;
            result.M14 = m14;
            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;
            result.M24 = m24;
            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
            result.M34 = m34;
            result.M41 = m41;
            result.M42 = m42;
            result.M43 = m43;
            result.M44 = m44;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains a multiplication of <see cref="MatrixD"/> and a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>Result of the matrix multiplication with a scalar.</returns>
        public static MatrixD Multiply(MatrixD matrix1, double scaleFactor)
        {
            matrix1.M11 *= scaleFactor;
            matrix1.M12 *= scaleFactor;
            matrix1.M13 *= scaleFactor;
            matrix1.M14 *= scaleFactor;
            matrix1.M21 *= scaleFactor;
            matrix1.M22 *= scaleFactor;
            matrix1.M23 *= scaleFactor;
            matrix1.M24 *= scaleFactor;
            matrix1.M31 *= scaleFactor;
            matrix1.M32 *= scaleFactor;
            matrix1.M33 *= scaleFactor;
            matrix1.M34 *= scaleFactor;
            matrix1.M41 *= scaleFactor;
            matrix1.M42 *= scaleFactor;
            matrix1.M43 *= scaleFactor;
            matrix1.M44 *= scaleFactor;
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains a multiplication of <see cref="MatrixD"/> and a scalar.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">Result of the matrix multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref MatrixD matrix1, double scaleFactor, out MatrixD result)
        {
            result.M11 = matrix1.M11 * scaleFactor;
            result.M12 = matrix1.M12 * scaleFactor;
            result.M13 = matrix1.M13 * scaleFactor;
            result.M14 = matrix1.M14 * scaleFactor;
            result.M21 = matrix1.M21 * scaleFactor;
            result.M22 = matrix1.M22 * scaleFactor;
            result.M23 = matrix1.M23 * scaleFactor;
            result.M24 = matrix1.M24 * scaleFactor;
            result.M31 = matrix1.M31 * scaleFactor;
            result.M32 = matrix1.M32 * scaleFactor;
            result.M33 = matrix1.M33 * scaleFactor;
            result.M34 = matrix1.M34 * scaleFactor;
            result.M41 = matrix1.M41 * scaleFactor;
            result.M42 = matrix1.M42 * scaleFactor;
            result.M43 = matrix1.M43 * scaleFactor;
            result.M44 = matrix1.M44 * scaleFactor;

        }

        /// <summary>
        /// Returns a matrix with the all values negated.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/>.</param>
        /// <returns>Result of the matrix negation.</returns>
        public static MatrixD Negate(MatrixD matrix)
        {
            matrix.M11 = -matrix.M11;
            matrix.M12 = -matrix.M12;
            matrix.M13 = -matrix.M13;
            matrix.M14 = -matrix.M14;
            matrix.M21 = -matrix.M21;
            matrix.M22 = -matrix.M22;
            matrix.M23 = -matrix.M23;
            matrix.M24 = -matrix.M24;
            matrix.M31 = -matrix.M31;
            matrix.M32 = -matrix.M32;
            matrix.M33 = -matrix.M33;
            matrix.M34 = -matrix.M34;
            matrix.M41 = -matrix.M41;
            matrix.M42 = -matrix.M42;
            matrix.M43 = -matrix.M43;
            matrix.M44 = -matrix.M44;
            return matrix;
        }

        /// <summary>
        /// Returns a matrix with the all values negated.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/>.</param>
        /// <param name="result">Result of the matrix negation as an output parameter.</param>
        public static void Negate(ref MatrixD matrix, out MatrixD result)
        {
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;
            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains subtraction of one matrix from another.
        /// </summary>
        /// <param name="matrix1">The first <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">The second <see cref="MatrixD"/>.</param>
        /// <returns>The result of the matrix subtraction.</returns>
        public static MatrixD Subtract(MatrixD matrix1, MatrixD matrix2)
        {
            matrix1.M11 -= matrix2.M11;
            matrix1.M12 -= matrix2.M12;
            matrix1.M13 -= matrix2.M13;
            matrix1.M14 -= matrix2.M14;
            matrix1.M21 -= matrix2.M21;
            matrix1.M22 -= matrix2.M22;
            matrix1.M23 -= matrix2.M23;
            matrix1.M24 -= matrix2.M24;
            matrix1.M31 -= matrix2.M31;
            matrix1.M32 -= matrix2.M32;
            matrix1.M33 -= matrix2.M33;
            matrix1.M34 -= matrix2.M34;
            matrix1.M41 -= matrix2.M41;
            matrix1.M42 -= matrix2.M42;
            matrix1.M43 -= matrix2.M43;
            matrix1.M44 -= matrix2.M44;
            return matrix1;
        }

        /// <summary>
        /// Creates a new <see cref="MatrixD"/> that contains subtraction of one matrix from another.
        /// </summary>
        /// <param name="matrix1">The first <see cref="MatrixD"/>.</param>
        /// <param name="matrix2">The second <see cref="MatrixD"/>.</param>
        /// <param name="result">The result of the matrix subtraction as an output parameter.</param>
        public static void Subtract(ref MatrixD matrix1, ref MatrixD matrix2, out MatrixD result)
        {
            result.M11 = matrix1.M11 - matrix2.M11;
            result.M12 = matrix1.M12 - matrix2.M12;
            result.M13 = matrix1.M13 - matrix2.M13;
            result.M14 = matrix1.M14 - matrix2.M14;
            result.M21 = matrix1.M21 - matrix2.M21;
            result.M22 = matrix1.M22 - matrix2.M22;
            result.M23 = matrix1.M23 - matrix2.M23;
            result.M24 = matrix1.M24 - matrix2.M24;
            result.M31 = matrix1.M31 - matrix2.M31;
            result.M32 = matrix1.M32 - matrix2.M32;
            result.M33 = matrix1.M33 - matrix2.M33;
            result.M34 = matrix1.M34 - matrix2.M34;
            result.M41 = matrix1.M41 - matrix2.M41;
            result.M42 = matrix1.M42 - matrix2.M42;
            result.M43 = matrix1.M43 - matrix2.M43;
            result.M44 = matrix1.M44 - matrix2.M44;
        }

        /// <summary>
        /// Swap the matrix rows and columns.
        /// </summary>
        /// <param name="matrix">The matrix for transposing operation.</param>
        /// <returns>The new <see cref="MatrixD"/> which contains the transposing result.</returns>
        public static MatrixD Transpose(MatrixD matrix)
        {
            MatrixD ret;
            Transpose(ref matrix, out ret);
            return ret;
        }

        /// <summary>
        /// Swap the matrix rows and columns.
        /// </summary>
        /// <param name="matrix">The matrix for transposing operation.</param>
        /// <param name="result">The new <see cref="MatrixD"/> which contains the transposing result as an output parameter.</param>
        public static void Transpose(ref MatrixD matrix, out MatrixD result)
        {
            MatrixD ret;

            ret.M11 = matrix.M11;
            ret.M12 = matrix.M21;
            ret.M13 = matrix.M31;
            ret.M14 = matrix.M41;

            ret.M21 = matrix.M12;
            ret.M22 = matrix.M22;
            ret.M23 = matrix.M32;
            ret.M24 = matrix.M42;

            ret.M31 = matrix.M13;
            ret.M32 = matrix.M23;
            ret.M33 = matrix.M33;
            ret.M34 = matrix.M43;

            ret.M41 = matrix.M14;
            ret.M42 = matrix.M24;
            ret.M43 = matrix.M34;
            ret.M44 = matrix.M44;

            result = ret;
        }

        public static MatrixD Transform(MatrixD value, QuaternionD rotation)
        {
            MatrixD result;
            Transform(ref value, ref rotation, out result);
            return result;
        }

        public static void Transform(
            ref MatrixD value,
            ref QuaternionD rotation,
            out MatrixD result
        )
        {
            MatrixD rotMatrix = CreateFromQuaternion(rotation);
            Multiply(ref value, ref rotMatrix, out result);
        }

        #endregion

        #region Public Static Operator Overloads

        /// <summary>
        /// Adds two matrixes.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/> on the left of the add sign.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/> on the right of the add sign.</param>
        /// <returns>Sum of the matrixes.</returns>
        public static MatrixD operator +(MatrixD matrix1, MatrixD matrix2)
        {
            return MatrixD.Add(matrix1, matrix2);
        }

        /// <summary>
        /// Divides the elements of a <see cref="MatrixD"/> by the elements of another <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/> on the left of the div sign.</param>
        /// <param name="matrix2">Divisor <see cref="MatrixD"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the matrixes.</returns>
        public static MatrixD operator /(MatrixD matrix1, MatrixD matrix2)
        {
            return MatrixD.Divide(matrix1, matrix2);
        }

        /// <summary>
        /// Divides the elements of a <see cref="MatrixD"/> by a scalar.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a matrix by a scalar.</returns>
        public static MatrixD operator /(MatrixD matrix, double divider)
        {
            return MatrixD.Divide(matrix, divider);
        }

        /// <summary>
        /// Compares whether two <see cref="MatrixD"/> instances are equal without any tolerance.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/> on the left of the equal sign.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/> on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(MatrixD matrix1, MatrixD matrix2)
        {
            return matrix1.Equals(matrix2);
        }

        /// <summary>
        /// Compares whether two <see cref="MatrixD"/> instances are not equal without any tolerance.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/> on the left of the not equal sign.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/> on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(MatrixD matrix1, MatrixD matrix2)
        {
            return !matrix1.Equals(matrix2);
        }

        /// <summary>
        /// Multiplies two matrixes.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/> on the left of the mul sign.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/> on the right of the mul sign.</param>
        /// <returns>Result of the matrix multiplication.</returns>
        /// <remarks>
        /// Using matrix multiplication algorithm - see http://en.wikipedia.org/wiki/Matrix_multiplication.
        /// </remarks>
        public static MatrixD operator *(MatrixD matrix1, MatrixD matrix2)
        {
            return Multiply(matrix1, matrix2);
        }

        /// <summary>
        /// Multiplies the elements of matrix by a scalar.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the matrix multiplication with a scalar.</returns>
        public static MatrixD operator *(MatrixD matrix, double scaleFactor)
        {
            return Multiply(matrix, scaleFactor);
        }

        /// <summary>
        /// Subtracts the values of one <see cref="MatrixD"/> from another <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="matrix1">Source <see cref="MatrixD"/> on the left of the sub sign.</param>
        /// <param name="matrix2">Source <see cref="MatrixD"/> on the right of the sub sign.</param>
        /// <returns>Result of the matrix subtraction.</returns>
        public static MatrixD operator -(MatrixD matrix1, MatrixD matrix2)
        {
            return Subtract(matrix1, matrix2);
        }

        /// <summary>
        /// Inverts values in the specified <see cref="MatrixD"/>.
        /// </summary>
        /// <param name="matrix">Source <see cref="MatrixD"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static MatrixD operator -(MatrixD matrix)
        {
            return Negate(matrix);
        }

        #endregion
    }
}
