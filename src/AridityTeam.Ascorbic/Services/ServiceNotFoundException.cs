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

namespace AridityTeam.Services
{
    /// <summary>
	/// Thrown when the <seealso cref="IServiceManager"/> cannot find a required service.
	/// </summary>
	[Serializable]
    public class ServiceNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <seealso cref="ServiceNotFoundException"/> error.
        /// </summary>
        public ServiceNotFoundException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="ServiceNotFoundException"/> error with the specified service interface type.
        /// </summary>
        /// <param name="serviceType">The value of the service's interface type.</param>
        public ServiceNotFoundException(Type serviceType) : base("Required service not found: " + serviceType.FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="ServiceNotFoundException"/> error with the specified error message.
        /// </summary>
        /// <param name="message">The value of the error message.</param>
        public ServiceNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="ServiceNotFoundException"/> error with the specified error message and the reference
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        public ServiceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="ServiceNotFoundException"/> error with the serialized data.
        /// </summary>
        /// <param name="info">The <seealso cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <seealso cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [Obsolete]
        protected ServiceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
