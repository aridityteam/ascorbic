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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AridityTeam.IO;

namespace AridityTeam.Logging
{
    /// <summary>
    /// Provides a concrete implementation of the <see cref="ILogger"/> interface that writes log messages
    /// to console output with detailed formatting including timestamp, file location, and message level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This logger implementation provides comprehensive logging capabilities with the following features:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Multiple message levels (Debug, Info, Warn, Error, Critical, Fatal)</description></item>
    /// <item><description>Formatted messages with timestamp and source file information</description></item>
    /// <item><description>String formatting support with multiple arguments</description></item>
    /// <item><description>Automatic stack trace analysis to determine calling location</description></item>
    /// <item><description>Separate output streams for different message levels</description></item>
    /// </list>
    /// <para>
    /// Debug messages are written to <seealso cref="Debug.WriteLine(string)"/>, while Error, Critical, and Fatal
    /// messages are written to the error stream. All other messages are written to the standard output stream.
    /// </para>
    /// </remarks>
    public class Logger : ILogger
    {
        private bool _disposed;
        private readonly LoggerSettings _settings;

        /// <summary>
        /// Creates a new instance of the logger with the default settings.
        /// </summary>
        public Logger()
            : this(LoggerSettings.Default)
        {
        }

        /// <summary>
        /// Creates a new instance of the logger with the specified settings.
        /// </summary>
        /// <param name="settings">The value of the logger settings.</param>
        public Logger(LoggerSettings settings)
        {
            _settings = settings;
            RegisterEventListener(new DebugLogEventListener());
        }

        /// <summary>
        /// Gets the minimum logging level.
        /// </summary>
        /// <value>The actual message level.</value>
        public MessageLevel MinimumLevel => _settings.MinimumLevel;

        /// <summary>
        /// Gets or sets the console colors for different log levels.
        /// </summary>
        public Dictionary<MessageLevel, (ConsoleColor Foreground, ConsoleColor Background)> LogLevelColors { get; } =
            new Dictionary<MessageLevel, (ConsoleColor Foreground, ConsoleColor Background)>()
        {
            { MessageLevel.Info, (ConsoleColor.Green, ConsoleColor.Black) },
            { MessageLevel.Warn, (ConsoleColor.Yellow, ConsoleColor.Black) },
            { MessageLevel.Err, (ConsoleColor.Red, ConsoleColor.Black) },
            { MessageLevel.Fatal, (ConsoleColor.White, ConsoleColor.Red) }
        };

        private StreamWriter? _fileWriter = null;
        private readonly List<ILogEventListener> _listeners = new List<ILogEventListener>();

        /// <summary>
        /// Gets the text writer used for standard output messages.
        /// </summary>
        /// <value>The console output writer.</value>
        public TextWriter OutputWriter
        {
            get
            {
                var writer = new MultiTextWriter();
                if (_settings.StdOut.HasFlag(LoggerDestination.Console))
                    writer.AddWriter(Console.Out);
                if (_settings.StdOut.HasFlag(LoggerDestination.Error))
                    writer.AddWriter(Console.Error);
                if (_settings.StdOut.HasFlag(LoggerDestination.File))
                    writer.AddWriter(CreateStreamWriter());

                return writer;
            }
        }

        /// <summary>
        /// Gets the text writer used for error output messages.
        /// </summary>
        /// <value>The console error writer.</value>
        public TextWriter ErrWriter
        {
            get
            {
                var writer = new MultiTextWriter();
                if (_settings.StdOut.HasFlag(LoggerDestination.Console))
                    writer.AddWriter(Console.Out);
                if (_settings.StdOut.HasFlag(LoggerDestination.Error))
                    writer.AddWriter(Console.Error);
                if (_settings.StdOut.HasFlag(LoggerDestination.File))
                    writer.AddWriter(CreateStreamWriter());

                return writer;
            }
        }

        private StreamWriter CreateStreamWriter()
        {
            if (_fileWriter != null) return _fileWriter;

            _fileWriter = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug.log"), true)
            {
                AutoFlush = true
            };
            return _fileWriter;
        }

        /// <summary>
        /// Registers an event listener into the current logger.
        /// </summary>
        /// <param name="listener">The value of the class inherited from <seealso cref="ILogEventListener"/>.</param>
        public void RegisterEventListener(ILogEventListener listener)
        {
            if (_listeners.Contains(listener)) return;
            _listeners.Add(listener);
        }

