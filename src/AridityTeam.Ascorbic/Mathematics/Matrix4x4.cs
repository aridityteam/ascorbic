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
    /// Represents a 4x4 transformation matrix.
    /// </summary>
    [GenerateShape]
    [Serializable]
    public partial struct Matrix4x4
    {
        /// <summary>The matrix elements, indexed by [row, column].</summary>
        [DataMember]
        public double[,] M;

        /// <summary>Initializes a new 4x4 matrix. Identity by default.</summary>
        [ConstructorShape]
        public Matrix4x4(bool identity = true)
        {
            M = new double[4, 4];
            if (identity)
                for (int i = 0; i < 4; i++)
                    M[i, i] = 1;
        }

        /// <summary>Gets the identity matrix.</summary>
        public static Matrix4x4 Identity => new();

        /// <summary>Creates a translation matrix.</summary>
        /// <param name="v">Translation vector.</param>
        public static Matrix4x4 Translation(Vector3 v)
        {
            var m = Identity;
            m.M[0, 3] = v.X;
            m.M[1, 3] = v.Y;
            m.M[2, 3] = v.Z;
            return m;
        }

        /// <summary>Creates a scaling matrix.</summary>
        /// <param name="v">Scale factors for X, Y, Z.</param>
        public static Matrix4x4 Scale(Vector3 v)
        {
            var m = Identity;
            m.M[0, 0] = v.X;
            m.M[1, 1] = v.Y;
            m.M[2, 2] = v.Z;
            return m;
        }

        /// <summary>Multiplies two 4x4 matrices.</summary>
        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            var r = new Matrix4x4(false);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        r.M[i, j] += a.M[i, k] * b.M[k, j];
            return r;
        }
    }
}
