﻿/*
 * Copyright (c) 2025 The Aridity Team
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using PolyType;

namespace AridityTeam.Mathematics
{
    /// <summary>
    /// Represents a three dimensional vector using three double-precision floating-point numbers.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [Serializable]
    [GenerateShape]
    public partial struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// Defines a zero-length <seealso cref="Vector3"/>.
        /// </summary>
        public static Vector3 Zero => new(0, 0);

        /// <summary>
        /// Represents the X coordinate of this vector instance.
        /// </summary>
        [DataMember]
        public double X { get; set; }

        /// <summary>
        /// Represents the Y coordinate of this vector instance.
        /// </summary>
        [DataMember]
        public double Y { get; set; }

        /// <summary>
        /// Represents the Z coordinate of this vector instance.
        /// </summary>
        [DataMember]
        public double Z { get; set; }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">The value of the X coordinate.</param>
        public Vector3(double x)
        {
            X = x;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The value of the X coordinate.</param>
        /// <param name="y">The value of the Y coordinate.</param>
        public Vector3(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Vector3"/> struct.
        /// </summary>
        /// <param name="x">The value of the X coordinate.</param>
        /// <param name="y">The value of the Y coordinate.</param>
        /// <param name="z">The value of the Z coordinate.</param>
        [ConstructorShape]
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets a string for the debugger to display for this struct.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string DebuggerDisplay => this.ToString();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="a">An object to compare with the other object.</param>
        /// <param name="b">An object to compare with the other object.</param>
        /// <returns><see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Vector3 a, Vector3 b) =>
            a.Equals(b);

        /// <summary>
        /// Indicates whether the current object is not equal to another object of the same type.
        /// </summary>
        /// <param name="a">An object to compare with the other object.</param>
        /// <param name="b">An object to compare with the other object.</param>
        /// <returns><see langword="true"/> if the current object is not equal to the other parameter; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Vector3 a, Vector3 b) =>
            !(a==b);

        /// <summary>
        /// Adds two vectors together component-wise.
        /// </summary>
        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Subtracts the second vector from the first vector component-wise.
        /// </summary>
        public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        public static Vector3 operator *(Vector3 v, double scalar) => new(v.X * scalar, v.Y * scalar, v.Z * scalar);

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        public static Vector3 operator /(Vector3 v, double scalar) => new(v.X / scalar, v.Y / scalar, v.Z / scalar);

        /// <summary>
        /// Negates a vector.
        /// </summary>
        public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z);

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        public readonly double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        public readonly double LengthSquared => X * X + Y * Y + Z * Z;

        /// <summary>
        /// Returns a normalized (unit length) version of the vector.
        /// </summary>
        public Vector3 Normalized => this / Length;

        /// <summary>
        /// Computes the dot product of this vector with another vector.
        /// </summary>
        public readonly double Dot(Vector3 other) => X * other.X + Y * other.Y + Z * other.Z;

        /// <summary>
        /// Computes the cross product of this vector with another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The cross product vector.</returns>
        public readonly Vector3 Cross(Vector3 other) =>
            new(Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X);

        /// <summary>
        /// Computes the distance from this vector to another vector.
        /// </summary>
        public readonly double DistanceTo(Vector3 other) => (this - other).Length;

        /// <summary>
        /// Represents a vector with all components set to 1.
        /// </summary>
        public static Vector3 One => new(1, 1, 1);

        /// <summary>
        /// Represents a vector pointing upwards along the Y axis.
        /// </summary>
        public static Vector3 Up => new(0, 1, 0);

        /// <summary>
        /// Represents a vector pointing forward along the Z axis.
        /// </summary>
        public static Vector3 Forward => new(0, 0, 1);

        /// <summary>
        /// Represents a vector pointing right along the X axis.
        /// </summary>
        public static Vector3 Right => new(1, 0, 0);

        /// <inheritdoc/>
        public readonly bool Equals(Vector3 other) =>
            this.X == other.X && this.Y == other.Y;

        /// <inheritdoc cref="Equals(Vector3)"/>
        public override readonly bool Equals(object? obj) =>
            obj is not null &&
            Equals((Vector3)obj);

        /// <summary>
        /// Returns a string that contains this current Vector instance's X, Y, Z coordinates.
        /// </summary>
        /// <returns>A string that contains this current Vector instance's X, Y, Z coordinates.</returns>
        public override readonly string ToString() =>
            $"X={X} Y={Y} Z={Z}";

        /// <inheritdoc/>
        public override readonly int GetHashCode() =>
            HashCode.Combine(X, Y);
    }
}
