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

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Configuration
{
    /// <summary>
    /// Serializes, and deserializes the configuration file.
    /// </summary>
    public interface IConfigurationSerializer<T> where T : class, new()
    {
        /// <summary>
        /// Serializes the specified configuration instance.
        /// </summary>
        /// <param name="newConfig">The new configuration instance.</param>
        /// <param name="stream">The stream to write into.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that serializes the configuration into the specified stream.</returns>
        Task SerializeConfigAsync(T newConfig, Stream stream, CancellationToken token = default);

        /// <summary>
        /// Deserializes the specified configuration instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that deserializes the configuration from the specified stream.</returns>
        Task<T> DeserializeConfigAsync(Stream stream, CancellationToken token = default);
    }
}
