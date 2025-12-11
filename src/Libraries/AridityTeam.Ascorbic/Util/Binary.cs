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
using System.Linq;
using System.Text;

namespace AridityTeam.Util
{
    /// <summary>
    /// Represents a binary value as a string and provides functionality to compare binary values.
    /// </summary>
    /// <remarks>The <see cref="Binary"/> class encapsulates a binary value represented as a string.  Instances of
    /// this class can be compared for equality using the <see cref="Equals(Binary?)"/> method.</remarks>
    public class Binary : IEquatable<Binary>, IComparable<Binary>
    {
        private readonly string _binText;

        /// <summary>
        /// Gets the binary string representation.
        /// </summary>
        public string Value => ToString();

        /// <summary>
        /// Gets the length of the binary string.
        /// </summary>
        public int Length => _binText.Length;

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Binary"/> class.
        /// </summary>
        /// <param name="binaryText">The value of the text that contains binary code.</param>
        public Binary(string binaryText)
        {
            Requires.NotNull(binaryText);
            _binText = ConvertStringToBinary(binaryText);
        }

        /// <summary>
        /// Converts a string to a <see cref="Binary"/> instance.
        /// </summary>
        public static implicit operator Binary(string binaryText) => new Binary(binaryText);

        /// <summary>
        /// Converts a <see cref="Binary"/> instance to a string literal.
        /// </summary>
        public static implicit operator string(Binary binary) => binary?.ToString() ?? string.Empty;

        /// <summary>
        /// Determines whether two <see cref="Binary"/> instances are equal.
        /// </summary>
        public static bool operator ==(Binary a, Binary b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null || b is null)
                return false;
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two <see cref="Binary"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Binary a, Binary b) => !(a == b);

        /// <summary>
        /// Tries to parse raw text, and outputs a new <seealso cref="Binary"/> instance.
        /// </summary>
        /// <param name="text">The value of the raw text.</param>
        /// <param name="result">The output value of the new instance.</param>
        /// <returns><see langword="true"/> if it successfully parsed raw text into <seealso cref="Binary"/>; otherwise obviously <see langword="false"/>.</returns>
        public static bool TryParse(string text, out Binary result)
        {
            if (string.IsNullOrEmpty(text))
            {
                result = null!;
                return false;
            }

            if (!IsBinaryValue(text))
            {
                result = new Binary(text);
                return true;
            }

            result = new Binary(text);
            return true;
        }

        private static bool IsBinaryValue(string text)
            => text.All(c => c == '0' || c == '1');

        /// <summary>
        /// Checks if the current <seealso cref="Binary"/> instance is equal to the other one, or not.
        /// </summary>
        /// <param name="other">The value of the other <seealso cref="Binary"/> instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the other instance; otherwise obviously <see langword="false"/>.</returns>
        public bool Equals(Binary? other) => _binText == other?._binText
            && GetHashCode() == other?.GetHashCode();

        /// <inheritdoc/>
        public int CompareTo(Binary? other)
        {
            if (other is null) return 1;
            return string.Compare(_binText, other._binText, StringComparison.Ordinal);
        }

        /// <summary>
        /// This serves a purpose, if the inputted text is not a binary value but a string, then we must try to
        /// convert it into binary.
        /// </summary>
        private static string ConvertStringToBinary(string data)
        {
            if (IsBinaryValue(data))
                return data;

            var sb = new StringBuilder(data.Length * 8);
            foreach (var c in data)
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));

            return sb.ToString();
        }

        /// <summary>
        /// Converts binary code into bytes.
        /// </summary>
        /// <returns>Multiple bytes, depending on the binary code's length.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the data length is not a multiple of 8.</exception>
        public byte[] ToBytes()
        {
            if (_binText.Length % 8 != 0)
                throw new InvalidOperationException("Binary data length must be a multiple of 8.");
            return Enumerable.Range(0, _binText.Length / 8)
                             .Select(i => Convert.ToByte(_binText.Substring(i * 8, 8), 2))
                             .ToArray();
        }

        /// <summary>
        /// Converts a binary value into a literal string.
        /// </summary>
        /// <returns>The raw string UTF-8 data of the binary code.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the data length is not a multiple of 8.</exception>
        public override string ToString()
        {
            if (_binText.Length % 8 != 0)
                throw new InvalidOperationException("Binary data length must be a multiple of 8.");

            var bytes = new byte[_binText.Length / 8];
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteStr = _binText.Substring(i * 8, 8);
                bytes[i] = Convert.ToByte(byteStr, 2);
            }

            return Encoding.UTF8.GetString(bytes);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as Binary);

        /// <inheritdoc/>
        public override int GetHashCode() =>
            _binText.GetHashCode();
    }
}
