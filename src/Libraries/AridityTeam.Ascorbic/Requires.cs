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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AridityTeam
{
    /// <summary>
    /// Common runtime checks that throw appropriate exceptions for validation failures.
    /// </summary>
    public static partial class Requires
    {
        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NotNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
            where T : class
        {
            if (value is null)
                throw new ArgumentNullException(parameterName);
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null or empty.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NotNullOrEmpty([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            if (value is null)
                throw new ArgumentNullException(parameterName);

            if (value.Length == 0)
                throw new ArgumentException(SR.Validation_ValueEmpty, parameterName);

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty or consists only of white-space characters.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NotNullOrWhiteSpace([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            if (value is null)
                throw new ArgumentNullException(parameterName);

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(SR.Validation_ValueEmptyOrWhitespace, parameterName);

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <returns>The value of the parameter.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// This method exists for callers who themselves only know the type as a generic parameter which
        /// may or may not be a class, but certainly cannot be null.
        /// </remarks>
        [DebuggerStepThrough]
        public static T NotNullAllowStructs<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the file specified isn't present.
        /// </summary>
        /// <param name="filePath">The value of the file's absolute path.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentException">Thrown if the specified file doesn't exist, or is not a valid absolute path.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filePath"/> is <see langword="null"/>.</exception>
        public static void FileExists(string filePath, [CallerArgumentExpression(nameof(filePath))] string? parameterName = null)
        {
            NotNullOrEmpty(filePath, parameterName);
            ValidFilePath(filePath, parameterName);

            if (!File.Exists(filePath))
                throw new ArgumentException(
                    Format(SR.Validation_FileMustExist,
                    filePath), parameterName);
        }

        /// <summary>
        /// Throws an exception if the directory specified isn't present.
        /// </summary>
        /// <param name="directory">The value of the file's absolute path.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentException">Thrown if the specified directory doesn't exist, or is not a valid absolute path.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="directory"/> is <see langword="null"/>.</exception>
        public static void DirectoryExists(string directory, [CallerArgumentExpression(nameof(directory))] string? parameterName = null)
        {
            NotNullOrEmpty(directory, parameterName);
            ValidDirectoryPath(directory, parameterName);

            if (!Directory.Exists(directory))
                throw new ArgumentException(
                    Format(SR.Validation_FileMustExist,
                    directory), parameterName);
        }

        /// <summary>
        /// Throws an exception if the specified parameters are not equal to each other.
        /// </summary>
        /// <param name="actual">The value of the actual argument.</param>
        /// <param name="expected">The value of the expected argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="actual"/> is not equal to <paramref name="expected"/>.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EqualTo<T>(T actual, T expected, [CallerArgumentExpression(nameof(actual))] string? parameterName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(actual, expected))
                throw new ArgumentException(
                    Format(SR.Validation_NotEqualTo,
                    expected, actual), parameterName);
        }

        /// <summary>
        /// Throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is false.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void True(bool condition, string? message = null)
        {
            if (!condition)
                throw new ArgumentException(message ?? SR.Validation_MustBeTrue);
        }

        /// <summary>
        /// Throws an exception if the condition is true.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="message">The message to include in the exception.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="condition"/> is true.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void False(bool condition, string? message = null)
        {
            if (condition)
                throw new ArgumentException(message ?? SR.Validation_MustBeFalse);
        }

        /// <summary>
        /// Throws an exception if the specified value is not in the specified range.
        /// </summary>
        /// <typeparam name="T">The type of the value to check.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not in the specified range.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InRange<T>(T value, T min, T max, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(parameterName, value,
                    Format(SR.Validation_ValueMustBeInRange,
                    min, max));
        }

        /// <summary>
        /// Throws an exception if the collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> is empty.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNullOrEmpty<T>([NotNull] IEnumerable<T>? collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
        {
            if (collection is null)
                throw new ArgumentNullException(parameterName);

            if (!collection.Any())
                throw new ArgumentException(SR.Validation_CollectionEmpty, parameterName);
        }

        /// <summary>
        /// Throws an exception if the string does not match the specified pattern.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> or <paramref name="pattern"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> does not match the pattern.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MatchesPattern(string value, string pattern, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            NotNull(pattern, nameof(pattern));

            if (!Regex.IsMatch(value, pattern))
                throw new ArgumentException(Format(SR.Validation_StringNotMatchingPattern,
                    pattern), parameterName);
        }

        /// <summary>
        /// Throws an exception if the string is not a valid email address.
        /// </summary>
        /// <param name="value">The email address to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid email address.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidEmail(string value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            var pattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            if (!pattern.IsMatch(value))
                throw new ArgumentException(SR.Validation_InvalidEmail, parameterName);
        }

        /// <summary>
        /// Throws an exception if the string is not a valid URL.
        /// </summary>
        /// <param name="value">The URL to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid URL.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidUrl(string value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            if (!Uri.TryCreate(value, UriKind.Absolute, out _))
                throw new ArgumentException(SR.Validation_InvalidURL, parameterName);
        }

        /// <summary>
        /// Throws an exception if the string length is not within the specified range.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="minLength">The minimum allowed length.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> length is not within the specified range.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LengthInRange(string value, int minLength, int maxLength, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            if (value.Length < minLength || value.Length > maxLength)
                throw new ArgumentOutOfRangeException(parameterName, value.Length,
                    Format(SR.Validation_LengthMustBeInRange, minLength, maxLength));
        }

        /// <summary>
        /// Throws an exception if the collection contains any null elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> contains any null elements.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoNullElements<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
            where T : class
        {
            NotNull(collection, parameterName);
            if (collection.Any(item => item is null))
                throw new ArgumentException(SR.Validation_HasNullElements, parameterName);
        }

        /// <summary>
        /// Throws an exception if the collection contains any duplicate elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="collection"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="collection"/> contains any duplicate elements.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NoDuplicates<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? parameterName = null)
        {
            NotNull(collection, parameterName);
            if (collection.Distinct().Count() != collection.Count())
                throw new ArgumentException(SR.Validation_HasCollectionDuplicates, parameterName);
        }

        /// <summary>
        /// Throws an exception if the value is not one of the specified valid values.
        /// </summary>
        /// <typeparam name="T">The type of the value to validate.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="validValues">The set of valid values.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="validValues"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not one of the valid values.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OneOf<T>(T value, IEnumerable<T> validValues, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(validValues, nameof(validValues));
            if (!validValues.Contains(value))
                throw new ArgumentException(Format(SR.Validation_NotOneOf, string.Join(", ", validValues)), parameterName);
        }

        /// <summary>
        /// Throws an exception if the value is not a valid enum value.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid enum value.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidEnum<T>(T value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
            where T : struct, Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
                throw new ArgumentException(Format(SR.Validation_InvalidEnum, typeof(T).Name), parameterName);
        }

        /// <summary>
        /// Throws an exception if the value is not a valid file path.
        /// </summary>
        /// <param name="value">The path to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid file path.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidFilePath(string value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            try
            {
                var path = Path.GetFullPath(value);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentException(SR.Validation_InvalidFile, parameterName);
            }
            catch (Exception)
            {
                throw new ArgumentException(SR.Validation_InvalidFile, parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the value is not a valid directory path.
        /// </summary>
        /// <param name="value">The path to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid directory path.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidDirectoryPath(string value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            try
            {
                var path = Path.GetFullPath(value);
                if (string.IsNullOrEmpty(path) || Path.HasExtension(path))
                    throw new ArgumentException(SR.Validation_InvalidDirectory, parameterName);
            }
            catch (Exception)
            {
                throw new ArgumentException(SR.Validation_InvalidDirectory, parameterName);
            }
        }

        /// <summary>
        /// Throws an exception if the value is not a valid IP address.
        /// </summary>
        /// <param name="value">The IP address to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid IP address.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IPAddress ValidIpAddress(string value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            NotNull(value, parameterName);
            if (!IPAddress.TryParse(value, out var ip))
                throw new ArgumentException(SR.Validation_InvalidIP, parameterName);
            return ip;
        }

        /// <summary>
        /// Throws an exception if the value is not a valid port number.
        /// </summary>
        /// <param name="value">The port number to validate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception. If this argument is omitted (explicitly writing <see langword="null" /> does not qualify), the expression used in the first argument will be used as the parameter name.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is not a valid port number.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidPort(int value, [CallerArgumentExpression(nameof(value))] string? parameterName = null)
        {
            if (value < 0 || value > 65535)
                throw new ArgumentOutOfRangeException(parameterName, value, SR.Validation_PortInvalid);
        }

        /// <summary>
        /// Helper method that formats string arguments.
        /// </summary>
        private static string Format(string format, params object?[] arguments)
        {
            return PrivateErrorHelpers.Format(format, arguments);
        }
    }
}
