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

namespace AridityTeam.Mathematics
{
    /// <summary>
    /// Represents a 2D vector using two double-precision floating-point numbers.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [Serializable]
    public struct Vector2D : IEquatable<Vector2D>
    {
        /// <summary>
        /// Defines a zero-length <seealso cref="Vector2D"/>.
        /// </summary>
        public static Vector2D Zero => new(0, 0);

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
        /// Initializes a new instance of the <seealso cref="Vector2D"/> struct.
        /// </summary>
        /// <param name="x">The value of the X coordinate.</param>
        public Vector2D(double x)
        {
            X = x;
            Y = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Vector2D"/> struct.
        /// </summary>
        /// <param name="x">The value of the X coordinate.</param>
        /// <param name="y">The value of the Y coordinate.</param>
        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
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
        public static bool operator ==(Vector2D a, Vector2D b) =>
            a.Equals(b);

        /// <summary>
        /// Indicates whether the current object is not equal to another object of the same type.
        /// </summary>
        /// <param name="a">An object to compare with the other object.</param>
        /// <param name="b">An object to compare with the other object.</param>
        /// <returns><see langword="true"/> if the current object is not equal to the other parameter; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Vector2D a, Vector2D b) =>
            !(a==b);

        /// <summary>
        /// Adds two vectors together component-wise.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The component-wise sum of the vectors.</returns>
        public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Subtracts the second vector from the first vector component-wise.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The component-wise difference of the vectors.</returns>
        public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);

        /// <summary>
        /// Multiplies a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>The vector scaled by the scalar.</returns>
        public static Vector2D operator *(Vector2D v, double scalar) => new(v.X * scalar, v.Y * scalar);

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>The vector divided by the scalar.</returns>
        public static Vector2D operator /(Vector2D v, double scalar) => new(v.X / scalar, v.Y / scalar);

        /// <summary>
        /// Negates a vector.
        /// </summary>
        /// <param name="v">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        public static Vector2D operator -(Vector2D v) => new(-v.X, -v.Y);

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        public readonly double Length => Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        public readonly double LengthSquared => X * X + Y * Y;

        /// <summary>
        /// Returns a normalized (unit length) version of the vector.
        /// </summary>
        public Vector2D Normalized => this / Length;

        /// <summary>
        /// Computes the dot product of this vector with another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The dot product.</returns>
        public readonly double Dot(Vector2D other) => X * other.X + Y * other.Y;

        /// <summary>
        /// Computes the distance from this vector to another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The Euclidean distance.</returns>
        public readonly double DistanceTo(Vector2D other) => (this - other).Length;

        /// <summary>
        /// Represents a vector with all components set to 1.
        /// </summary>
        public static Vector2D One => new(1, 1);

        /// <inheritdoc/>
        public readonly bool Equals(Vector2D other) =>
            this.X == other.X && this.Y == other.Y;

        /// <inheritdoc cref="Equals(Vector2D)"/>
        public override readonly bool Equals(object? obj) =>
            obj is not null &&
            Equals((Vector2D)obj);

        /// <summary>
        /// Returns a string that contains this current Vector instance's X, Y coordinates.
        /// </summary>
        /// <returns>A string that contains this current Vector instance's X, Y coordinates.</returns>
        public override readonly string ToString() =>
            $"X={X} Y={Y}";

        /// <inheritdoc/>
        public override readonly int GetHashCode() =>
            HashCode.Combine(X, Y);
    }
}
