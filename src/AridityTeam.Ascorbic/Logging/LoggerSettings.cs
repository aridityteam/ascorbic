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
using System.Runtime.Serialization;

using PolyType;

namespace AridityTeam.Logging
{
    /// <summary>
    /// Stores configuration and stuff for proper logging, these properties
    /// can be changed at any time.
    /// </summary>
    [Serializable]
    [GenerateShape]
    public partial class LoggerSettings
    {
        /// <summary>
        /// Default configuration for the logger.
        /// </summary>
        public static readonly LoggerSettings Default = new()
        {
            LogEventListeners = [],
            Exclusions = [],
            LogFileName = "debug.log",
            StdOut = LoggerDestination.Console,
            StdErr = LoggerDestination.Error,
            Colors = true,
            ShouldCloseWriterOnDispose = false,
            ShouldCloseErrWriterOnDispose = false
        };

        /// <summary>
        /// The current registered log event listeners.
        /// </summary>
        [DataMember]
        public List<ILogEventListener> LogEventListeners { get; set; } = [];

        /// <summary>
        /// The file name of the logger output.
        /// </summary>
        [DataMember]
        public string LogFileName { get; set; } = string.Empty;

        /// <summary>
        /// The main destination for normal messages. (info to warning)
        /// </summary>
        [DataMember]
        public LoggerDestination StdOut { get; set; }

        /// <summary>
        /// The main destination for error messages. (error to fatal)
        /// </summary>
        [DataMember]
        public LoggerDestination StdErr { get; set; }

        /// <summary>
        /// Specifies whether to close the output writer upon disposing the logger.
        /// The only exception to this is when the destination is redirected to a log file.
        /// </summary>
        [DataMember]
        public bool ShouldCloseWriterOnDispose { get; set; }

        /// <summary>
        /// Specifies whether to close the error writer upon disposing the logger.
        /// The only exception to this is when the destination is redirected to a log file.
        /// </summary>
        [DataMember]
        public bool ShouldCloseErrWriterOnDispose { get; set; }

        /// <summary>
        /// Specifies whether to enable color logging or not.
        /// </summary>
        [DataMember]
        public bool Colors { get; set; }

        /// <summary>
        /// The logger's minimum message level to be displayed.
        /// </summary>
        [DataMember]
        public MessageLevel MinimumLevel { get; set; }

        /// <summary>
        /// Types/classes to be skipped ahead of the file name checker.
        /// </summary>
        [DataMember]
        public List<Type> Exclusions { get; set; } = [];
    }
}
