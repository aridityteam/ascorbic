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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace AridityTeam
{
    /// <summary>
    /// Common runtime checks that throw <see cref="InternalErrorException" /> exceptions upon failure.
    /// </summary>
    public static partial class Assumes
    {
        /// <summary>
        /// Throws <see cref="InternalErrorException" /> if the specified value is null.
        /// </summary>
        /// <typeparam name="T">The type of value to test.</typeparam>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void NotNull<T>(T? value)
            where T : class
        {
            if (value is null)
            {
                FailNotNull<T>();
            }
        }

        /// <summary>
        /// Throws <see cref="InternalErrorException" /> if the specified value is null or empty.
        /// </summary>
        [DebuggerStepThrough]
        public static void NotNullOrEmpty(string? value)
        {
            NotNull(value);
            True(value?.Length > 0);
            True(value?[0] != '\0');
        }

        /// <summary>
        /// Throws <see cref="InternalErrorException" /> if the specified value is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of value to test.</typeparam>
        [DebuggerStepThrough]
        public static void NotNullOrEmpty<T>(ICollection<T?>? values)
        {
            Assumes.NotNull(values);
            Assumes.True(values?.Count > 0);
        }

        /// <summary>
        /// Throws <see cref="InternalErrorException" /> if the specified value is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of value to test.</typeparam>
        [DebuggerStepThrough]
        public static void NotNullOrEmpty<T>(IEnumerable<T> values)
        {
            Assumes.NotNull(values);

            bool isEmpty;
            if (values is ICollection<T> collection)
            {
                isEmpty = collection.Count == 0;
            }
            else if (values is IReadOnlyCollection<T> readOnlyCollection)
            {
                isEmpty = readOnlyCollection.Count == 0;
            }
            else
            {
                using IEnumerator<T> enumerator = values.GetEnumerator();
                isEmpty = !enumerator.MoveNext();
            }

            Assumes.False(isEmpty);
        }

        /// <summary>
        /// Throws <see cref="InternalErrorException" /> if the specified value is not null.
        /// </summary>
        /// <typeparam name="T">The type of value to test.</typeparam>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void Null<T>(T value)
            where T : class
        {
            if (value is not null)
            {
                FailNull<T>();
            }
        }

        /// <summary>
        /// Throws <see cref="InternalErrorException" /> if the specified object is not of a given type.
        /// </summary>
        /// <typeparam name="T">The type the value is expected to be.</typeparam>
        /// <param name="value">The value to test.</param>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void Is<T>(object value)
        {
            True(value is T);
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to true.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void False(bool condition, [CallerArgumentExpression(nameof(condition)), Localizable(false)] string? message = null)
        {
            if (condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to true.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void False(bool condition, [Localizable(false)] string unformattedMessage, object? arg1)
        {
            if (condition)
            {
                Fail(Format(unformattedMessage, arg1));
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to true.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void False(bool condition, [Localizable(false)] string unformattedMessage, params object?[] args)
        {
            if (condition)
            {
                Fail(Format(unformattedMessage, args));
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to true.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void False(bool condition, [InterpolatedStringHandlerArgument("condition")] ref ValidationInterpolatedStringHandlerInvertedCondition message)
        {
            if (condition)
            {
                Fail(message.ToStringAndClear());
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to false.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void True(bool condition, [CallerArgumentExpression(nameof(condition)), Localizable(false)] string? message = null)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to false.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void True(bool condition, [Localizable(false)] string unformattedMessage, object? arg1)
        {
            if (!condition)
            {
                Fail(Format(unformattedMessage, arg1));
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to false.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void True(bool condition, [Localizable(false)] string unformattedMessage, params object?[] args)
        {
            if (!condition)
            {
                Fail(Format(unformattedMessage, args));
            }
        }

        /// <summary>
        /// Throws an public exception if a condition evaluates to false.
        /// </summary>
        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static void True(bool condition, [InterpolatedStringHandlerArgument("condition")] ref ValidationInterpolatedStringHandler message)
        {
            if (!condition)
            {
                Fail(message.ToStringAndClear());
            }
        }

        /// <summary>
        /// Unconditionally throws an <see cref="InternalErrorException"/>.
        /// </summary>
        /// <returns>Nothing. This method always throws.</returns>
        [DebuggerStepThrough]
        public static Exception NotReachable()
        {
            // Keep these two as separate lines of code, so the debugger can come in during the assert dialog
            // that the exception's constructor displays, and the debugger can then be made to skip the throw
            // in order to continue the investigation.
            var exception = new InternalErrorException();
            bool proceed = true; // allows debuggers to skip the throw statement
            if (proceed)
            {
                throw exception;
            }
            else
            {
#pragma warning disable CS8763
                return new Exception();
#pragma warning restore CS8763
            }
        }

        /// <summary>
        /// Unconditionally throws an <see cref="InternalErrorException"/>.
        /// </summary>
        /// <typeparam name="T">The type that the method should be typed to return (although it never returns anything).</typeparam>
        /// <returns>Nothing. This method always throws.</returns>
        [DebuggerStepThrough]
        public static T? NotReachable<T>()
        {
            // Keep these two as separate lines of code, so the debugger can come in during the assert dialog
            // that the exception's constructor displays, and the debugger can then be made to skip the throw
            // in order to continue the investigation.
            var exception = new InternalErrorException();
            bool proceed = true; // allows debuggers to skip the throw statement
            if (proceed)
            {
                throw exception;
            }
            else
            {
#pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return.
                return default;
#pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return.
            }
        }

        /// <summary>
        /// Verifies that a value is not null, and throws <see cref="InternalErrorException" /> about a missing service otherwise.
        /// </summary>
        /// <typeparam name="T">The interface of the imported part.</typeparam>
        [DebuggerStepThrough]
        public static void Present<T>(T? component)
        {
            if (component is null)
            {
                Type coreType = PrivateErrorHelpers.TrimGenericWrapper(typeof(T), typeof(Lazy<>));
                Fail(Format(SR.ServiceMissing, coreType.FullName));
            }
        }

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <returns>Nothing, as this method always throws.  The signature allows for "throwing" Fail so C# knows execution will stop.</returns>
        [DebuggerStepThrough]
        public static Exception Fail([Localizable(false)] string? message = null)
        {
            var exception = new InternalErrorException(message);
            bool proceed = true; // allows debuggers to skip the throw statement
            if (proceed)
            {
                throw exception;
            }
            else
            {
#pragma warning disable CS8763
                return new Exception();
#pragma warning restore CS8763
            }
        }

        /// <summary>
        /// Throws an public exception.
        /// </summary>
        /// <returns>Nothing, as this method always throws.  The signature allows for "throwing" Fail so C# knows execution will stop.</returns>
        public static Exception Fail([Localizable(false)] string message, Exception innerException)
        {
            var exception = new InternalErrorException(message, innerException);
            bool proceed = true; // allows debuggers to skip the throw statement
            if (proceed)
            {
                throw exception;
            }
            else
            {
#pragma warning disable CS8763
                return new Exception();
#pragma warning restore CS8763
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void FailNotNull<T>()
        {
            Fail(Format(SR.UnexpectedNull, typeof(T).Name));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void FailNull<T>()
        {
            Fail(Format(SR.UnexpectedNonNull, typeof(T).Name));
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
