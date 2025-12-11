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
using System.Diagnostics;

namespace AridityTeam
{
    /// <summary>
    /// Common runtime checks that trace messages and invoke an assertion failure,
    /// but does *not* throw exceptions.
    /// </summary>
    public static class Report
    {
        /// <summary>
        /// Verifies that a value is not null, and reports an error about a missing MEF component otherwise.
        /// </summary>
        /// <typeparam name="T">The interface of the imported part.</typeparam>
        [Conditional("DEBUG")]
        public static void IfNotPresent<T>(T part)
        {
            if (part is null)
            {
                Type coreType = PrivateErrorHelpers.TrimGenericWrapper(typeof(T), typeof(Lazy<>));
                Fail(Format(SR.ServiceMissing, coreType.FullName));
            }
        }

        /// <summary>
        /// Reports an error if a condition evaluates to true.
        /// </summary>
        [Conditional("DEBUG")]
        public static void If(bool condition, [Localizable(false)] string? message = null)
        {
            if (condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Reports an error if a condition does not evaluate to true.
        /// </summary>
        [Conditional("DEBUG")]
        public static void IfNot(bool condition, [Localizable(false)] string? message = null)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Reports an error if a condition does not evaluate to true.
        /// </summary>
        [Conditional("DEBUG")]
        public static void IfNot(bool condition, [Localizable(false)] string message, object arg1)
        {
            if (!condition)
            {
                Fail(PrivateErrorHelpers.Format(message, arg1));
            }
        }

        /// <summary>
        /// Reports an error if a condition does not evaluate to true.
        /// </summary>
        [Conditional("DEBUG")]
        public static void IfNot(bool condition, [Localizable(false)] string message, object arg1, object arg2)
        {
            if (!condition)
            {
                Fail(PrivateErrorHelpers.Format(message, arg1, arg2));
            }
        }

        /// <summary>
        /// Reports an error if a condition does not evaluate to true.
        /// </summary>
        [Conditional("DEBUG")]
        public static void IfNot(bool condition, [Localizable(false)] string message, params object[] args)
        {
            if (!condition)
            {
                Fail(PrivateErrorHelpers.Format(message, args));
            }
        }

        /// <summary>
        /// Reports an error if a condition does not evaluate to true.
        /// </summary>
        [Conditional("DEBUG")]
        public static void IfNot(bool condition, ref ValidationInterpolatedStringHandler message)
        {
            if (!condition)
            {
                Fail(message.ToStringAndClear());
            }
        }

        /// <summary>
        /// Reports a certain failure.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Fail([Localizable(false)] string? message = null)
        {
            if (message is null)
            {
                message = "A recoverable error has been detected.";
            }

            Debug.WriteLine(message);
            Debug.Assert(false, message);
        }

        /// <summary>
        /// Reports a certain failure.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Fail([Localizable(false)] string message, params object?[] args)
        {
            Fail(Format(message, args));
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
