using System;

using AridityTeam.Mathematics;

namespace AridityTeam.Ascorbic.Tests.Mathematics
{
    public class MathUtilTests
    {
#if NET472_OR_GREATER
    [Theory]
    [InlineData(5, 1, 10, 5)]
    [InlineData(0, 1, 10, 1)]
    [InlineData(15, 1, 10, 10)]
    [InlineData(10, 10, 10, 10)]
    public void Clamp_Int_Works(int value, int min, int max, int expected)
    {
        int result = MathHelper.Clamp(value, min, max);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5.5, 1.0, 10.0, 5.5)]
    [InlineData(0.5, 1.0, 10.0, 1.0)]
    [InlineData(15.5, 1.0, 10.0, 10.0)]
    public void Clamp_Double_Works(double value, double min, double max, double expected)
    {
        double result = MathHelper.Clamp(value, min, max);
        Assert.Equal(expected, result, 5); // precision tolerance
    }

    [Theory]
    [InlineData(5L, 1L, 10L, 5L)]
    [InlineData(0L, 1L, 10L, 1L)]
    [InlineData(15L, 1L, 10L, 10L)]
    public void Clamp_Long_Works(long value, long min, long max, long expected)
    {
        double result = MathHelper.Clamp(value, min, max);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(5f, 1f, 10f, 5f)]
    [InlineData(0f, 1f, 10f, 1f)]
    [InlineData(15f, 1f, 10f, 10f)]
    public void Clamp_Float_Works(float value, float min, float max, float expected)
    {
        double result = MathHelper.Clamp(value, min, max);
        Assert.Equal(expected, result, 5);
    }
#endif

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 2, 4)]
        [InlineData(7, 4, 8)]
        [InlineData(16, 16, 16)]
        [InlineData(20, 16, 32)]
        public void GetNearestPow2_Works(long number, long expectedLess, long expectedHigher)
        {
            bool result = MathUtil.GetNearestPow2(number, out long less, out long higher);
            Assert.True(result);
            Assert.Equal(expectedLess, less);
            Assert.Equal(expectedHigher, higher);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 2)]
        [InlineData(7, 4)]
        [InlineData(20, 16)]
        public void GetNearestLessPowOf2_Works(long number, long expected)
        {
            long result = MathUtil.GetNearestLessPowOf2(number);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 4)]
        [InlineData(7, 8)]
        [InlineData(20, 32)]
        public void GetNearestHigherPowOf2_Works(long number, long expected)
        {
            long result = MathUtil.GetNearestHigherPowOf2(number);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 4)]
        [InlineData(6, 8)]
        [InlineData(20, 16)] // closer to 16 than 32
        [InlineData(24, 32)] // closer to 32 than 16
        public void RoundToNearestPowOf2_Works(long number, long expected)
        {
            long result = MathUtil.RoundToNearestPowOf2(number);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(4, 2)]
        [InlineData(8, 3)]
        [InlineData(16, 4)]
        [InlineData(32, 5)]
        public void GetBinaryLogBase_Works(long number, long expected)
        {
            long result = MathUtil.GetBinaryLogBase(number);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetBinaryLogBase_Throws_OnNonPowerOf2()
        {
            Assert.Throws<ArgumentException>(() => MathUtil.GetBinaryLogBase(3));
            Assert.Throws<ArgumentException>(() => MathUtil.GetBinaryLogBase(6));
            Assert.Throws<ArgumentException>(() => MathUtil.GetBinaryLogBase(20));
        }
    }

}
