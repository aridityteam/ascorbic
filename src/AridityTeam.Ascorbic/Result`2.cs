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
using System.Runtime.Serialization;

namespace AridityTeam
{
    /// <summary>
    /// Represents an indicator that the operation was sucessful, or not.
    /// </summary>
    [Serializable]
    public sealed class Result<T, TException> : IResult<T, TException>
        where T : class
        where TException : Exception, new()
    {
        #region Properties
        /// <summary>
        /// Gets the returned value after the operation has completed.
        /// </summary>
        [DataMember]
        public T? Value { get; private set; }

        /// <summary>
        /// Determines whether the operation was successful.
        /// </summary>
        [DataMember]
        public bool Successful { get; private set; }

        /// <summary>
        /// Determines whether the operation wasn't successful.
        /// </summary>
        [DataMember]
        public bool Failed { get; private set; }

        /// <summary>
        /// Gets the attached exception.
        /// </summary>
        [DataMember]
        public TException? Exception { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <seealso cref="Result{T}"/> class.
        /// </summary>
        public Result()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Result{T}"/> class.
        /// </summary>
        private Result(T? value)
            : this(value, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Result{T}"/> class.
        /// </summary>
        private Result(T? value, bool success)
            : this(value, success, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="Result{T}"/> class.
        /// </summary>
        private Result(T? value, bool success, TException? ex)
        {
            if (success && ex != null)
                throw new ArgumentException(
                    "A successful result cannot have an exception.", nameof(ex));

            if (!success && value != null)
                throw new ArgumentException(
                    "A failed result cannot have a value.", nameof(value));

            Value = value;
            Successful = success;
            Failed = !success;
            Exception = ex;
        }
        #endregion

        #region Methods/Helpers
        /// <summary>
        /// Throws an exception if the result indicates an failure.
        /// </summary>
        public void ThrowIfFailed()
        {
            if (Failed)
                throw Exception ?? new TException();
        }

        /// <summary>
        /// Tries to get the returned value from the final result.
        /// </summary>
        /// <param name="value">The output of the returned value.</param>
        /// <returns>
        /// <see langword="true"/> if the result indicates an success,
        /// and the <seealso cref="Value"/> is not <see langword="null"/>;
        /// otherwise <see langword="false"/> if not.
        /// </returns>
        public bool TryGetValue(out T value)
        {
            if (Successful && Value != null)
            {
                value = Value;
                return true;
            }

            value = null!;
            return false;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Determines whether the result indicate an success, or not.
        /// </summary>
        /// <param name="result">The value of the <seealso cref="Result{T, TException}"/> instance.</param>
        /// <returns><see langword="true"/> if the result indicates an success; otherwise <see langword="false"/> if it indicates an failure.</returns>
        public static implicit operator bool(Result<T, TException> result) =>
            result.Successful;

        /// <summary>
        /// Determines whether the result did not indicate an success, or not.
        /// </summary>
        /// <param name="result">The value of the <seealso cref="Result{T, TException}"/> instance.</param>
        /// <returns><see langword="true"/> if the result indicates an failure; otherwise <see langword="false"/> if it indicates an success.</returns>
        public static bool operator !(Result<T, TException> result) =>
            result.Failed;
        #endregion

        #region Static Methods/Helpers
        /// <summary>
        /// Initializes and returns a new <seealso cref="Result{T, TException}"/> instance that indicates an success.
        /// </summary>
        /// <param name="value">The value of the returned value.</param>
        /// <returns>A new <seealso cref="Result{T, TException}"/> instance that indicates an success.</returns>
        public static Result<T, TException> Success(T value)
        {
            Requires.NotNull(value);
            return new(value, true);
        }

        /// <summary>
        /// Initializes and returns a new <seealso cref="Result{T, TException}"/> instance that indicates an failure.
        /// </summary>
        /// <param name="ex">The value of the error data.</param>
        /// <returns>A new <seealso cref="Result{T, TException}"/> instance that indicates an success.</returns>
        public static Result<T, TException> Failure(TException? ex) =>
            new(null, false, ex);
        #endregion


        #region Overriden common methods
        /// <inheritdoc/>
        public override string ToString() =>
            Successful
                ? $"Success({Value})"
                : $"Failure({Exception?.GetType().Name})";

        /// <inheritdoc/>
        public override int GetHashCode() =>
            HashCode.Combine(Successful, Value, Exception);
        #endregion
    }
}
