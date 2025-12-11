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

namespace AridityTeam.Util
{
    /// <summary>
    /// Parses command-line arguments.
    /// </summary>
    public class CommandLine
    {
        private List<string> _curArgs = new List<string>();

        /// <summary>
        /// Creates a new instance of the parser using the command-line arguments provided by the runtime.
        /// </summary>
        public CommandLine()
            : this(Environment.GetCommandLineArgs())
        {
        }

        /// <summary>
        /// Creates a new instance of the parser using the provided command-line arguments.
        /// </summary>
        /// <param name="args">The value of the command-line arguments.</param>
        public CommandLine(string[] args) =>
            _curArgs = new List<string>(args);

        /// <summary>
        /// Sets the current command-line arguments with the newly provided one.
        /// </summary>
        /// <param name="args">The value of the command-line arguments.</param>
        public void SetCommandLineArgs(string[] args) =>
            _curArgs = [.. args];

        /// <summary>
        /// Tries to find the specified parameter in the command-line arguments.
        /// </summary>
        /// <param name="parm">The value of the parameter to find.</param>
        /// <returns><see langword="true"/> if the specified parameter in the command-line arguments is found; otherwise <see langword="false"/>.</returns>
        public bool FindParm(string parm)
        {
            foreach (string arg in _curArgs)
            {
                if (arg.StartsWith(parm, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the parameter's value from the command-line arguments.
        /// </summary>
        /// <param name="parm">The value of the parameter to find and get.</param>
        /// <returns>If the specified parameter in the command-line arguments is found, it will return the value from it; otherwise <see langword="null"/>.</returns>
        public string? GetParm(string parm)
        {
            Requires.NotNullOrEmpty(parm);

            foreach (string arg in _curArgs)
            {
                if (arg.StartsWith(parm + '=', StringComparison.OrdinalIgnoreCase))
                    return arg.Substring(parm.Length + 1);

                if (arg.StartsWith(parm + ':', StringComparison.OrdinalIgnoreCase))
                    return arg.Substring(parm.Length + 1);
            }

            return null;
        }

        /// <summary>
        /// Returns the command-line args joined as a full string.
        /// </summary>
        /// <returns>The command-line args joined as a full string.</returns>
        public override string ToString() =>
            string.Join(" ", _curArgs);
    }
}
