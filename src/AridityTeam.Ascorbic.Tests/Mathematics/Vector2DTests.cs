using System;

using AridityTeam.Mathematics;

namespace AridityTeam.Ascorbic.Tests.Mathematics
{
    public class Vector2DTests
    {
        [Fact]
        public void Vector2D_Addition_Works()
        {
            var v1 = new Vector2(1, 2);
            var v2 = new Vector2(3, 4);
            var result = v1 + v2;
            Assert.Equal(new Vector2(4, 6), result);
        }

        [Fact]
        public void Vector2D_Subtraction_Works()
        {
            var v1 = new Vector2(5, 7);
            var v2 = new Vector2(2, 3);
            var result = v1 - v2;
            Assert.Equal(new Vector2(3, 4), result);
        }

        [Fact]
        public void Vector2D_ScalarMultiplication_Works()
        {
            var v = new Vector2(2, 3);
            var result = v * 2;
            Assert.Equal(new Vector2(4, 6), result);
        }

        [Fact]
        public void Vector2D_ScalarDivision_Works()
        {
            var v = new Vector2(4, 6);
            var result = v / 2;
            Assert.Equal(new Vector2(2, 3), result);
        }

        [Fact]
        public void Vector2D_Negation_Works()
        {
            var v = new Vector2(2, -3);
            var result = -v;
            Assert.Equal(new Vector2(-2, 3), result);
        }

        [Fact]
        public void Vector2D_Length_Works()
        {
            var v = new Vector2(3, 4);
            Assert.Equal(5, v.Length, 5);
            Assert.Equal(25, v.LengthSquared, 5);
        }

        [Fact]
        public void Vector2D_Normalized_Works()
        {
            var v = new Vector2(3, 4);
            var normalized = v.Normalized;
            Assert.Equal(1, normalized.Length, 5);
        }

        [Fact]
        public void Vector2D_DotProduct_Works()
        {
            var v1 = new Vector2(1, 2);
            var v2 = new Vector2(3, 4);
            Assert.Equal(11, v1.Dot(v2));
        }

        [Fact]
        public void Vector2D_DistanceTo_Works()
        {
            var v1 = new Vector2(1, 2);
            var v2 = new Vector2(4, 6);
            Assert.Equal(5, v1.DistanceTo(v2), 5);
        }

        [Fact]
        public void Vector2D_Constants_Works()
        {
            Assert.Equal(new Vector2(0, 0), Vector2.Zero);
            Assert.Equal(new Vector2(1, 1), Vector2.One);
        }
    }
}
