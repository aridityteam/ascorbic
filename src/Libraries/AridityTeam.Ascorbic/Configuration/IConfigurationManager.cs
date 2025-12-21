/*
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
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Configuration
{
    /// <summary>
    /// Manages the current configuration by loading a XML configuration file from a file.
    /// </summary>
    public interface IConfigurationManager<T> where T : class
    {
        /// <summary>
        /// Gets the current configuration instance.
        /// </summary>
        T CurrentConfig { get; }

        #region File-based Operations

        /// <summary>
        /// Loads configuration from the configured file path asynchronously.
        /// Creates default configuration if the file is empty or missing.
        /// </summary>
        Task LoadConfigAsync(CancellationToken token = default);

        /// <summary>
        /// Saves configuration to the configured file path asynchronously.
        /// </summary>
        Task SaveConfigAsync(T newConfig, CancellationToken token = default);

        /// <summary>
        /// Synchronously loads configuration from the configured file path.
        /// </summary>
        void LoadConfig();

        /// <summary>
        /// Synchronously saves configuration to the configured file path.
        /// </summary>
        void SaveConfig(T newConfig);

        #endregion

        #region Stream-based Operations

        /// <summary>
        /// Loads configuration from a stream asynchronously.
        /// Does NOT dispose the stream.
        /// </summary>
        Task LoadConfigAsync(Stream stream, CancellationToken token = default);

        /// <summary>
        /// Saves configuration to a stream asynchronously.
        /// Does NOT dispose the stream.
        /// </summary>
        Task SaveConfigAsync(T newConfig, Stream stream, CancellationToken token = default);

        #endregion
    }
}
