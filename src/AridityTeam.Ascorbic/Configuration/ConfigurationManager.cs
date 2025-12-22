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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AridityTeam.Configuration
{
    /// <summary>
    /// Manages the current configuration by loading/saving XML configuration from file or stream.
    /// </summary>
    public class ConfigurationManager<T> : IConfigurationManager<T> where T : class, new()
    {
        private readonly XmlSerializerHelper<T> _helper = new();

        private T _currentConfig = null!;

        /// <summary>
        /// Gets the current configuration instance.
        /// </summary>
        public T CurrentConfig => _currentConfig;

        private readonly string _configurationPath;

        /// <summary>
        /// Initializes a new instance with default file path.
        /// </summary>
        public ConfigurationManager()
            : this(Path.Combine(
                  Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                  Assembly.GetEntryAssembly()?.GetName().Name ?? "App",
                  "config.cfg"))
        { }

        /// <summary>
        /// Initializes a new instance with a specified file path.
        /// </summary>
        public ConfigurationManager(string configurationPath)
        {
            if (string.IsNullOrWhiteSpace(configurationPath))
                throw new ArgumentException("Configuration path cannot be null or empty.", nameof(configurationPath));

            _configurationPath = configurationPath;
        }

        #region File-based Methods

        /// <summary>
        /// Loads configuration from the configured file path.
        /// </summary>
        public async Task LoadConfigAsync(CancellationToken token = default)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_configurationPath)!);

            using var fs = new FileStream(
                _configurationPath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.Read);

            await LoadConfigAsync(fs, token);
        }

        /// <summary>
        /// Saves configuration to the configured file path.
        /// </summary>
        public async Task SaveConfigAsync(T newConfig, CancellationToken token = default)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_configurationPath)!);

            using var fs = new FileStream(
                _configurationPath,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.Read);

            await SaveConfigAsync(newConfig, fs, token);
        }

        #endregion

        #region Stream-based Methods

        /// <summary>
        /// Loads configuration from a stream. Does NOT dispose the stream.
        /// </summary>
        public async Task LoadConfigAsync(Stream stream, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (stream.Length == 0)
            {
                // initialize default config if empty
                await SaveConfigAsync(new T(), stream, token);
                stream.Position = 0;
            }

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            _currentConfig = _helper.BytesToObject(ms.ToArray()) ?? new T();
        }

        /// <summary>
        /// Saves configuration to a stream. Does NOT dispose the stream.
        /// </summary>
        public async Task SaveConfigAsync(T newConfig, Stream stream, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            stream.Position = 0;

            using var sw = new StreamWriter(stream, Encoding.UTF8, 1024, leaveOpen: true);
            var serializer = new XmlSerializer(typeof(T));

            using var writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true, Async = true });
            serializer.Serialize(writer, newConfig);

            await sw.FlushAsync();
            stream.SetLength(stream.Position); // truncate leftover bytes
        }

        #endregion

        #region Synchronous Convenience Methods

        /// <summary>
        /// Loads configuration synchronously from the file.
        /// </summary>
        public void LoadConfig()
            => LoadConfigAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Saves configuration synchronously to the file.
        /// </summary>
        public void SaveConfig(T newConfig)
            => SaveConfigAsync(newConfig).GetAwaiter().GetResult();

        #endregion
    }
}
