/*
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
    /// Represents a plane in 3D space.
    /// </summary>
    [GenerateShape]
    [Serializable]
    public partial struct Plane
    {
        /// <summary>Normal vector of the plane.</summary>
        [DataMember]
        public Vector3 Normal;

        /// <summary>Distance from the origin along the normal.</summary>
        [DataMember]
        public double Distance;

        /// <summary>Initializes a plane from a normal and a distance.</summary>
        public Plane(Vector3 normal, double distance) => (Normal, Distance) = (normal.Normalized, distance);

        /// <summary>Initializes a plane from a point on the plane and its normal vector.</summary>
        public Plane(Vector3 point, Vector3 normal) => (Normal, Distance) = (normal.Normalized, -normal.Dot(point));

        /// <summary>Calculates the signed distance from a point to the plane.</summary>
        public readonly double DistanceToPoint(Vector3 point) => Normal.Dot(point) + Distance;

        /// <summary>Projects a point onto the plane.</summary>
        public readonly Vector3 ProjectPoint(Vector3 point) => point - Normal * DistanceToPoint(point);
    }
}