        /// <summary>
        /// Unregisters an event listener from the current logger.
        /// </summary>
        /// <param name="listener">The value of the existing registered class inherited from <seealso cref="ILogEventListener"/>.</param>
        public void UnregisterEventListener(ILogEventListener listener)
        {
            if (!_listeners.Contains(listener)) return;

            if (listener is IDisposable disposable)
                disposable.Dispose();

            _listeners.Remove(listener);
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void LogDebug(string message) => Log(MessageLevel.Debug, message);

        /// <summary>
        /// Logs a debug message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogDebug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0) => Log(MessageLevel.Debug, format, arg0);

        /// <summary>
        /// Logs a debug message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogDebug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1) => Log(MessageLevel.Debug, format, arg0, arg1);

        /// <summary>
        /// Logs a debug message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogDebug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1, object arg2) => Log(MessageLevel.Debug, format, arg0, arg1, arg2);

        /// <summary>
        /// Logs a debug message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogDebug([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Debug, format, args);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void LogInfo(string message) => Log(MessageLevel.Info, message);

        /// <summary>
        /// Logs an informational message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogInfo([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0) => Log(MessageLevel.Info, format, arg0);

        /// <summary>
        /// Logs an informational message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogInfo([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1) => Log(MessageLevel.Info, format, arg0, arg1);

        /// <summary>
        /// Logs an informational message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogInfo([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1, object arg2) => Log(MessageLevel.Info, format, arg0, arg1, arg2);

        /// <summary>
        /// Logs an informational message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogInfo([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Info, format, args);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void LogWarn(string message) => Log(MessageLevel.Warn, message);

        /// <summary>
        /// Logs a warning message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogWarn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0) => Log(MessageLevel.Warn, format, arg0);

        /// <summary>
        /// Logs a warning message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogWarn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1) => Log(MessageLevel.Warn, format, arg0, arg1);

        /// <summary>
        /// Logs a warning message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogWarn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1, object arg2) => Log(MessageLevel.Warn, format, arg0, arg1, arg2);

        /// <summary>
        /// Logs a warning message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogWarn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Warn, format, args);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void LogErr(string message) => Log(MessageLevel.Err, message);

        /// <summary>
        /// Logs an error message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogErr([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0) => Log(MessageLevel.Err, format, arg0);

        /// <summary>
        /// Logs an error message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogErr([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1) => Log(MessageLevel.Err, format, arg0, arg1);

        /// <summary>
        /// Logs an error message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogErr([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1, object arg2) => Log(MessageLevel.Err, format, arg0, arg1, arg2);

        /// <summary>
        /// Logs an error message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogErr([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Err, format, args);

        /// <summary>
        /// Logs a critical message.
        /// </summary>
        /// <param name="message">The critical message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void LogCrit(string message) => Log(MessageLevel.Crit, message);

        /// <summary>
        /// Logs a critical message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogCrit([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0) => Log(MessageLevel.Crit, format, arg0);

        /// <summary>
        /// Logs a critical message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogCrit([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1) => Log(MessageLevel.Crit, format, arg0, arg1);

        /// <summary>
        /// Logs a critical message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogCrit([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1, object arg2) => Log(MessageLevel.Crit, format, arg0, arg1, arg2);

        /// <summary>
        /// Logs a critical message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogCrit([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Crit, format, args);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The fatal message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void LogFatal(string message) => Log(MessageLevel.Fatal, message);

        /// <summary>
        /// Logs a fatal message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogFatal([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0) => Log(MessageLevel.Fatal, format, arg0);

        /// <summary>
        /// Logs a fatal message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogFatal([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1) => Log(MessageLevel.Fatal, format, arg0, arg1);

        /// <summary>
        /// Logs a fatal message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogFatal([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format,
            object arg0, object arg1, object arg2) => Log(MessageLevel.Fatal, format, arg0, arg1, arg2);

        /// <summary>
        /// Logs a fatal message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void LogFatal([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Fatal, format, args);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public void Log(string message) => Log(MessageLevel.Info, message);

        /// <summary>
        /// Logs an informational message with a single argument.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object arg0) =>
            Log(MessageLevel.Info, format, new object[] { arg0 });

        /// <summary>
        /// Logs an informational message with two arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object arg0, object arg1) =>
            Log(MessageLevel.Info, format, new object[] { arg0, arg1 });

        /// <summary>
        /// Logs an informational message with three arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <param name="arg1">The second argument to format.</param>
        /// <param name="arg2">The third argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object arg0, object arg1,
            object arg2) =>
            Log(MessageLevel.Info, format, new object[] { arg0, arg1, arg2 });

