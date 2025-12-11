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

namespace AridityTeam.Logging
{
    /// <summary>
    /// Stores configuration and stuff for proper logging, these properties
    /// can be changed at any time.
    /// </summary>
    public class LoggerSettings
    {
        /// <summary>
        /// Default configuration for the logger.
        /// </summary>
        public static readonly LoggerSettings Default =
            new LoggerSettings(new Type[]{},LoggerDestination.Default, LoggerDestination.Error,
                false, MessageLevel.Info, "debug.log", true, new ILogEventListener[]{});

        /// <summary>
        /// Stores configuration and stuff for proper logging, these properties
        /// can be changed at any time.
        /// </summary>
        /// <param name="exclusions">The collection of type exclusions.</param>
        /// <param name="stdout">The value of the destination for normal messages. (info to warning)</param>
        /// <param name="stderr">The value of the destination for error messages. (error to fatal)</param>
        /// <param name="closeWriterOnDispose">Specifies whether to close the main output writer upon disposing the logger.</param>
        /// <param name="minLevel">The value of the minimum logger level.</param>
        /// <param name="logFileName">The value of the output file name.</param>
        /// <param name="enableColors">Specifies whether to enable log message colors.</param>
        /// <param name="listeners">The collection of log event listeners.</param>
        public LoggerSettings(Type[] exclusions, LoggerDestination stdout, LoggerDestination stderr,
            bool closeWriterOnDispose,
            MessageLevel minLevel,
            string logFileName, bool enableColors, ILogEventListener[] listeners)
        {
            Exclusions = exclusions;
            StdOut = stdout;
            StdErr = stderr;
            ShouldCloseWriterOnDispose = closeWriterOnDispose;
            ShouldCloseErrWriterOnDispose = closeWriterOnDispose;
            MinimumLevel = minLevel;
            LogFileName = logFileName;
            Colors = enableColors;
            LogEventListeners = new List<ILogEventListener>(listeners);
        }

        /// <summary>
        /// The current registered log event listeners.
        /// </summary>
        public List<ILogEventListener> LogEventListeners { get; set; }

        /// <summary>
        /// The file name of the logger output.
        /// </summary>
        public string LogFileName { get; set; }

        /// <summary>
        /// The main destination for normal messages. (info to warning)
        /// </summary>
        public LoggerDestination StdOut { get; set; }

        /// <summary>
        /// The main destination for error messages. (error to fatal)
        /// </summary>
        public LoggerDestination StdErr { get; set; }

        /// <summary>
        /// Specifies whether to close the output writer upon disposing the logger.
        /// The only exception to this is when the destination is redirected to a log file.
        /// </summary>
        public bool ShouldCloseWriterOnDispose { get; set; }

        /// <summary>
        /// Specifies whether to close the error writer upon disposing the logger.
        /// The only exception to this is when the destination is redirected to a log file.
        /// </summary>
        public bool ShouldCloseErrWriterOnDispose { get; set; }

        /// <summary>
        /// Specifies whether to enable color logging or not.
        /// </summary>
        public bool Colors { get; set; }

        /// <summary>
        /// The logger's minimum message level to be displayed.
        /// </summary>
        public MessageLevel MinimumLevel { get; set; }

        /// <summary>
        /// Types/classes to be skipped ahead of the file name checker.
        /// </summary>
        public Type[] Exclusions { get; set; }
    }
}
