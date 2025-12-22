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

namespace AridityTeam.Logging
{
    /// <summary>
    /// Defines the severity levels for log messages.
    /// </summary>
    /// <remarks>
    /// The levels are ordered from least severe (Debug) to most severe (Fatal).
    /// Each level represents a different degree of importance or urgency for the logged message.
    /// </remarks>
    public enum MessageLevel
    {
        /// <summary>
        /// Debug level messages used for detailed diagnostic information.
        /// </summary>
        /// <remarks>
        /// These messages are typically only useful during development and debugging.
        /// They provide detailed information about the internal state of the application.
        /// </remarks>
        Debug = 0,

        /// <summary>
        /// Information level messages for general application flow.
        /// </summary>
        /// <remarks>
        /// These messages provide general information about the application's operation
        /// and are useful for monitoring normal application behavior.
        /// </remarks>
        Info,

        /// <summary>
        /// Warning level messages for potentially harmful situations.
        /// </summary>
        /// <remarks>
        /// These messages indicate situations that might be problematic but don't
        /// necessarily indicate an error or failure.
        /// </remarks>
        Warn,

        /// <summary>
        /// Error level messages for error events that might still allow the application to continue running.
        /// </summary>
        /// <remarks>
        /// These messages indicate errors that have occurred but don't necessarily
        /// prevent the application from continuing its operation.
        /// </remarks>
        Err,

        /// <summary>
        /// Critical level messages for critical events that may prevent the application from functioning properly.
        /// </summary>
        /// <remarks>
        /// These messages indicate serious problems that may affect the application's
        /// ability to function correctly.
        /// </remarks>
        Crit,

        /// <summary>
        /// Fatal level messages for fatal events that will lead to application termination.
        /// </summary>
        /// <remarks>
        /// These messages indicate critical errors that will cause the application
        /// to terminate or become unusable.
        /// </remarks>
        Fatal
    }
}
