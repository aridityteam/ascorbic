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
using System.Security.Cryptography;
using System.Text;

namespace AridityTeam.Util
{
    /// <summary>
    /// Provides extension methods for a regular <seealso cref="string"/>. 
    /// Hash generator methods provided by https://gist.github.com/rmacfie/828054/a2ed7ed1c023fbb7781e8f11ac69f2b0d780a717.
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// Combines a string with each specified string values.
        /// </summary>
        /// <param name="str">The value of the string to combine with.</param>
        /// <param name="args">The value of each strings to combine with the other one.</param>
        /// <returns>The combined string; otherwise the original string if no arguments specified.</returns>
        public static string CombineString(this string str, params object[] args)
        {
            if (string.IsNullOrEmpty(str))
                str = string.Empty;

            if (args is null || args.Length == 0)
                return str;

            var builder = new StringBuilder(str);
            foreach (var arg in args)
            {
                builder.Append(arg);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Throws an <seealso cref="ArgumentNullException"/> if the specified value is null.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the string is null.</exception>
        public static void NotNull(this string value) =>
            Requires.NotNull(value, nameof(value));

        /// <summary>
        /// Throws an <seealso cref="ArgumentNullException"/> if the specified value is null or an empty whitespace.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the string is null or an empty whitespace.</exception>
        public static void NotNullOrEmpty(this string value) =>
            Requires.NotNullOrEmpty(value, nameof(value));

        /// <summary>
        /// Throws an <seealso cref="ArgumentNullException"/> if the specified value is null or an empty whitespace.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the string is null or an empty whitespace.</exception>
        public static void NotNullOrWhiteSpace(this string value) =>
            Requires.NotNullOrWhiteSpace(value, nameof(value));

        /// <summary>
        /// Calculates the MD5 hash for the given string.
        /// </summary>
        /// <returns>A 32 char long hash.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
        public static string GetHashMd5(this string input) =>
            ComputeHash(HashType.Md5, input);

        /// <summary>
        /// Calculates the SHA-1 hash for the given string.
        /// </summary>
        /// <returns>A 40 char long hash.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
        public static string GetHashSha1(this string input) =>
            ComputeHash(HashType.Sha1, input);

        /// <summary>
        /// Calculates the SHA-256 hash for the given string.
        /// </summary>
        /// <returns>A 64 char long hash.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
        public static string GetHashSha256(this string input) =>
            ComputeHash(HashType.Sha256, input);

        /// <summary>
        /// Calculates the SHA-384 hash for the given string.
        /// </summary>
        /// <returns>A 96 char long hash.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
        public static string GetHashSha384(this string input) =>
            ComputeHash(HashType.Sha384, input);

        /// <summary>
        /// Calculates the SHA-512 hash for the given string.
        /// </summary>
        /// <returns>A 128 char long hash.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
        public static string GetHashSha512(this string input) =>
            ComputeHash(HashType.Sha512, input);

        /// <summary>
        /// Computes a normal string into a hash string specified by a type of hash to be encrypted.
        /// </summary>
        /// <param name="hashType">The value of the hash type.</param>
        /// <param name="input">The value of the normal string.</param>
        /// <returns>The encrypted hash string.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="input"/> is null or empty.</exception>
        public static string ComputeHash(HashType hashType, string input)
        {
            Requires.NotNullOrEmpty(input);

            var hasher = GetHasher(hashType);
            var inputBytes = Encoding.UTF8.GetBytes(input);

            var hashBytes = hasher.ComputeHash(inputBytes);
            var hash = new StringBuilder();
            foreach (var b in hashBytes)
            {
                hash.Append(string.Format("{0:x2}", b));
            }

            return hash.ToString();
        }

        private static HashAlgorithm GetHasher(HashType hashType) =>
            hashType switch
            {
                HashType.Md5 => MD5.Create(),
                HashType.Sha1 => SHA1.Create(),
                HashType.Sha256 => SHA256.Create(),
                HashType.Sha384 => SHA384.Create(),
                HashType.Sha512 => SHA512.Create(),
                _ => throw new ArgumentOutOfRangeException(nameof(hashType)),
            };
    }
}
