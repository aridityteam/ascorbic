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
        /// The current loaded configuration.
        /// </summary>
        T CurrentConfig { get; }

        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <param name="configurationPath">The value of the configuration file path.</param>
        void LoadConfig(string configurationPath);

        /// <summary>
        /// Loads the configuration from a file asynchronously.
        /// </summary>
        /// <param name="configurationPath">The value of the configuration file path.</param>
        Task LoadConfigAsync(string configurationPath);

        /// <summary>
        /// Loads the configuration from a file asynchronously.
        /// </summary>
        /// <param name="configurationPath">The value of the configuration file path.</param>
        /// <param name="token">The value of the cancellation token.</param>
        Task LoadConfigAsync(string configurationPath, CancellationToken token = default);

        /// <summary>
        /// Saves the new configuration, and sets that new one as the current global configuration.
        /// </summary>
        /// <param name="newConfig">The value of the new configuration</param>
        void SaveConfig(T newConfig);

        /// <summary>
        /// Saves the new configuration asynchronously, and sets that new one as the current global configuration.
        /// </summary>
        /// <param name="newConfig">The value of the new configuration</param>
        Task SaveConfigAsync(T newConfig);

        /// <summary>
        /// Saves the new configuration asynchronously, and sets that new one as the current global configuration.
        /// </summary>
        /// <param name="newConfig">The value of the new configuration</param>
        /// <param name="token">The value of the cancellation token.</param>
        Task SaveConfigAsync(T newConfig, CancellationToken token = default);
    }
}
