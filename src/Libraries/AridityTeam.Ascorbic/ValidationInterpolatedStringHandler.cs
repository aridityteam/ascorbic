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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;


namespace AridityTeam
{
    /// <summary>Provides an interpolated string handler for validation functions that only perform formatting if the condition check fails.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [InterpolatedStringHandler]
    public ref struct ValidationInterpolatedStringHandler
    {
        private StringBuilder? stringBuilder;

        /// <summary>Initializes a new instance of the <see cref="ValidationInterpolatedStringHandler"/> struct.</summary>
        /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
        /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
        /// <param name="condition">The condition Boolean passed to the method.</param>
        /// <param name="shouldAppend">A value indicating whether formatting should proceed.</param>
        /// <remarks>This is intended to be called only by compiler-generated code. Arguments are not validated as they'd otherwise be for members intended to be used directly.</remarks>
        public ValidationInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
        {
            if (condition)
            {
                this.stringBuilder = null;
                shouldAppend = false;
            }
            else
            {
                this.stringBuilder = new();
                shouldAppend = true;
            }
        }
        
        /// <summary>Writes the specified string to the handler.</summary>
        /// <param name="value">The string to write.</param>
        public void AppendLiteral(string value) => this.stringBuilder!.Append(value);

        /// <summary>Writes the specified value to the handler.</summary>
        /// <param name="value">The value to write.</param>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        public void AppendFormatted<T>(T value) => this.stringBuilder!.Append(value);

        /// <summary>Writes the specified value to the handler.</summary>
        /// <param name="value">The value to write.</param>
        /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
        /// <param name="format">The format string.</param>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        public void AppendFormatted<T>(T value, int alignment = 0, string? format = null)
        {
            string result = value is IFormattable ? ((IFormattable)value).ToString(format, null) : (value?.ToString() ?? string.Empty);
            bool left = alignment < 0;
            alignment = Math.Abs(alignment);
            if (result.Length < alignment)
            {
                string padding = new string(' ', alignment - result.Length);
                result = left ? result + padding : padding + result;
            }

            this.stringBuilder!.Append(result);
        }

        /// <summary>Extracts the built string from the handler.</summary>
        /// <returns>The formatted string.</returns>
        internal string ToStringAndClear()
        {
            string s = this.stringBuilder?.ToString() ?? string.Empty;
            this.stringBuilder = null;
            return s;
        }
    }
}