        /// <summary>
        /// Logs an informational message with multiple arguments.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object[] args) =>
            Log(MessageLevel.Info, format, args);

        /// <summary>
        /// Formats a log message with level, timestamp, file information, and the actual message.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="currentTime">The current timestamp.</param>
        /// <param name="fileName">The source file name.</param>
        /// <param name="lineNumber">The source line number.</param>
        /// <param name="message">The message to format.</param>
        /// <returns>A formatted log message string.</returns>
        /// <remarks>
        /// The formatted message follows the pattern: "LEVEL - HH-mm-ss-MM-dd-yyyy - filename:line - message"
        /// </remarks>
        private static string FormatMessage(MessageLevel level, DateTime currentTime, string fileName, int lineNumber, string message)
        {
            var sb = new StringBuilder();

            // log level
            sb.Append(level.ToString().ToUpperInvariant());

            sb.Append(" - ");

            // date time
            sb.Append($"{currentTime:HH-mm-ss-MM-dd-yyyy}");

            sb.Append(" - ");

            // file info
            sb.Append($"{fileName}:{lineNumber}");

            sb.Append(" - ");

            // main message
            sb.Append(message);

            return sb.ToString();
        }

        /// <summary>
        /// Logs a message at the specified level with automatic stack trace analysis to determine the calling location.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="message">The message to log.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <remarks>
        /// This method analyzes the stack trace to find the calling method that is not part of the Logger class,
        /// then formats and writes the message to the appropriate output stream based on the message level.
        /// </remarks>
        public void Log(MessageLevel level, string message)
        {
            if (!((int)level >= (int)MinimumLevel))
                return;

            var st = new StackTrace(true); // 'true' enables file info
            StackFrame? frame = null;

            var exclusions = new HashSet<Type>(_settings.Exclusions);

            for (int i = 1; i < st.FrameCount; i++)
            {
                var frameCandidate = st.GetFrame(i);
                var method = frameCandidate?.GetMethod();
                var declaringType = method?.DeclaringType;

                if (declaringType == null)
                    continue;

                if (declaringType == typeof(Logger) || exclusions.Contains(declaringType))
                    continue;

                frame = frameCandidate;
                break;
            }

            var file = Path.GetFileName(frame?.GetFileName()) ?? "unknown file";
            var line = frame?.GetFileLineNumber() ?? 0;

            if (_settings.Colors)
            {
                var colors = LogLevelColors.TryGetValue(level,
                    out (ConsoleColor Foreground, ConsoleColor Background) value)
                    ? value : (ConsoleColor.White, ConsoleColor.Black);

                var originalForeground = Console.ForegroundColor;
                var originalBackground = Console.BackgroundColor;

                try
                {
                    Console.ForegroundColor = colors.Item1;
                    Console.BackgroundColor = colors.Item2;

                    switch (level)
                    {
                        case MessageLevel.Debug:
                            Debug.WriteLine(FormatMessage(level, DateTime.Now,
                            file, line, message));
                            break;
                        case MessageLevel.Err:
                        case MessageLevel.Crit:
                        case MessageLevel.Fatal:
                            ErrWriter.WriteLine(FormatMessage(level, DateTime.Now,
                            file, line, message));
                            break;
                        default:
                            OutputWriter.WriteLine(FormatMessage(level, DateTime.Now,
                            file, line, message));
                            break;
                    }
                }
                finally
                {
                    Console.ForegroundColor = originalForeground;
                    Console.BackgroundColor = originalBackground;
                }
            }
            else
            {
                switch (level)
                {
                    case MessageLevel.Debug:
                        Debug.WriteLine(FormatMessage(level, DateTime.Now,
                        file, line, message));
                        break;
                    case MessageLevel.Err:
                    case MessageLevel.Crit:
                    case MessageLevel.Fatal:
                        ErrWriter.WriteLine(FormatMessage(level, DateTime.Now,
                        file, line, message));
                        break;
                    default:
                        OutputWriter.WriteLine(FormatMessage(level, DateTime.Now,
                        file, line, message));
                        break;
                }
            }

            foreach (var listener in _listeners)
            {
                listener.OnLogEvent(this, new LogEventArgs(this));
                listener.OnLogMessageEvent(this, new LogMessageEventArgs(this, message, level));
            }
        }

