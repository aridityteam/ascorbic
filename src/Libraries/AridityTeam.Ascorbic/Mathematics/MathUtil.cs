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
using System.Runtime.CompilerServices;

namespace AridityTeam.Mathematics
{
    /// <summary>
    /// Provides helper methods for common mathematical operations, 
    /// including clamping values and working with powers of two.
    /// </summary>
    public static class MathUtil
    {
#if NET472_OR_GREATER
        /// <summary>
        /// Clamps an integer value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int value, int min, int max) =>
            Math.Min(Math.Max(value, min), max);

        /// <summary>
        /// Clamps an integer value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static double Clamp(double value, double min, double max) =>
            Math.Min(Math.Max(value, min), max);

        /// <summary>
        /// Clamps an integer value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static long Clamp(long value, long min, long max) =>
            Math.Min(Math.Max(value, min), max);

        /// <summary>
        /// Clamps an integer value between a minimum and maximum value.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float value, float min, float max) =>
            Math.Min(Math.Max(value, min), max);
#endif

        /// <summary>
        /// Gets the nearest lower and higher powers of two relative to the given number.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <param name="lessPow2">The nearest power of two less than or equal to <paramref name="number"/>.</param>
        /// <param name="higherPow2">The nearest power of two greater than or equal to <paramref name="number"/>.</param>
        /// <returns><see langword="true"/> if a valid power of two was found; otherwise, <see langword="false"/>.</returns>
        public static bool GetNearestPow2(long number, out long lessPow2, out long higherPow2)
        {
            Requires.InRange(number, 0L, long.MaxValue);
            if (number <= 2L)
            {
                if (number == 0L)
                {
                    lessPow2 = higherPow2 = 0L;
                    return true;
                }
                lessPow2 = higherPow2 = number;
                return true;
            }
            if ((number & number - 1L) == 0L)
            {
                lessPow2 = higherPow2 = number;
                return true;
            }
            int num1 = 0;
            long num2 = number;
            while (num2 > 0L)
            {
                num2 >>= 1;
                ++num1;
            }
            higherPow2 = (long)(1 << num1);
            int num3 = num1 - 1;
            lessPow2 = (long)(1 << num3);
            return higherPow2 > 0L;
        }

        /// <summary>
        /// Gets the nearest power of two that is less than or equal to the specified number.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>The nearest lower power of two.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNearestLessPowOf2(long number)
        {
            GetNearestPow2(number, out long lessPow2, out long _);
            return lessPow2;
        }

        /// <summary>
        /// Gets the nearest power of two that is greater than or equal to the specified number.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>The nearest higher power of two.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the number is too large to find a valid power of two.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetNearestHigherPowOf2(long number)
        {
            if (!GetNearestPow2(number, out long _, out long higherPow2))
                throw new ArgumentOutOfRangeException(nameof(number), 
                    PrivateErrorHelpers.Format(SR.Math_TheNumberIsTooBig, number));
            return higherPow2;
        }

        /// <summary>
        /// Rounds a number to the nearest power of two.
        /// </summary>
        /// <param name="number">The number to round.</param>
        /// <returns>The nearest power of two.</returns>
        public static long RoundToNearestPowOf2(long number)
        {
            GetNearestPow2(number, out long lessPow2, out long higherPow2);
            return higherPow2 >= 0L && higherPow2 - number <= number - lessPow2 ? higherPow2 : lessPow2;
        }

        /// <summary>
        /// Computes the base-2 logarithm of a number, which must be a power of two.
        /// </summary>
        /// <param name="number">The number to compute the binary logarithm for.</param>
        /// <returns>The exponent such that 2^exponent equals <paramref name="number"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="number"/> is not a power of two.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="number"/> is out of range (0-<seealso cref="long.MaxValue"/>).</exception>
        public static long GetBinaryLogBase(long number)
        {
            Requires.InRange(number, 0L, long.MaxValue);
            int num1 = -1;
            long num2 = number;
            while (num2 > 0L)
            {
                num2 >>= 1;
                ++num1;
            }
            return number == (1 << num1) ? num1 : throw new ArgumentException(
                PrivateErrorHelpers.Format(SR.Math_TheNumberIsNotPow2, number));
        }
    }
}
