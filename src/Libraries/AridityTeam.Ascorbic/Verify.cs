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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AridityTeam.Util;

namespace AridityTeam
{
    /// <summary>
    /// Common runtime checks that throw exceptions upon failure.
    /// </summary>
    public static class Verify
    {
        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if a condition is <see langword="false" />.
        /// </summary>
        /// <param name="condition">The value of the boolean condition.</param>
        /// <param name="message">The value of the error message.</param>
        /// <param name="conditionText">The value of the condition's name.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="condition"/> returned <see langword="false" />.</exception>
        [DebuggerStepThrough]
        public static void Operation(
            bool condition,
            string? message = null,
            [CallerArgumentExpression(nameof(condition))] string? conditionText = null)
        {
            if (!condition)
                throw new InvalidOperationException(message ?? 
                    Format(SR.Validation_OperationFailed, conditionText));
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the object is <see langword="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The value of the object instance.</param>
        /// <param name="message">The value of the error message.</param>
        /// <param name="expr">The value of the object's name</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> is <see langword="null" />.</exception>
        public static void NotNull<T>(
            [NotNull] T? value,
            string? message = null,
            [CallerArgumentExpression(nameof(value))] string? expr = null) where T : class
        {
            if (value is null)
                throw new InvalidOperationException(message ?? $"Expected non-null value: {expr}");
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null or empty.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="message">The value of the error message.</param>
        /// <param name="expr">The value of the object's name.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> is <see langword="null"/> or empty.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NotNullOrEmpty(
            [NotNull] string value,
            string? message = null,
            [CallerArgumentExpression(nameof(value))] string? expr = null)
        {
            if (value is null)
                throw new InvalidOperationException(message ?? $"Expected a non-null or non-empty value: {expr}");

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(SR.Validation_ValueEmpty, expr);

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null or an empty whitespace.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="message">The value of the error message.</param>
        /// <param name="expr">The value of the object's name.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> is <see langword="null"/> or empty.</exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NotNullOrWhiteSpace(
            [NotNull] string value,
            string? message = null,
            [CallerArgumentExpression(nameof(value))] string? expr = null)
        {
            if (value is null)
                throw new InvalidOperationException(message ?? $"Expected a non-null or non-empty value: {expr}");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(SR.Validation_ValueEmpty, expr);

            return value;
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if an object is disposed.
        /// </summary>
        /// <param name="disposedValue">The value of the disposable instance.</param>
        /// <param name="message">The value of the error message.</param>
        /// <exception cref="ObjectDisposedException">Thrown if <paramref name="disposedValue"/> has been already disposed.</exception>
        [DebuggerStepThrough]
        public static void NotDisposed(IDisposableObservable disposedValue, string? message = null)
        {
            Requires.NotNull(disposedValue, nameof(disposedValue));

            if (disposedValue.IsDisposed)
            {
                var objectName = disposedValue.GetType().FullName ?? string.Empty;
                if (message is object)
                {
                    throw new ObjectDisposedException(objectName, message);
                }
                else
                {
                    throw new ObjectDisposedException(objectName);
                }
            }
        }

        /// <summary>
        /// Throws an exception if the given value is negative.
        /// </summary>
        /// <param name="hresult">The HRESULT corresponding to the desired exception.</param>
        /// <param name="ignorePreviousComCalls">If true, prevents <c>ThrowExceptionForHR</c> from returning an exception from a previous COM call and instead always use the HRESULT specified.</param>
        /// <remarks>
        /// No exception is thrown for S_FALSE.
        /// </remarks>
        [DebuggerStepThrough]
        [System.Security.SecurityCritical]
        public static void HResult(int hresult, bool ignorePreviousComCalls = false)
        {
            if (hresult < 0)
            {
                if (ignorePreviousComCalls)
                {
                    Marshal.ThrowExceptionForHR(hresult, new IntPtr(-1));
                }
                else
                {
                    Marshal.ThrowExceptionForHR(hresult);
                }
            }
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