        /// <summary>
        /// Logs a message at the specified level with a single argument.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first argument to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] MessageLevel level, string format,
            object arg0) => Log(level, format, new object[] { arg0 });

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
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] MessageLevel level, string format,
            object arg0, object arg1) => Log(level, format, new object[] { arg0, arg1 });

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
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] MessageLevel level, string format,
            object arg0, object arg1, object arg2) => Log(level, format, new object[] { arg0, arg1, arg2 });

        /// <summary>
        /// Logs a message at the specified level with multiple arguments.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments to format.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="format"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="level"/> is not a valid <see cref="MessageLevel"/> value.</exception>
        /// <exception cref="FormatException">Thrown when the format string is invalid.</exception>
        /// <remarks>
        /// This method analyzes the stack trace to find the calling method that is not part of the Logger class,
        /// then formats and writes the message to the appropriate output stream based on the message level.
        /// </remarks>
        public void Log([StringSyntax(StringSyntaxAttribute.CompositeFormat)] MessageLevel level, string format, params object[] args)
        {
            if (!((int)level >= (int)MinimumLevel))
                return;

            var st = new StackTrace(true); // 'true' enables file info
            StackFrame? frame = null;

            var exclusions = new HashSet<Type>(_settings.Exclusions);

            for (int i = 1; i < st.FrameCount; i++)
            {
                var frameCandidate = st.GetFrame(i);
                var method = frameCandidate?.GetMethod();
                var declaringType = method?.DeclaringType;

                if (declaringType == null)
                    continue;

                if (declaringType == typeof(Logger) || exclusions.Contains(declaringType))
                    continue;

                frame = frameCandidate;
                break;
            }

            var file = Path.GetFileName(frame?.GetFileName()) ?? "unknown file";
            var line = frame?.GetFileLineNumber() ?? 0;

            if (_settings.Colors)
            {
                var colors = LogLevelColors.TryGetValue(level,
                    out (ConsoleColor Foreground, ConsoleColor Background) value)
                        ? value : (ConsoleColor.White, ConsoleColor.Black);

                var originalForeground = Console.ForegroundColor;
                var originalBackground = Console.BackgroundColor;

                try
                {
                    Console.ForegroundColor = colors.Item1;
                    Console.BackgroundColor = colors.Item2;

                    switch (level)
                    {
                        case MessageLevel.Debug:
                            Debug.WriteLine(FormatMessage(level, DateTime.Now,
                            file, line, string.Format(format, args)));
                            break;
                        case MessageLevel.Err:
                        case MessageLevel.Crit:
                        case MessageLevel.Fatal:
                            ErrWriter.WriteLine(FormatMessage(level, DateTime.Now,
                            file, line, string.Format(format, args)));
                            break;
                        default:
                            OutputWriter.WriteLine(FormatMessage(level, DateTime.Now,
                            file, line, string.Format(format, args)));
                            break;
                    }
                }
                finally
                {
                    Console.ForegroundColor = originalForeground;
                    Console.BackgroundColor = originalBackground;
                }
            }
            else
            {
                switch (level)
                {
                    case MessageLevel.Debug:
                        Debug.WriteLine(FormatMessage(level, DateTime.Now,
                        file, line, string.Format(format, args)));
                        break;
                    case MessageLevel.Err:
                    case MessageLevel.Crit:
                    case MessageLevel.Fatal:
                        ErrWriter.WriteLine(FormatMessage(level, DateTime.Now,
                        file, line, string.Format(format, args)));
                        break;
                    default:
                        OutputWriter.WriteLine(FormatMessage(level, DateTime.Now,
                        file, line, string.Format(format, args)));
                        break;
                }
            }

            foreach (var listener in _listeners)
            {
                listener.OnLogEvent(this, new LogEventArgs(this));
                listener.OnLogMessageEvent(this, new LogMessageEventArgs(this, string.Format(format, args), level));
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Logger and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        /// <remarks>
        /// If <paramref name="disposing"/> is true and the logger was constructed with <c>closeWriterOnDispose = true</c>,
        /// the output writer will be closed. Otherwise, only the unmanaged resources are released.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_settings.ShouldCloseWriterOnDispose
                        || _settings.StdOut.HasFlag(LoggerDestination.File))
                        OutputWriter.Close();
                    if (_settings.ShouldCloseErrWriterOnDispose
                        || _settings.StdErr.HasFlag(LoggerDestination.File))
                        ErrWriter.Close();

                    foreach (var listener in _listeners)
                        UnregisterEventListener(listener);
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the Logger.
        /// </summary>
        /// <remarks>
        /// This method calls the protected <see cref="Dispose(bool)"/> method with <c>disposing = true</c>
        /// and suppresses finalization of the object.
        /// </remarks>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
