using System;

using AridityTeam.Mathematics;

namespace AridityTeam.Ascorbic.Tests.Mathematics
{
    public class Vector3DTests
    {
        [Fact]
        public void Vector3D_Addition_Works()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(4, 5, 6);
            Assert.Equal(new Vector3(5, 7, 9), v1 + v2);
        }

        [Fact]
        public void Vector3D_Subtraction_Works()
        {
            var v1 = new Vector3(5, 7, 9);
            var v2 = new Vector3(1, 2, 3);
            Assert.Equal(new Vector3(4, 5, 6), v1 - v2);
        }

        [Fact]
        public void Vector3D_ScalarMultiplication_Works()
        {
            var v = new Vector3(1, 2, 3);
            Assert.Equal(new Vector3(2, 4, 6), v * 2);
        }

        [Fact]
        public void Vector3D_ScalarDivision_Works()
        {
            var v = new Vector3(2, 4, 6);
            Assert.Equal(new Vector3(1, 2, 3), v / 2);
        }

        [Fact]
        public void Vector3D_Negation_Works()
        {
            var v = new Vector3(1, -2, 3);
            Assert.Equal(new Vector3(-1, 2, -3), -v);
        }

        [Fact]
        public void Vector3D_Length_Works()
        {
            var v = new Vector3(2, 3, 6);
            var expected = Math.Sqrt(4 + 9 + 36); // sqrt(49) = 7
            Assert.Equal(expected, v.Length, 5);
            Assert.Equal(49, v.LengthSquared, 5);
        }

        [Fact]
        public void Vector3D_Normalized_Works()
        {
            var v = new Vector3(1, 2, 2);
            var normalized = v.Normalized;
            Assert.Equal(1, normalized.Length, 5);
        }

        [Fact]
        public void Vector3D_DotProduct_Works()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(4, 5, 6);
            Assert.Equal(32, v1.Dot(v2));
        }

        [Fact]
        public void Vector3D_CrossProduct_Works()
        {
            var v1 = new Vector3(1, 0, 0);
            var v2 = new Vector3(0, 1, 0);
            Assert.Equal(new Vector3(0, 0, 1), v1.Cross(v2));
        }

        [Fact]
        public void Vector3D_DistanceTo_Works()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(4, 6, 3);
            Assert.Equal(5, v1.DistanceTo(v2), 5);
        }

        [Fact]
        public void Vector3D_Constants_Works()
        {
            Assert.Equal(new Vector3(0, 0, 0), Vector3.Zero);
            Assert.Equal(new Vector3(1, 1, 1), Vector3.One);
            Assert.Equal(new Vector3(0, 1, 0), Vector3.Up);
            Assert.Equal(new Vector3(0, 0, 1), Vector3.Forward);
            Assert.Equal(new Vector3(1, 0, 0), Vector3.Right);
        }
    }
}
