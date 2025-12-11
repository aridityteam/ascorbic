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

namespace AridityTeam
{
    /// <summary>
    /// Extension methods for exceptions.
    /// </summary>
    public static class ExceptionEx
    {
        /// <summary>
        /// Adds data to the Data member of <paramref name="exception"/> before returning the modified exception.
        /// </summary>
        /// <typeparam name="T">The type of exception being modified.</typeparam>
        /// <param name="exception">The exception to add data to.</param>
        /// <param name="key">The key to use for the added data.</param>
        /// <param name="values">The values to add with the given <paramref name="key"/>.</param>
        /// <returns>A reference to the same <paramref name="exception"/>.</returns>
        /// <remarks>
        /// <para>This method should be used to add context (beyond the message and callstack we normally get) to the exception
        /// that would be useful when debugging Watson crashes.</para>
        /// <para>Do not use this method when you expect the exception to be handled.</para>
        /// </remarks>
        public static T AddData<T>(this T exception, string key, params object[] values)
            where T : Exception
        {
            Requires.NotNull(exception, nameof(exception));

            if (values?.Length > 0)
            {
                exception.Data.Add(key, values);
            }

            return exception;
        }
    }
}
