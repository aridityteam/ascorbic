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
using System.IO;

namespace AridityTeam.Logging
{
    /// <summary>
    /// Defines a logging interface that provides methods for writing log messages at different severity levels.
    /// </summary>
    /// <remarks>
    /// This interface provides a contract for logging implementations that support multiple message levels
    /// and various formatting options. It extends <see cref="IDisposable"/> to allow proper resource cleanup.
    /// </remarks>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Gets the minimum logging level.
        /// </summary>
        /// <value>The actual message level.</value>
        MessageLevel MinimumLevel { get; }

        /// <summary>
        /// Gets the text writer used for standard output messages.
        /// </summary>
        /// <value>The text writer for standard output.</value>
        TextWriter OutputWriter { get; }

        /// <summary>
        /// Gets the text writer used for error output messages.
        /// </summary>
        /// <value>The text writer for error output.</value>
        TextWriter ErrWriter { get; }

        /// <summary>
        /// Registers an event listener into the current logger.
        /// </summary>
        /// <param name="listener">The value of the class inherited from <seealso cref="ILogEventListener"/>.</param>
        void RegisterEventListener(ILogEventListener listener);

        /// <summary>
        /// Unregisters an event listener from the current logger.
        /// </summary>
        /// <param name="listener">The value of the existing registered class inherited from <seealso cref="ILogEventListener"/>.</param>
        void UnregisterEventListener(ILogEventListener listener);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void LogInfo(string message);

        /// <summary>
        /// Logs an informational message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogInfo(string format, object arg0);

        /// <summary>
        /// Logs an informational message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogInfo(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an informational message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogInfo(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an informational message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogInfo(string format, params object[] args);

        /// <summary>
        /// Logs an warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void LogWarn(string message);

        /// <summary>
        /// Logs an warning message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogWarn(string format, object arg0);

        /// <summary>
        /// Logs an warning message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogWarn(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an warning message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogWarn(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an warning message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogWarn(string format, params object[] args);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void LogErr(string message);

        /// <summary>
        /// Logs an error message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogErr(string format, object arg0);

        /// <summary>
        /// Logs an error message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogErr(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an error message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogErr(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an error message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogErr(string format, params object[] args);

        /// <summary>
        /// Logs an critical message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void LogCrit(string message);

        /// <summary>
        /// Logs an critical message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogCrit(string format, object arg0);

        /// <summary>
        /// Logs an critical message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogCrit(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an critical message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogCrit(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an critical message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogCrit(string format, params object[] args);

        /// <summary>
        /// Logs an fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void LogFatal(string message);

        /// <summary>
        /// Logs an fatal message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogFatal(string format, object arg0);

        /// <summary>
        /// Logs an fatal message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogFatal(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an fatal message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogFatal(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an fatal message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogFatal(string format, params object[] args);

        /// <summary>
        /// Logs an debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void LogDebug(string message);

        /// <summary>
        /// Logs an debug message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogDebug(string format, object arg0);

        /// <summary>
        /// Logs an debug message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogDebug(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an debug message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogDebug(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an debug message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void LogDebug(string format, params object[] args);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        void Log(string message);

        /// <summary>
        /// Logs an informational message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(string format, object arg0);

        /// <summary>
        /// Logs an informational message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(string format, object arg0, object arg1);

        /// <summary>
        /// Logs an informational message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs an informational message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(string format, params object[] args);

        /// <summary>
        /// Logs a message at the specified level.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        void Log(MessageLevel level, string message);

        /// <summary>
        /// Logs a message at the specified level with a single argument.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(MessageLevel level, string format, object arg0);

        /// <summary>
        /// Logs a message at the specified level with two arguments.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(MessageLevel level, string format, object arg0, object arg1);

        /// <summary>
        /// Logs a message at the specified level with three arguments.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(MessageLevel level, string format, object arg0, object arg1, object arg2);

        /// <summary>
        /// Logs a message at the specified level with multiple arguments.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        void Log(MessageLevel level, string format, params object[] args);
    }
}
