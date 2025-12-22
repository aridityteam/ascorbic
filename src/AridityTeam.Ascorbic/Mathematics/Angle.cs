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
using System.Runtime.Serialization;

using PolyType;

namespace AridityTeam.Mathematics
{
    /// <summary>
    /// Represents an angle in degrees, with conversion to/from radians.
    /// </summary>
    [GenerateShape]
    [Serializable]
    public partial struct Angle(double degrees)
    {
        /// <summary>The angle in degrees.</summary>
        [DataMember]
        public double Degrees = degrees;

        /// <summary>Gets or sets the angle in radians.</summary>
        [DataMember]
        public double Radians
        {
            readonly get => MathUtil.ToRadians(Degrees);
            set => Degrees = MathUtil.ToDegrees(value);
        }

        /// <summary>Creates an angle from radians.</summary>
        /// <param name="radians">Angle in radians.</param>
        /// <returns>Angle in degrees.</returns>
        public static Angle FromRadians(double radians) => new(MathUtil.ToDegrees(radians));

        /// <summary>Returns a string representation of the angle in degrees.</summary>
        public override readonly string ToString() => $"{Degrees}°";
    }
}
